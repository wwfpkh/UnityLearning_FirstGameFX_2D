using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyController : MonoBehaviour
{
    //【保护变量】
    protected Animator myAnimator;          // 保护类型，子类可访问，Animator变量
    protected AudioSource myDeathAudioSource;        // Audio Source组件
        
    
    ////-------------------------------Start
    protected void Start()                     // 保护类型，子类可访问，Animator变量
    {
        myAnimator = this.GetComponent<Animator>();      // Animator组件赋值
        myDeathAudioSource = this.GetComponent<AudioSource>();      // Audio Source组件赋值
    }
    
    ////【公共函数】---------------------------------------------------------------
    //【死亡函数】
    public void isDeathFun()     // 死亡，则执行该函数————因为要在其他类种使用，所以定义为public
    {
            
        myDeathAudioSource.Play(0);         // 延时0s播放音频
        myAnimator.SetTrigger("isDeathAnimation_T");        // 设置Death动画触发变量，触发death动画，在death动画结束的时候会触发event
                
    }
    //--------------------------------------------------------------------------------
    //【销毁角色】
    void DestroyFun()
    {
        Destroy(this.gameObject);       // 销毁自身
    }
        
        
}