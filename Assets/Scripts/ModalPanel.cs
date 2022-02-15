using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;
using System.Collections;

namespace ModalPanel
{
    public class EventButtonDetails 
    {
        public string buttonTitle;
        public Sprite buttonBackground;  // Not implemented
        public UnityAction action;
        public UnityEvent onEvent;
    }

    public class EventSliderDetails 
    {

    }

    public class ModalPanelDetails 
    {
        public string title; // Not implemented
        public string question;
        public Sprite iconImage;
        public Sprite panelBackgroundImage; // Not implemented
        public EventButtonDetails button1Details;
        public EventButtonDetails button2Details;
        public EventButtonDetails button3Details;
        public EventSliderDetails sliderDetails;
    }


    public class ModalPanel : MonoBehaviour 
    {

        public Text question;
        public Image iconImage;
        public Button button1;
        public Button button2;
        public Button button3;

        public Text button1Text;
        public Text button2Text;
        public Text button3Text;

        public GameObject modalPanelObject;

        private static ModalPanel modalPanel;
        public static ModalPanel instance = null;

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

        public static ModalPanel Instance () 
        {
            if (!modalPanel) 
            {
                modalPanel = FindObjectOfType(typeof (ModalPanel)) as ModalPanel;
                if (!modalPanel)
                    Debug.LogError ("There needs to be one active ModalPanel script on a GameObject in your scene.");
            }

            return modalPanel;
        }

        public void NewChoice (ModalPanelDetails details)
        {
            modalPanelObject.SetActive (true);

            this.iconImage.gameObject.SetActive(false);
            button1.gameObject.SetActive(false);
            button2.gameObject.SetActive(false);
            button3.gameObject.SetActive(false);

            this.question.text = details.question;

            if (details.iconImage) 
            {
                this.iconImage.sprite = details.iconImage;
                this.iconImage.gameObject.SetActive(true);
            }

            button1.onClick.RemoveAllListeners();
            button1.onClick.AddListener (details.button1Details.action);
            button1.onClick.AddListener (ClosePanel);
            button1Text.text = details.button1Details.buttonTitle;
            button1.gameObject.SetActive(true);

            if (details.button2Details != null) 
            {
                button2.onClick.RemoveAllListeners();
                button2.onClick.AddListener (details.button2Details.action);
                button2.onClick.AddListener (ClosePanel);
                button2Text.text = details.button2Details.buttonTitle;
                button2.gameObject.SetActive(true);
            }

            if (details.button3Details != null) 
            {
                button3.onClick.RemoveAllListeners();
                button3.onClick.AddListener (details.button3Details.action);
                button3.onClick.AddListener (ClosePanel);
                button3Text.text = details.button3Details.buttonTitle;
                button3.gameObject.SetActive(true);
            }
        }

        public int Choice (ModalPanelDetails details)
        {
            modalPanelObject.SetActive (true);

            this.iconImage.gameObject.SetActive(false);
            button1.gameObject.SetActive(false);
            button2.gameObject.SetActive(false);
            button3.gameObject.SetActive(false);

            this.question.text = details.question;

            if (details.iconImage) 
            {
                this.iconImage.sprite = details.iconImage;
                this.iconImage.gameObject.SetActive(true);
            }

            button1.onClick.RemoveAllListeners();
            button1.onClick.AddListener (details.button1Details.action);
            button1.onClick.AddListener (ClosePanel);
            button1Text.text = details.button1Details.buttonTitle;
            button1.gameObject.SetActive(true);

            if (details.button2Details != null) 
            {
                button2.onClick.RemoveAllListeners();
                button2.onClick.AddListener (details.button2Details.action);
                button2.onClick.AddListener (ClosePanel);
                button2Text.text = details.button2Details.buttonTitle;
                button2.gameObject.SetActive(true);
            }

            if (details.button3Details != null) 
            {
                button3.onClick.RemoveAllListeners();
                button3.onClick.AddListener (details.button3Details.action);
                button3.onClick.AddListener (ClosePanel);
                button3Text.text = details.button3Details.buttonTitle;
                button3.gameObject.SetActive(true);
            }
            return 1;
        }

        void ClosePanel () 
        {
            modalPanelObject.SetActive (false);    
        }
    }
}