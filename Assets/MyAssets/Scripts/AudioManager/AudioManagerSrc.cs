using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioManagerSrc : MonoBehaviour
{
    public AudioSource myAudioSource;      // AudioSource类
    [SerializeField] private AudioClip jumpAudio, hurtAudio, eatCherryAudio;    // AudioClip类
    
    //【播放跳跃音频】
    public void PlayJumpAudio()
    {
        myAudioSource.clip = jumpAudio;     // 给AudioSource的Clip赋值
        myAudioSource.Play();               // 播放音频
    }
    
    //【播放受伤音频】
    public void PlayHurtAudio()
    {
        myAudioSource.clip = hurtAudio;     // 给AudioSource的Clip赋值
        myAudioSource.Play();               // 播放音频 
    }
    
    //【播放死亡音频（受伤音量增大版）】
    public void PlayDeathAudio()
    {   
        myAudioSource.clip = hurtAudio;     // 给AudioSource的Clip赋值
        myAudioSource.volume = 0.3f;        // 音量增大
        myAudioSource.Play();               // 播放音频 
    }
    
    //【播放吃樱桃音频】
    public void PlayEatCherryAudio()
    {
        myAudioSource.clip = eatCherryAudio;     // 给AudioSource的Clip赋值
        myAudioSource.Play();               // 播放音频 
    }
}
