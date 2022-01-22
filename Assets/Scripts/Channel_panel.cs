using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class Channel_panel : MonoBehaviour
{
    GameObject btn_prefab; 
    public Transform panel_pos;

    public static int btn_max = 5;
    GameObject[] btn_array = new GameObject[btn_max];
    public Vector3 btn_start;
    public Vector3 btn_gap;

    void Start()
    {
        btn_start = new Vector3(200, 0, 0);
        btn_gap = new Vector3(110, 0, 0);

        btn_prefab = Resources.Load<GameObject> ("Prefabs/Channel_Button");

		if (btn_prefab ==null)
        { Debug.Log("mbtnpreprefab==null"); }

        for (int i=0; i<btn_max; i++)
        {
            btn_array[i] = button_create(i);
        }
    }

    void Update()
    {
        
    }

    public GameObject button_create(int index)
    {
        GameObject button = Instantiate(btn_prefab, panel_pos);
        if (button==null)
        { Debug.Log("button==null"); }
        
        button.transform.localPosition = btn_start + (btn_gap * index);
        return button;
    }
}


namespace Channel
{
    public class Channel
    {
        string id;
        string name;

        void init()
        {

        }
    }
}