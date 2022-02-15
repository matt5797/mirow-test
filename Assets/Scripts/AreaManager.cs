using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Newtonsoft.Json.Linq;

public class AreaManager : MonoBehaviour 
{
    public static AreaManager instance = null;
    public float last_Lat; //마지막 위도
    public float last_Long; //마지막 경도
    public float current_Lat; //현재 위도
    public float current_Long; //현재 경도
    public float update_distance;

    private static WaitForSeconds second;
    private static bool gpsStarted = false;
    private static LocationInfo location;
    public int maxWait; //최대 대기 시간
    public int updateWait; //최대 대기 시간

    private void Awake () 
    {
        second = new WaitForSeconds(10.0f);
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
        StartCoroutine(GPSStart());
    }

    void Update()
    {
    }

    IEnumerator GPSStart () 
    {
        #if UNITY_EDITOR
        while (!UnityEditor.EditorApplication.isRemoteConnected)
        {
            yield return null;
        }
        #endif
        // 유저가 GPS 사용중인지 최초 체크
        if (!Input.location.isEnabledByUser) 
        {
            Debug.Log("GPS is not enabled");
            yield break;
        }

        //GPS 서비스 시작
        Input.location.Start();
        Debug.Log("Awaiting initialization");

        //활성화될 때 까지 대기
        maxWait = 20;
        while (Input.location.status == LocationServiceStatus.Initializing && maxWait > 0) 
        {
            yield return new WaitForSeconds(1.0f);
            maxWait -= 1;
        }

        //제한시간 지날경우 활성화 중단
        if (maxWait < 1) 
        {
            Debug.Log("Timed out");
            yield break;
        }

        //연결 실패
        if (Input.location.status == LocationServiceStatus.Failed) 
        {
            Debug.Log("Unable to determine device location");
            yield break;
        } 
        else 
        {
            updateWait = 20;
            //접근 허가됨
            gpsStarted = true;

            //현재 위치 갱신
            while (gpsStarted) 
            {
                location = Input.location.lastData;
                current_Lat = location.latitude * 1.0f;
                current_Long = location.longitude * 1.0f;
                if (updateWait<1)
                {
                    updateWait = 20;
                    last_Lat = current_Lat;
                    last_Long = current_Long;
                    if (GetDistance(current_Lat, current_Long, last_Lat, last_Long) > update_distance)
                    {
                        channel_list_update(last_Lat, last_Long);
                    }
                }
                yield return second;
                updateWait -= 1;
            }
        }
    }

    float GetDistance(float x1, float y1, float x2, float y2)
    {
        float width = x2 - x1;
        float height = y2 - y1;
        
        float distance = width * width + height * height;
        distance = Mathf.Sqrt(distance);

        return distance;
    }

    public async void channel_list_update(double last_Lat, double last_Long, double range=0.0035)
    {
        List<string> channel_list = new List<string>();
        JObject result = await AWS.AWSManager.instance.areaGetAsync(last_Lat, last_Long, range: range);
        foreach (JToken row in result["body"])
        {
            channel_list.Add(row[1].ToString());
        }
        ChannelManager.ChannelManager.instance.channel_list_update(channel_list);
    }

    public void GPSUpdate() {
        if (gpsStarted)
        {
            location = Input.location.lastData;
            current_Lat = location.latitude * 1.0f;
            current_Long = location.longitude * 1.0f;
        }
    }

    //위치 서비스 종료
    public static void GPSStop () {
        if (Input.location.isEnabledByUser) {
            gpsStarted = false;
            Input.location.Stop();
        }
    }
}
