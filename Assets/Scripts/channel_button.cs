using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

using ChannelManager;

public class channel_button : MonoBehaviour
{
    Channel channel;
    public Image button_image;
    public RawImage channel_image;
    public Sprite base_image;
    public Sprite selected_image;
    bool btn_selected = false;
    
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void button_select()
    {
        if (btn_selected)
        {
            btn_selected = false;
            channel.selected = false;
            button_image.sprite = base_image;
        }
        else
        {
            btn_selected = true;
            channel.selected = true;
            button_image.sprite = selected_image;
        }
    }

    public void channel_change(Channel input_channel)
    {
        channel = input_channel;
        channel_image_update();
    }

    void channel_image_update()
    {
        channel_image.texture = channel.get_image();
    }
}
