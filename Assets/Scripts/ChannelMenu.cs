using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using System;

public class ChannelMenu : MonoBehaviour
{
    GameObject btn_prefab; 
    public Transform panel_pos;
    Channel_list channel_list;

    
    int index=0;
    int button_gap = 30;

    void Start()
    {
        btn_prefab = Resources.Load<GameObject> ("Prefabs/Channel_Button");

		if (btn_prefab ==null)
        { Debug.Log("mbtnpreprefab==null"); }
    }

    void Update()
    {
        
    }

    public void button_create()
    {
        GameObject button = Instantiate(btn_prefab, panel_pos);
        if (button==null)
        { Debug.Log("button==null"); }
        //RectTransform btnpos = button.GetComponent<RectTransform>();
        //channel_list.add_channel(button);
        
        //RectTransform을 잡기 위한 코드
        //button.transform.position = gameObject.transform.position;
        button.transform.localPosition = new Vector3 (((++index) * 50), 0, 0);
        /*
        //버튼프리팹의 최초 생성 postion값은 부모인 BtnGroup으로 맞춰줍니다.
        //텍스처 바꾸기
        Image image = button.GetComponent<Image>();
        
        //생성할 버튼 Image 컴포넌트에 접근
        Sprite btnsprite = Resources.Load<Sprite>(mstrImageName[nn]); 

        //nn이 0부터 3까지 도는 동안 각각에 맞는 이미지 sprite을 바꿔줍니다.
		image.sprite = btnsprite;

		btnpos.SetParent(gameObject.transform); 
        //SetParent 부모오브젝트 설정 여기서는 부모의 transform을 받기 위함입니다.

		btnpos.SetInsetAndSizeFromParentEdge(RectTransform.Edge.Left,(20*nn), 36);
        //왼쪽 모서리로 부터,얼마만큼 떨어져서 ,버튼의 크기
        //버튼의 크기는 만들어 놓은 프리팹의 크기로 맞춰주는게 좋습니다.
        */
    }
}


public class Channel_list : MonoBehaviour
{
    GameObject[] channel_list;
    int index;
    int button_gap = 30;
    
    void Init()
    {
        channel_list = new GameObject[10];
        index = 0;
    }

    void update_position()
    {
        //foreach (GameObject channel in channel_list) {}
    }

    public void add_channel(GameObject channel)
    {
        if (index<10)
        {
            channel_list[index++] = channel;
            Debug.Log(channel);
            Debug.Log(channel.transform);
            channel.transform.localPosition = new Vector3 (((index+1) * 50), 0, 0);
        }
    }
}
