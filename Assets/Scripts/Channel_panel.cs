using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

using ChannelManager;

public class Channel_panel : MonoBehaviour
{
    public static Channel_panel instance = null;
    GameObject btn_prefab; 
    public Transform panel_pos;
    public int channel_buffer_max { get; private set; }
    public GameObject[] btn_array { get; private set; }
    public Vector3 btn_start;
    public Vector3 btn_gap;
    public GameObject selected_btn;

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
        btn_array = new GameObject[channel_buffer_max];

        btn_start = new Vector3(200, 0, 0);
        btn_gap = new Vector3(110, 0, 0);

        btn_prefab = Resources.Load<GameObject> ("Prefabs/Channel_Button");
		if (btn_prefab ==null)
        { Debug.Log("btnpreprefab==null"); }

        button_init();
    }

    void Update()
    {
        
    }

    public void button_init()
    {
        for (int i=0; i<channel_buffer_max; i++)
        {
            btn_array[i] = button_create(i);
        }
    }

    public GameObject button_create(int index)
    {
        GameObject button = Instantiate(btn_prefab, panel_pos);
        if (button==null)
        { Debug.Log("button==null"); }
        
        button.transform.localPosition = btn_start + (btn_gap * index);
        return button;
    }

    public void button_update(Channel[] channels)
    {
        for (int i=0; i<btn_array.Length; i++)
        {
            if (channels[i]==null)
            {
                btn_array[i].SetActive(false);
            }
            else
            {
                btn_array[i].GetComponent<channel_button>().channel_change(channels[i]);
                btn_array[i].SetActive(true);
            }
        }
    }
}

