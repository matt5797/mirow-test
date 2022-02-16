using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

using AWS;
using Newtonsoft.Json.Linq;
using System.Threading.Tasks;
using Helper;

namespace ChannelManager
{
	public class ChannelManager : MonoBehaviour
	{
        public static ChannelManager instance = null;
		//public ChannelQueue queue;
		public SortedSet<Channel> channel_set { get; private set; }
		int channel_buffer_max;
		//public event EventHandler ShapeChanged;

		protected virtual void OnChannelSetChanged()  
        {
			//ShapeChanged(this, e);
			top_channel_update();
			string res = "채널 리스트 출력";
			foreach (Channel ch in channel_set)
				res += "\n" + ch.Id + " " + ch.Priority;
			Debug.Log(res);
        }

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
			channel_buffer_max = ChannelPanel.instance.channel_buffer_max;
			//test_channel_add();
			//test_channel_update();
			//JObject res = AWSManager.instance.channel_info_get("test");
			//test();
			/*
			SortedSet<Channel> test = new SortedSet<Channel>(new ChannelComparer());
			Channel test1 = new Channel() { Priority = 0, Id = "test1"};
			Channel test2 = new Channel() { Priority = 200, Id = "test2"};
			Channel test3 = new Channel() { Priority = 100, Id = "test3"};
			test.Add(test1);
			test.Add(test2);
			test.Add(test3);
			foreach(Channel ch in test)
			{
				Debug.Log(ch.Id+" "+ch.Priority);
			}
			test3.Priority = -100;
			foreach(Channel ch in test)
			{
				Debug.Log(ch.Id+" "+ch.Priority);
			}
			*/
		}

		// Update is called once per frame
		void Update()
		{
			
		}

		public void channel_priority_change(string id, int priority)
		{
			bool changed = false;
			List<Channel> temp_list = new List<Channel>();
			foreach (Channel ch in channel_set)
			{
				if (ch.Id==id)
				{
					temp_list.Add(ch);
					//ch.Priority = priority;
					changed = true;
				}
			}
			if (changed)
			{
				foreach(Channel ch in temp_list)
				{
					channel_set.Remove(ch);
					ch.Priority = priority;
					channel_set.Add(ch);
				}
				OnChannelSetChanged();
			}
		}

		public void channel_list_update(List<String> channel_list)
		{
			List<Channel> temp_list = new List<Channel>();

			foreach (Channel ch in channel_set)
			{
				if (channel_list.Contains(ch.Id))
					channel_list.Remove(ch.Id);
				else
					temp_list.Add(ch);
			}

			foreach (Channel ch in temp_list)
				channel_set.Remove(ch);

			foreach (string channel_id in channel_list)
				channel_set.Add(new Channel() { Priority = 0, Id = channel_id});
			
			OnChannelSetChanged();
		}

		public void test_channel_add()
		{
			channel_set.Add(new Channel() { Priority = 0, Id = "e8dba2d4-c6d5-4e99-ac0d-fce066786caf"});
			//channel_set.Add(new Channel() { Priority = 200, Id = "0eb4f076-8adf-4639-b285-508da2a49d39"});
			//channel_set.Add(new Channel() { Priority = 100, Id = "6e776214-b144-4648-aefd-64851a9c1900"});
			//channel_set.Add(new Channel() { Priority = 100, Id = "8a684aaa-d79a-44e7-a68b-4122f560293c"});
			//channel_set.Add(new Channel() { Priority = 0, Id = "f88397a4-6c41-4a42-b868-40f26d4f071e"});
			//channel_set.Add(new Channel() { Priority = 0, Id = "f32db066-ff35-4b0a-97f4-264a46377bfa"});
			OnChannelSetChanged();
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
					Debug.Log(enumerator.Current.Id+" "+enumerator.Current.Priority);
					channel_array[i] = enumerator.Current;
				}
			}
			ChannelPanel.instance.button_update(channel_array);
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
		public Tree<ChannelMenu> menuTree { get; set; } = new Tree<ChannelMenu>();

		public async Task get_info()
		{
			if (Name==null)
			{
				JObject info = await AWSManager.instance.channelInfoGet(Id);
				if (((int)info["statusCode"])==200)
				{
					Name = info["body"]["name"].ToString();
					Image_bucket = info["body"]["image"]["image_bucket"].ToString();
					Image_name = info["body"]["image"]["image_name"].ToString();
					insert_menu(menuTree, info["body"]["menu"]);
				}
			}
		}

		void insert_menu(Tree<ChannelMenu> root, JToken children)
		{
			Tree<ChannelMenu> tree;
			foreach (JToken token in children)
			{
				tree = new Tree<ChannelMenu>();
				tree.Node = new ChannelMenu(token["id"].ToString(), token["name"].ToString());
				insert_menu(tree, token["children"]);
				root.Children.Add(tree);
			}
		}

		public async Task<Texture2D> get_image()
		{
			if (Image==null)
			{
				if (Image_bucket==null)
					await get_info();
				Image = await AWS.AWSManager.instance.GetTextureAsync(Image_bucket, Image_name);
			}
			return Image;
		}

		public override string ToString()
		{
			return Id;
		}
	}

	public class ChannelMenu
	{
		public string id { get; set; }
		public string text { get; set; }

		public ChannelMenu(string _id, string _text)
		{
			id = _id;
			text = _text;
		}

		public override string ToString()
		{
			return id;
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
			else if (cx.Priority>cy.Priority)
				return -1;
			else if (cx.Priority<cy.Priority)
				return 1;
			if (cx.GetHashCode()>cy.GetHashCode())
				return 1;
			else
				return -1;
		}
	}
}