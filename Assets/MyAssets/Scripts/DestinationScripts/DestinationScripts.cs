using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

// using UnityEngine.SceneManagement;

public class DestinationScripts : MonoBehaviour
{
    public GameObject panelUI;      // Object组件
    public int nextSceneIndex;      // 下一个场景的编号
    private bool isHitDestination;  // 是否接触到目的地

    ////--------------------------UpDate
    private void Update()
    {
        if (isHitDestination)   // 当接触到目的地
        {
            if (Input.GetKeyDown(KeyCode.E))    // 当按下 E 键时
            {
                SceneManager.LoadScene(nextSceneIndex);     // 加载下一个场景
            }
        }

    }


    ////--------------------------------------------------------Event Function 事件触发函数
    ///【到达目的地】
    private void OnTriggerEnter2D(Collider2D col)       // 当box和Trigger相撞
    {
        
        if (col.CompareTag("Player"))       // 若是Palyer遇到
        {
            panelUI.SetActive(true);     // 开启panelUI
        }
    }

    ///【离开目的地】
    private void OnTriggerExit2D(Collider2D other)      // 当box和Trigger从相撞到不相撞
    {
        isHitDestination = false;       // 没有接触到目的地
        if (other.CompareTag("Player"))       // 若是Palyer遇到
        {
            panelUI.SetActive(false);     // 关闭panelUI
        }
    }
    
    //【2Box和Trigger相互接触时】
    private void OnTriggerStay2D(Collider2D other)
    {
        isHitDestination = true;        // 接触到目的地
    }
    
    ////--------------------------------------------------------
}
