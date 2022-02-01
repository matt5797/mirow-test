using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

using AWS;
using Newtonsoft.Json.Linq;
using System.Threading.Tasks;

namespace ChannelManager
{
	public class ChannelManager : MonoBehaviour
	{
        public static ChannelManager instance = null;
		//public ChannelQueue queue;
		public SortedSet<Channel> channel_set { get; private set; }
		int channel_buffer_max;

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

		// Start is called before the first frame update
		void Start()
		{
			channel_set = new SortedSet<Channel>(new ChannelComparer());
			channel_buffer_max = Channel_panel.instance.channel_buffer_max;
			test_channel_add();
			//JObject res = AWSManager.instance.channel_info_get("test");
			//test();
		}

		// Update is called once per frame
		void Update()
		{
			
		}

		void test_channel_add()
		{
			channel_set.Add(new Channel() { Priority = 0, Id = "test"});
			channel_set.Add(new Channel() { Priority = 100, Id = "test1"});
			channel_set.Add(new Channel() { Priority = 100, Id = "test2"});
			channel_set.Add(new Channel() { Priority = 200, Id = "test3"});
			channel_set.Add(new Channel() { Priority = 0, Id = "test4"});
		}

		public void top_channel_update()
		{
			Channel[] channel_array = new Channel[channel_buffer_max];
			IEnumerator<Channel> enumerator = channel_set.GetEnumerator();
			for (int i=0; i<channel_buffer_max; i++)
			{
				enumerator.MoveNext();
				if (enumerator.Current!=null)
				{
					channel_array[i] = enumerator.Current;
				}
			}
			Channel_panel.instance.button_update(channel_array);
		}
	}

	public class Channel
	{
		public int Priority { get; set; }
        public string Id { get; set; }
        public string Name { get; set; }
        public string Image_bucket { get; set; }
        public string Image_name { get; set; }
		Texture2D Image { get; set; }
		public bool selected { get; set; } = false;

		public void get_info()
		{
			if (Name==null)
			{
				JObject info = AWSManager.instance.channel_info_get(Id);
				if (((int)info["statusCode"])==200)
				{
					Name = info["body"]["name"].ToString();
					Image_bucket = info["body"]["image_bucket"].ToString();
					Image_name = info["body"]["image_name"].ToString();
				}
			}
		}

		public async Task<Texture2D> get_image()
		{
			if (Image==null)
			{
				if (Image_bucket==null)
					get_info();
				Debug.Log(Image_name);
				//Image = AWS.AWSManager.instance.GetTexture(Image_bucket, Image_name);
				//Task<Texture2D> task1 = AWS.AWSManager.instance.GetTextureAsync(Image_bucket, Image_name);
				//await task1;
				Image = await AWS.AWSManager.instance.GetTextureAsync(Image_bucket, Image_name);
			}
			return Image;
		}

		public override string ToString()
		{
			return Id;
		}
	}

	public class ChannelComparer : IComparer<object>
	{
		public int Compare(object x, object y)
		{
			Channel cx = x as Channel;
        	Channel cy = y as Channel;

			if (cx.Id==cy.Id)
				return 0;
			else if (cx.Priority>=cy.Priority)
				return -1;
			else
				return 1;
		}
	}
	
}