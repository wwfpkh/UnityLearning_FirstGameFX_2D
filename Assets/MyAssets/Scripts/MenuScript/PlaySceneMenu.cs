using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;        // Audio头文件
using UnityEngine.UI;

public class PlaySceneMenu : MonoBehaviour
{
    public GameObject pauseMenu;        // 获取暂停菜单物体
    public Slider audioControlSlider;   // Slider组件（需UI头文件）
    public AudioMixer myAudiomixer;     // 音频控制组件
    
    //【公共函数】
    public void PauseMenuOn()       // 暂停菜单开启，暂停按钮调用
    {
        pauseMenu.SetActive(true);  // 开启暂停菜单
        Time.timeScale = 0f;        // 时间不运行
    }

    public void PauseMenuOff()      // 暂停菜单禁用，返回按钮调用
    {
        pauseMenu.SetActive(false);  // 禁用暂停菜单
        Time.timeScale = 1f;        // 时间恢复正常
    }
    
    //【音量控制函数】
    public void SetVolumeFun()
    {
        myAudiomixer.SetFloat("MainVolume", audioControlSlider.value);        // 通过滑块设置 AudioMixer中的“MainVolume”值
    }
    
}
