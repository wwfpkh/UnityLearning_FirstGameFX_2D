using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CollectionScript : MonoBehaviour
{
    
    public AudioSource myCollectionEatAutioSource;     // 音频组件：被吃掉
    //【公共变量】
    public bool isEating;                          // 被吃掉
    ////---------------------------------------------Start 
    protected void Start()
    {
        myCollectionEatAutioSource = this.GetComponent<AudioSource>();      // 音频组件赋值
    }

    protected void Update()
    {
        IsEatingFun();
    }
        //【被吃掉函数】
        void IsEatingFun()
        {
            if (isEating)   // 若被吃掉
            {
                myCollectionEatAutioSource.Play(0);     // 播放被吃音效
                // Destroy(this.gameObject);
            }
        }
}
