using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

using ChannelManager;
using Helper;

public class ChannelPanel : MonoBehaviour
{
    public static ChannelPanel instance = null;
    Button btn_prefab; 
    public Transform panel_pos;
    public int channel_buffer_max { get; private set; }
    public List<Button> btn_list { get; private set; }
    public Vector3 btn_start;
    public Vector3 btn_gap;
    public Button selected_btn;

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
        channel_buffer_max = 4;
        btn_list = new List<Button>();

        btn_start = new Vector3(160, 0, 0);
        btn_gap = new Vector3(90, 0, 0);

        btn_prefab = Resources.Load<Button> ("Prefabs/ChannelButton");
		if (btn_prefab ==null)
        { Debug.Log("ChannelButton==null"); }

        button_init();
    }

    void Update()
    {
        
    }

    public void button_init()
    {
        for (int i=0; i<channel_buffer_max; i++)
            btn_list.Add(button_create(i));
    }

    public Button button_create(int index)
    {
        Button button = Instantiate(btn_prefab, panel_pos);
        if (button==null)
        { Debug.Log("button==null"); }
        
        //button.GetComponent<ChannelButton>().
        button.transform.localPosition = btn_start + (btn_gap * index);
        return button;
    }

    public void button_update(Channel[] channels)
    {
        int index = 0;
        foreach (Button button in btn_list)
        {
            if (channels[index]==null)
            {
                button.gameObject.SetActive(false);
            }
            else
            {
                button.GetComponent<ChannelButton>().channel_change(channels[index]);
                button.gameObject.SetActive(true);
            }
            index++;
        }
    }

    void button_active()
    {
        foreach (Button btn in btn_list)
        {
            //btn.GetComponent<ChannelButton>()
        }
    }
}

