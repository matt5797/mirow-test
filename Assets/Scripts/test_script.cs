using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using System;
using System.IO;
using System.Net;
using Newtonsoft.Json.Linq;

public class test_script : MonoBehaviour
{
    public GameObject parentObject;
    // Start is called before the first frame update
    void Start()
    {
        DeleteChilds();
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    public void DeleteChilds()
    {
        // child 에는 부모와 자식이 함께 설정 된다.
        Transform[] childlist = parentObject.GetComponentsInChildren<Transform>();

        foreach (var iter in childlist)
        {
            // 부모(this.gameObject)는 삭제 하지 않기 위한 처리
            if(iter != this.transform)
            {
                Destroy(iter.gameObject);
            }
        }
    }
}
