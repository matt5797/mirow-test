using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using System;
using System.IO;
using System.Net;
using Newtonsoft.Json.Linq;

using AWS;
using ChannelManager;

public class test_script : MonoBehaviour
{
    public GameObject parentObject;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void test()
    {
        SortedSet<Channel> channel_set_1 = new SortedSet<Channel>(new ChannelComparer());
        List<string> channel_list = new List<string>();
        List<Channel> temp_list = new List<Channel>();

        channel_set_1.Add(new Channel() { Priority = 100, Id = "a"});
        channel_set_1.Add(new Channel() { Priority = 100, Id = "b"});
        channel_set_1.Add(new Channel() { Priority = 100, Id = "c"});

        channel_list.Add("b");
        channel_list.Add("c");
        channel_list.Add("d");

        foreach (Channel ch in channel_set_1)
        {
            if (channel_list.Contains(ch.Id))
                channel_list.Remove(ch.Id);
            else
                temp_list.Add(ch);
        }

        foreach (Channel ch in temp_list)
            channel_set_1.Remove(ch);

        foreach (string id in channel_list)
            channel_set_1.Add(new Channel() { Priority = 0, Id = id});

        Debug.Log(channel_set_1);
        foreach (Channel ch in channel_set_1)
        {
            Debug.Log(ch.Id+" "+ch.Priority.ToString());
        }
    }

    private static bool demo(int i) {
      return (i == 500);
   }

    void test2()
    {
        SortedSet<Channel> channel_set_1 = new SortedSet<Channel>(new ChannelComparer());
        List<string> channel_list = new List<string>();
        List<Channel> temp_list = new List<Channel>();

        Channel a = new Channel() { Priority = 100, Id = "a"};
        Channel b = new Channel() { Priority = 100, Id = "b"};
        Channel c = new Channel() { Priority = 100, Id = "c"};
        Channel ra = a;

        channel_set_1.Add(a);
        channel_set_1.Add(b);
        channel_set_1.Add(c);

        foreach (Channel ch in channel_set_1)
        {
            Debug.Log(ch.Id+" "+ch.Priority.ToString());
        }

        Debug.Log(ra==a);

        var res = channel_set_1.Remove(a);
        Debug.Log(res + "제거");

        foreach (Channel ch in channel_set_1)
        {
            Debug.Log(ch.Id+" "+ch.Priority.ToString());
        }

        SortedSet<int> set = new SortedSet<int>();
        set.Add(100);
        set.Add(200);

        set.Remove(200);

        foreach (int i in set)
        {
            Debug.Log(i);
        }
    }

    public void test3()
    {
        SortedSet<Channel> channel_set_1 = new SortedSet<Channel>(new ChannelComparer());
        SortedSet<Channel> channel_set_2 = new SortedSet<Channel>(new ChannelComparer());

        channel_set_1.Add(new Channel() { Priority = 100, Id = "a"});
        channel_set_1.Add(new Channel() { Priority = 100, Id = "b"});
        channel_set_1.Add(new Channel() { Priority = 100, Id = "c"});

        channel_set_2.Add(new Channel() { Priority = 0, Id = "b"});
        channel_set_2.Add(new Channel() { Priority = 0, Id = "c"});
        channel_set_2.Add(new Channel() { Priority = 0, Id = "d"});

        Debug.Log("channel_set_1");
        foreach (Channel ch in channel_set_1)
        {
            Debug.Log(ch.Id+" "+ch.Priority.ToString());
        }

        //channel_set_1.IntersectWith(channel_set_2);   // 교집합
        //channel_set_1.SymmetricExceptWith(channel_set_2); // 합집합 - 교집합
        //channel_set_2.IntersectWith(channel_set_1);   // 교집합
        //channel_set_1.ExceptWith(channel_set_2);
        //channel_set_1.SetEquals(channel_set_2); //교집합 여부
        //channel_set_1.Overlaps();

        Debug.Log("channel_set_1");
        foreach (Channel ch in channel_set_1)
        {
            Debug.Log(ch.Id+" "+ch.Priority.ToString());
        }
        Debug.Log("channel_set_2");
        foreach (Channel ch in channel_set_2)
        {
            Debug.Log(ch.Id+" "+ch.Priority.ToString());
        }
    }

    public async void area_get_test()
    {
        JObject result = null;
        result = await AWS.AWSManager.instance.areaGetAsync(126.7335, 37.3402, range: 0.0035);
        Debug.Log(result);
    }
}
