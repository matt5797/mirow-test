//
// Copyright 2014-2015 Amazon.com, 
// Inc. or its affiliates. All Rights Reserved.
// 
// Licensed under the AWS Mobile SDK For Unity 
// Sample Application License Agreement (the "License"). 
// You may not use this file except in compliance with the 
// License. A copy of the License is located 
// in the "license" file accompanying this file. This file is 
// distributed on an "AS IS" BASIS, WITHOUT WARRANTIES OR 
// CONDITIONS OF ANY KIND, express or implied. See the License 
// for the specific language governing permissions and 
// limitations under the License.
//

using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using Amazon.S3;
using Amazon.S3.Model;
using Amazon.Runtime;
using System.IO;
using System;
using Amazon.S3.Util;
using System.Collections.Generic;
using Amazon.CognitoIdentity;
using Amazon;
using System.Net;
using Newtonsoft.Json.Linq;
using System.Threading.Tasks;


namespace AWS
{
    public class AWSManager : MonoBehaviour
    {
        public static AWSManager instance = null;

        string IdentityPoolId = "ap-northeast-2:8654d235-2dd0-4346-88f9-bfc15c6ed0b2";
        string CognitoIdentityRegion = RegionEndpoint.APNortheast2.SystemName;
        private RegionEndpoint _CognitoIdentityRegion
        {
            get { return RegionEndpoint.GetBySystemName(CognitoIdentityRegion); }
        }
        string Region = RegionEndpoint.APNortheast2.SystemName;
        private RegionEndpoint _Region
        {
            get { return RegionEndpoint.GetBySystemName(Region); }
        }
        string S3BucketName = null;
        string SampleFileName = null;
        Button GetBucketListButton = null;
        Button PostBucketButton = null;
        Button GetObjectsListButton = null;
        Button DeleteObjectButton = null;
        Button GetObjectButton = null;
        Text ResultText = null;
        string BaseAPI = "https://wjp4f8poye.execute-api.ap-northeast-2.amazonaws.com/dev";

        private void Awake()
        {
            if (instance == null)
            {
                instance = this;
                DontDestroyOnLoad(gameObject);
            }
            else 
            {
                if (instance != this)
                    Destroy(this.gameObject);
            }
        }

        void Start()
        {
            //UnityInitializer.AttachToGameObject(this.gameObject);

            /*
            GetBucketListButton.onClick.AddListener(() => { GetBucketList(); });
            PostBucketButton.onClick.AddListener(() => { PostObject(); });
            GetObjectsListButton.onClick.AddListener(() => { GetObjects(); });
            DeleteObjectButton.onClick.AddListener(() => { DeleteObject(); });
            GetObjectButton.onClick.AddListener(() => { GetObject(); });
            */
        }

        #region private members

        private IAmazonS3 _s3Client;

        private AWSCredentials _credentials;

        private AWSCredentials Credentials
        {
            get
            {
                if (_credentials == null)
                    _credentials = new CognitoAWSCredentials(IdentityPoolId, _CognitoIdentityRegion);
                return _credentials;
            }
        }

        private IAmazonS3 S3Client
        {
            get
            {
                if (_s3Client == null)
                {
                    _s3Client = new AmazonS3Client(Credentials, _Region);
                }
                //test comment
                return _s3Client;
            }
        }

        #endregion

        public async Task GetObjectAsync(string S3BucketName, string FileName)
        {
            string responseBody = "";
            try
            {
                GetObjectRequest request = new GetObjectRequest
                {
                    BucketName = S3BucketName,
                    Key = FileName
                };
                using (GetObjectResponse response = await S3Client.GetObjectAsync(request))
                using (Stream responseStream = response.ResponseStream)
                using (StreamReader reader = new StreamReader(responseStream))
                {
                    string title = response.Metadata["x-amz-meta-title"]; // Assume you have "title" as medata added to the object.
                    string contentType = response.Headers["Content-Type"];
                    Console.WriteLine("Object metadata, Title: {0}", title);
                    Console.WriteLine("Content type: {0}", contentType);

                    responseBody = reader.ReadToEnd(); // Now you process the response body.
                }
            }
            catch (AmazonS3Exception e)
            {
                Console.WriteLine("Error encountered ***. Message:'{0}' when reading object", e.Message);
            }
            catch (Exception e)
            {
                Console.WriteLine("Unknown encountered on server. Message:'{0}' when reading object", e.Message);
            }
        }

        public async Task<Texture2D> GetTextureAsync(string S3BucketName, string FileName)
        {
            Texture2D tex = new Texture2D(2, 2);
            try
            {
                GetObjectRequest request = new GetObjectRequest
                {
                    BucketName = S3BucketName,
                    Key = FileName
                };
                using (GetObjectResponse response = await S3Client.GetObjectAsync(request))
                using (Stream responseStream = response.ResponseStream)
                {
                    byte[] data = null;

                    if (response.ResponseStream != null)
                    {
                        byte[] buffer = new byte[16 * 1024];
                        using (MemoryStream ms = new MemoryStream())
                        {
                            int read;
                            while ((read = responseStream.Read(buffer, 0, buffer.Length)) > 0)
                            {
                                ms.Write(buffer, 0, read);
                            }
                            data = ms.ToArray();
                        }
                        tex.LoadImage(data);
                    }
                }
            }
            catch (AmazonS3Exception e)
            {
                // If bucket or object does not exist
                Console.WriteLine("Error encountered ***. Message:'{0}' when reading object", e.Message);
            }
            catch (Exception e)
            {
                Console.WriteLine("Unknown encountered on server. Message:'{0}' when reading object", e.Message);
            }
            return tex;
        }

        public Texture2D bytesToTexture2D(byte[] imageBytes)
        {
            Texture2D tex = new Texture2D(2, 2);
            tex.LoadImage(imageBytes);
            return tex;
        }

        public async Task<JObject> channelInfoGet(string id)
        {
            string pathAPI = "/channel/info";
            JObject result = null;
            try
            {
                WebRequest request = WebRequest.Create(BaseAPI + pathAPI);
                request.Method = "GET";
                request.ContentType = "application/json";
                request.Headers.Add("id", id);

                using (WebResponse response = await request.GetResponseAsync())
                using (Stream dataStream = response.GetResponseStream())
                using (StreamReader reader = new StreamReader(dataStream))
                {
                    result = JObject.Parse(reader.ReadToEnd());
                }
            }
            catch (Exception e)
            {
                Debug.Log(e.ToString());
            }
            return result;
        }

        public async Task<JObject> areaGetAsync(float input_lat, float input_long, int srid=4166, float range=0.005f, int limit=20)
        {
            string pathAPI = "/area";
            JObject result = null;
            try
            {
                WebRequest request = WebRequest.Create(BaseAPI + pathAPI);
                request.Method = "GET";
                request.ContentType = "application/json";
                request.Headers.Add("lat", input_lat.ToString());
                request.Headers.Add("long", input_long.ToString());
                request.Headers.Add("srid", srid.ToString());
                request.Headers.Add("findRange", range.ToString());
                request.Headers.Add("limit", limit.ToString());

                using (WebResponse response = await request.GetResponseAsync())
                using (Stream dataStream = response.GetResponseStream())
                using (StreamReader reader = new StreamReader(dataStream))
                {
                    result = JObject.Parse(reader.ReadToEnd());
                }
            }
            catch (Exception e)
            {
                Debug.Log(e.ToString());
            }
            return result;
        }


        #region helper methods

        private string GetFileHelper()
        {
            var fileName = SampleFileName;

            if (!File.Exists(Application.persistentDataPath + Path.DirectorySeparatorChar + fileName))
            {
                var streamReader = File.CreateText(Application.persistentDataPath + Path.DirectorySeparatorChar + fileName);
                streamReader.WriteLine("This is a sample s3 file uploaded from unity s3 sample");
                streamReader.Close();
            }
            return fileName;
        }

        private string GetPostPolicy(string bucketName, string key, string contentType)
        {
            bucketName = bucketName.Trim();

            key = key.Trim();
            // uploadFileName cannot start with /
            if (!string.IsNullOrEmpty(key) && key[0] == '/')
            {
                throw new ArgumentException("uploadFileName cannot start with / ");
            }

            contentType = contentType.Trim();

            if (string.IsNullOrEmpty(bucketName))
            {
                throw new ArgumentException("bucketName cannot be null or empty. It's required to build post policy");
            }
            if (string.IsNullOrEmpty(key))
            {
                throw new ArgumentException("uploadFileName cannot be null or empty. It's required to build post policy");
            }
            if (string.IsNullOrEmpty(contentType))
            {
                throw new ArgumentException("contentType cannot be null or empty. It's required to build post policy");
            }

            string policyString = null;
            int position = key.LastIndexOf('/');
            if (position == -1)
            {
                policyString = "{\"expiration\": \"" + DateTime.UtcNow.AddHours(24).ToString("yyyy-MM-ddTHH:mm:ssZ") + "\",\"conditions\": [{\"bucket\": \"" +
                    bucketName + "\"},[\"starts-with\", \"$key\", \"" + "\"],{\"acl\": \"private\"},[\"eq\", \"$Content-Type\", " + "\"" + contentType + "\"" + "]]}";
            }
            else
            {
                policyString = "{\"expiration\": \"" + DateTime.UtcNow.AddHours(24).ToString("yyyy-MM-ddTHH:mm:ssZ") + "\",\"conditions\": [{\"bucket\": \"" +
                    bucketName + "\"},[\"starts-with\", \"$key\", \"" + key.Substring(0, position) + "/\"],{\"acl\": \"private\"},[\"eq\", \"$Content-Type\", " + "\"" + contentType + "\"" + "]]}";
            }

            return policyString;
        }

    }

    #endregion
    

}
