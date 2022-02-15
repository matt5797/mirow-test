using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

using ChannelManager;
using Helper;

public class ChannelMenuButton : MonoBehaviour
{
    ChannelManager.ChannelMenu channelMenu;
    public Tree<ChannelManager.ChannelMenu> channelMenuTree { get; set; }
    bool btn_selected = false;

    GameObject menu_prefab;
    public int menu_buffer_max { get; private set; }
    public List<GameObject> menu_list { get; private set; }
    public Vector3 menu_start;
    public Vector3 menu_gap;
    public Text text;
    private bool initialized = false;
    private bool menu_activated = false;

    public void init(Tree<ChannelManager.ChannelMenu> tree)
    {
        if (initialized)
            return;
        menu_buffer_max = 6;
        //menu_array = new GameObject[menu_buffer_max];
        menu_list = new List<GameObject>();

        menu_start = new Vector3(150, 0, 0);
        menu_gap = new Vector3(0, -60, 0);
        
        menu_prefab = Resources.Load<GameObject> ("Prefabs/ChannelMenuButton");
		if (menu_prefab ==null)
        { Debug.Log("ChannelMenuButton==null"); }

        channelMenuTree = tree;

        text.text = channelMenuTree.Node.text;

        menu_button_init();

        initialized = true;
    }
    
    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void menu_button_init()
    {
        int index = 0;

        foreach (Tree<ChannelManager.ChannelMenu> tree in channelMenuTree.Children)
        {
            menu_list.Add(menu_button_create(tree, index++));
        }
    }

    public GameObject menu_button_create(Tree<ChannelManager.ChannelMenu> menu, int index)
    {
        GameObject button = Instantiate(menu_prefab, this.transform);
        if (button==null)
        { Debug.Log("button==null"); }
        
        button.transform.localPosition = menu_start + (menu_gap * index);
        Component component = button.GetComponent<ChannelMenuButton>();
        
        button.GetComponent<ChannelMenuButton>().init(menu);

        return button;
    }

    public void menu_activate()
    {
        if (menu_activated)
        {
            foreach (GameObject menu in menu_list)
                menu.SetActive(false);
            menu_activated = false;
        }
        else
        {
            foreach (GameObject menu in menu_list)
                menu.SetActive(true);
            menu_activated = true;
        }
    }

    public void ButtonClick()
    {
        if (btn_selected)
        {
            //btn_selected = false;
            menu_activate();
        }
        else
        {
            menu_activate();
        }
    }
}
