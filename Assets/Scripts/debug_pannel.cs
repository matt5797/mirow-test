using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using System;
using System.IO;
using System.Net;
using Newtonsoft.Json.Linq;

public class debug_pannel : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void request_test()
    {
        Debug.Log("request test");

        //string targetURL = "http://data.ex.co.kr/openapi/safeDriving/forecast?key=test&type=json";
        string targetURL = "https://wjp4f8poye.execute-api.ap-northeast-2.amazonaws.com/dev/channel/info";
        string wbRequestResult = callWebRequest(targetURL);
        Debug.Log(wbRequestResult);
        var r = JObject.Parse(wbRequestResult);
        Debug.Log(r);
        var body = r["body"];
        Debug.Log(body["body1"]);
        
        //foreach (var o in list)
        //{
        //    Debug.Log(string.Format("{0} : {1}", "날짜", o["sdate"]));
        //}
    }

    public static string callWebRequest(string targetURL)
    {
        string responseFromServer = string.Empty;

        try
        {
            WebRequest request = WebRequest.Create(targetURL);
            request.Method = "GET";
            request.ContentType = "application/json";

            using (WebResponse response = request.GetResponse())
            using (Stream dataStream = response.GetResponseStream())
            using (StreamReader reader = new StreamReader(dataStream))
            {
                responseFromServer = reader.ReadToEnd();
            }

        }
        catch (Exception e)
        {
            Debug.Log(e.ToString());
        }

        return responseFromServer;
    }
}
