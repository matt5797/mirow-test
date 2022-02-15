using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

using System;
using System.IO;
using System.Net;
using Newtonsoft.Json.Linq;

using ChannelManager;

public class debug_pannel : MonoBehaviour
{
    public Text gqs_text;
    public RawImage test_image;
    int a = 0;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        gqs_text.text = String.Format("{0}, {1}", AreaManager.instance.current_Lat, AreaManager.instance.current_Long);
    }

    public void top_channel_update()
    {
        ChannelManager.ChannelManager.instance.top_channel_update();
    }

    public void test_channel_add()
    {
        ChannelManager.ChannelManager.instance.test_channel_add();
    }

    public void test_gps_change()
    {
        AreaManager.instance.channel_list_update(126.7335, 37.3402);
    }

    public void button_all_set_active(bool acive)
    {
        List<Button> btn_list = ChannelPanel.instance.btn_list;
        foreach (Button button in btn_list)
            button.gameObject.SetActive(acive);
    }

    public void channel_is_selected()
    {
        var a = ChannelManager.ChannelManager.instance.channel_set;
        Debug.Log(a);
        foreach (Channel ch in ChannelManager.ChannelManager.instance.channel_set)
        {
            Debug.Log("id:"+ch.Id+" select:"+ch.selected);
        }
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

    public async void image_test()
    {
        test_image.texture = await AWS.AWSManager.instance.GetTextureAsync("mirow-channel-image", "test_image1.png");
    }
}
