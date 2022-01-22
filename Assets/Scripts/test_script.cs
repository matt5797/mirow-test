using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using System;
using System.IO;
using System.Net;
using Newtonsoft.Json.Linq;

public class test_script : MonoBehaviour
{
    static string targetURL = "http://data.ex.co.kr/openapi/safeDriving/forecast?key=test&type=json";
    // Start is called before the first frame update
    void Start()
    {
        Debug.Log("I am alive!");

        string wbRequestResult = callWebRequest();
        var r2 = JObject.Parse(wbRequestResult);
        var list2 = r2["list"];

        Debug.Log(r2);
        foreach (var o in list2)
        {
            Debug.Log(string.Format("{0} : {1}", "날짜", o["sdate"]));
            Debug.Log(string.Format("{0} : {1}", "전국교통량", o["cjunkook"]));
            Debug.Log(string.Format("{0} : {1}", "지방교통량", o["cjibangDir"]));
            Debug.Log(string.Format("{0} : {1}", "대전->서울 버스 소요시간", o["cdjsu_bus"]));
            Debug.Log(string.Format("{0} : {1}", "대구->서울 버스 소요시간", o["cdgsu_bus"]));
            Debug.Log(string.Format("{0} : {1}", "울산->서울 버스 소요시간", o["cussu_bus"]));
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public static string callWebRequest()
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
