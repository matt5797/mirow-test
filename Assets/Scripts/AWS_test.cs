using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using Amazon;
using Amazon.S3;
using Amazon.S3.Model;
using Amazon.Runtime;


public class AWS_test : MonoBehaviour
{
    public string IdentityPoolId = "ap-northeast-2_NawkQ7PwO";
    public string CognitoIdentityRegion = RegionEndpoint.APNortheast2.SystemName;
    private RegionEndpoint _CognitoIdentityRegion
    {
        get { return RegionEndpoint.GetBySystemName(CognitoIdentityRegion); }
    }
    public string S3Region = RegionEndpoint.APNortheast2.SystemName;
    private RegionEndpoint _S3Region
    {
        get { return RegionEndpoint.GetBySystemName(S3Region); }
    }
    public string S3BucketName = null;
    public string SampleFileName = null;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void s3_test()
    {
        //AmazonS3Client S3Client = new AmazonS3Client (credentials);

    }
}
