using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

using ChannelManager;
using Helper;
using ModalPanel;
using System.Threading.Tasks;

public class ChannelButton : MonoBehaviour
{
    Channel channel;
    public Image button_image;
    public RawImage channel_image;
    public Sprite base_image;
    public Sprite selected_image;
    bool btn_selected = false;

    Tree<ChannelManager.ChannelMenu> menuTree;
    Button menu_prefab;
    public int menu_buffer_max { get; private set; }
    public List<Button> menu_list { get; private set; }
    public Vector3 menu_start;
    public Vector3 menu_gap;
    private bool menu_activated = false;
    float clickTime;
    float minClickTime = 1;
    bool isClick;

    /*
    void init()
    {
        if (initialized)
            return;
        menu_buffer_max = 6;
        menu_list = new List<Button>();

        menu_start = new Vector3(0, -80, 0);
        menu_gap = new Vector3(0, -60, 0);
        
        menu_prefab = Resources.Load<Button> ("Prefabs/ChannelMenuButton");
		if (menu_prefab ==null)
        { Debug.Log("ChannelMenuButton==null"); }
        
        initialized = true;
    }
    */

    void Awake()
    {
        menu_buffer_max = 6;
        menu_list = new List<Button>();

        menu_start = new Vector3(0, -80, 0);
        menu_gap = new Vector3(0, -60, 0);
        
        menu_prefab = Resources.Load<Button> ("Prefabs/ChannelMenuButton");
		if (menu_prefab ==null)
        { Debug.Log("ChannelMenuButton==null"); }
    }
    
    // Start is called before the first frame update
    void Start()
    {
    }

    // Update is called once per frame
    void Update()
    {
        if (isClick)
        {
            clickTime += Time.deltaTime;
            if (clickTime>=minClickTime)
            {
                channel_asynchronous();
                isClick=false;
            }
        }
        else
        {
            clickTime = 0;
        }
    }

    void resetButton()
    {
        menu_list = new List<Button>();
        btn_selected = false;
        menu_activated = false;
        menuTree = null;
        button_image.sprite = base_image;
    }

    public void menu_button_init()
    {
        int index = 0;

        foreach (Tree<ChannelManager.ChannelMenu> tree in menuTree.Children)
            menu_list.Add(menu_button_create(tree, index++));
    }

    public Button menu_button_create(Tree<ChannelManager.ChannelMenu> menu, int index)
    {
        Button button = Instantiate(menu_prefab, this.transform);
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
            foreach (Button menu in menu_list)
                menu.gameObject.SetActive(false);
            menu_activated = false;
        }
        else
        {
            foreach (Button menu in menu_list)
            {
                menu.gameObject.SetActive(true);
            }
            menu_activated = true;
        }
    }

    void channel_synchronous()
    {
        //채널 동기화
        channel.selected = true;
        button_image.sprite = selected_image;
    }

    void channel_asynchronous()
    {
        //채널 비동기화
        int res = asynchronous_modal();
        Debug.Log("모달 결과:"+res);
        if (res==1)
        {
            channel.selected = false;
            button_image.sprite = base_image;
        }
    }

    public int asynchronous_modal () {
        ModalPanelDetails modalPanelDetails = new ModalPanelDetails {question = "해당 채널과 비동기화 하시겠습니까?"};
        modalPanelDetails.button1Details = new EventButtonDetails {
            buttonTitle = "네",
            action = () => { }
        };
        modalPanelDetails.button2Details = new EventButtonDetails {
            buttonTitle = "아니요",
            action = () => { }
        };
        //ModalPanel.ModalPanel.instance.NewChoice(modalPanelDetails);
        return ModalPanel.ModalPanel.instance.Choice(modalPanelDetails);
    }

    public void ButtonClick()
    {
        if (!btn_selected)
        {
            btn_selected = true;
            channel_synchronous();
        }
        else
        {
            menu_activate();
        }
    }

    public void ButtonDown()
    {
        isClick = true;
    }
    public void ButtonUp()
    {
        clickTime = 0;
        isClick = false;
    }

    public async void channel_change(Channel input_channel)
    {
        resetButton();
        channel = input_channel;
        await channel_image_update();
        channel_menu_update();
    }

    async Task channel_image_update()
    {
        channel_image.texture = await channel.get_image();
    }

    void channel_menu_update()
    {
        menuTree = channel.menuTree;
        menu_button_init();
    }
}
