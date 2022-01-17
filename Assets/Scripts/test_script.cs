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
        string webClientResult = callWebClient();
        var r = JObject.Parse(webClientResult);
        var list = r["list"];

        Debug.Log(r);
        foreach (var o in list)
        {
            Debug.Log(string.Format("{0} : {1}", "날짜"               , o["sdate"]));
            Debug.Log(string.Format("{0} : {1}", "전국교통량"         , o["cjunkook"]));
            Debug.Log(string.Format("{0} : {1}", "지방교통량"         , o["cjibangDir"]));
            Debug.Log(string.Format("{0} : {1}", "서울->대전 소요시간" , o["csudj"]));
            Debug.Log(string.Format("{0} : {1}", "서울->대구 소요시간" , o["csudg"]));
            Debug.Log(string.Format("{0} : {1}", "서울->울산 소요시간" , o["csuus"]));
        }

        Debug.Log("*************************************************************");

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

    public static string callWebClient()
    {
        string result = string.Empty;
        try
        {
            WebClient client = new WebClient();

            //특정 요청 헤더값을 추가해준다. 
            //client.Headers.Add("user-agent", "Mozilla/4.0 (compatible; MSIE 6.0; Windows NT 5.2; .NET CLR 1.0.3705;)");

            using (Stream data = client.OpenRead(targetURL))
            {
                using (StreamReader reader = new StreamReader(data))
                {
                    string s = reader.ReadToEnd();
                    result = s;

                    reader.Close();
                    data.Close();
                }
            }

        }
        catch (Exception e)
        {
            //통신 실패시 처리로직
            Debug.Log(e.ToString());
        }
        return result;
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
