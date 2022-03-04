using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;      // 场景方法

public class TotalMenuScript : MonoBehaviour
{
    //【公共函数,可以在按钮按下时调用】
    public void PlayGameFun()
    {
        SceneManager.LoadScene("Level01");      // 加载Scene01
    }
    
    //【退出游戏】
    public void QuitGameFun()
    {
        Application.Quit();     // 退出游戏
    }
}