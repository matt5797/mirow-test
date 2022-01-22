using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class popup_panel : MonoBehaviour
{
    public GameObject popupPanel;
    public bool panel_active = false;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void active_change()
    {
        if (gameObject.activeSelf == true)
        {
            popupPanel.SetActive(false);
        }
        else
        {
            popupPanel.SetActive(true);
        }
    }
}
