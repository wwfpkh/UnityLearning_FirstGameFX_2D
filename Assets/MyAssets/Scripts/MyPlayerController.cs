using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;                   // 有关UI
using UnityEngine.SceneManagement;     // 有关场景

public class MyPlayerController : MonoBehaviour
{

    //【参数变量】
        private Rigidbody2D myRigidbody2D;           // 获得刚体
        private BoxCollider2D myCollider2D;             // 获得方形碰撞体
        // 音频变量
        // private AudioSource myJumpAudioSource;      // 1跳跃音频
        // private AudioSource myHurtAudioSource;      // 2受伤音频
        // private AudioSource myEatingAudioSource;    // 3吃东西音频
        // private AudioSource myDeathAudioSource;     // 4死亡音频
        
    //【接触判断变量】
    [Header(" 接触判断变量")]
    [SerializeField] private bool isGound;      // 判断是否在地面上
    [SerializeField] private bool isHitCeiling; // 判断是否接触到天花板
    
    //【环境检测】
    [Header(" 环境检测")]
    public LayerMask ground;                    // 获得地面的LayerMask
    public float leftFootOffset = 0.39f;         // 左脚到中心点的偏移值(正)
    public float rightFootOffset = 0.39f;        // 右脚到中心点的偏移值(正)
    public float upDistance = 1.18f;             // 头顶到中心点的距离(正)
    // Animator
    private Animator myAnimator;                // 获得Animator
    
    //【移动变量】
    public float moveSpeed;                       // 移动速度
    //【跳跃、下落变量】
    public float jumpForce;                       // 跳跃力
    private bool isJumpPressed;                   // 判断是否按下跳跃按键
    public  int jumpCount;                        // 多段跳次数
    [SerializeField]private int currentJumpCount; // 当前可跳跃次数
    [SerializeField]private bool isJump;                          // 判断是否在跳跃
    private bool isDrop;                          // 判断是否在下落
    private bool isAlreadyJump = false;           // 判断是否跳过
    //【下蹲变量】
    private bool isCrouch;                        // 判断是否下蹲
    
    //【受伤变量】
    [SerializeField]private bool isHurt;                          // 判断是否受伤
    
    
    //【物品收集】
    [SerializeField]private int cherryNum;       // 樱桃收集
    
    //【UI变量】
    public Text cherryNumUI;                     // UI变量：在收集樱桃时改变
    
    //【时间变量】
    private float myDelayTime = 0;               // 延时时间变量
    private bool isdelayTimeOn;                 // 是否开启延时
    
    //【BoxCollider参数变量】
    private Vector2 idleBoxOffset;          // 站立时的Box的Offset
    private Vector2 idleBoxSize;            // 站立时Box的Size
    private Vector2 crouchBoxOffset;        // 站立时的Box的Offset
    private Vector2 crouchBoxSize;          // 站立时的Box的Size
    
    //【AudioManager】
    public GameObject myAudioManager;         // 音频控制物体
    
    
    ////-------------------------------------Start
    // Start函数
    void Start()
    {
        Initialization();       // 初始化
    }
    
        //【初始化函数】
        void Initialization()
        {
            // 组件赋值
                myRigidbody2D = this.GetComponent<Rigidbody2D>();        // 刚体赋值:该物体的刚体
                myCollider2D = this.GetComponent<BoxCollider2D>();       // 碰撞体赋值：获得BoxCollider2D组件
                // 音频组件赋值
                    // myJumpAudioSource = this.GetComponents<AudioSource>()[0];   // 获得第一个音频组件
                    // myHurtAudioSource = this.GetComponents<AudioSource>()[1];   // 获得第二个音频组件
                    // myEatingAudioSource = this.GetComponents<AudioSource>()[2];   // 获得第三个音频组件
                    // myDeathAudioSource = this.GetComponents<AudioSource>()[3];      // 获得第四个音频组件
                myAnimator = this.GetComponent<Animator>();              // Animator赋值
            BoiColliderSizeOffsetFun();     // 记录Box的尺寸
        }
        
            //【记录初始的BoxCollider的尺寸位置，以及下蹲时的BoxCollider的尺寸位置】
            void BoiColliderSizeOffsetFun()
            {
                idleBoxOffset = myCollider2D.offset;        // 记录初始Box的Offset
                idleBoxSize = myCollider2D.size;            // 记录初始Box的Size
                
                crouchBoxOffset = new Vector2(idleBoxOffset.x, idleBoxOffset.y * 0.5f);      // 设置下蹲Box的Offset
                crouchBoxSize = new Vector2(idleBoxSize.x, idleBoxSize.y * 0.5f);           // 设置下蹲Box的Size
            }

    ////--------------------------------------------------------Update
    // Update函数
    void Update()
    {
        HitJudgeFunction();     // 判断接触函数
        DelayTimeFun();         // 延时函数
        MoveAllFunction();      // 总移动函数
        AnimatorAllFunction();  // 总动画函数
    }
        //【延时函数】
        void DelayTimeFun()
        {
            if (isdelayTimeOn)      // 若开启了延时
            {
                myDelayTime += Time.deltaTime;       // 时间增加
                if (isHurt)     // 若是因为受伤产生的延时
                {
                    if (myDelayTime >= 0.3)         // 若延时超过0.3s
                    {
                        isHurt = false;             // 不再受伤
                        isdelayTimeOn = false;      // 不再延时
                        myDelayTime = 0;            // 延时时间归0
                    }
                }
            }
        }
        
        //【接触判断函数：射线判断法】
        void HitJudgeFunction()
        {
            //【物体4个角判断是否与上下地面接触:射线判断法】
            RaycastHit2D downLeftnHit = myRayCastFun(new Vector2(-leftFootOffset, 0.0f), Vector2.down, 0.15f, ground);    // 左下接触地面判断
            RaycastHit2D downRightHit = myRayCastFun(new Vector2(rightFootOffset, 0.0f), Vector2.down, 0.15f, ground);    // 右下接触地面判断
            RaycastHit2D upLeftHit = myRayCastFun(new Vector2(-leftFootOffset, upDistance), Vector2.up, 0.15f, ground);       // 左上接触地面判断
            RaycastHit2D upLRightHit = myRayCastFun(new Vector2(rightFootOffset, upDistance), Vector2.up, 0.15f, ground);       // 左上接触地面判断
            //【判断是否与地面接触】
            if (downLeftnHit || downRightHit)
            {
                isGound = true;
            }
            else
            {
                isGound = false;
            }
            //【判断是否与天花板接触】
            if (upLeftHit || upLRightHit)
            {
                isHitCeiling = true;
            }
            else
            {
                isHitCeiling = false;
            }
        }
    
                ////myRayCastFun【射线判断函数(自己写的)，在Unity自带的方法基础上增加了功能】 
                //范围类型：射线类 （本质是Bool）； 传入的参量： (射线起点关于物体中心点的相对位置， 射线的方向，射线的长度， 检测的Layer)
                RaycastHit2D myRayCastFun(Vector2 offset, Vector2 rayDirection, float rayLength, LayerMask layer)       //————判断产生的射线是否与Layer重合
                {
                    Vector2 posCenter = this.transform.position;        // 获取物体中心点的坐标
                    Vector2 rayStartOrigin = posCenter + offset;                 // 射线起点的坐标
                    RaycastHit2D hit = Physics2D.Raycast(rayStartOrigin, rayDirection, rayLength, layer);   // Unity自带的方法，判断产生的射线是否与layer接触
                    //【射线显示】
                    Color rayColor = hit ? Color.red : Color.blue;  // 若接触，则射线颜色红，若未接触，则射线颜色蓝
                    Debug.DrawRay(rayStartOrigin, rayDirection * rayLength, rayColor);      //  Debug画线

                    return hit;
                }
        
    
        //【移动总函数】
        void MoveAllFunction()
        {
            //【移动执行】
            if (!isHurt)        // 若不处于受伤状态
            {
                GoundMoveFun();        // 地面移动
                SkipDropFun();         // 跳跃、下落
                CourchFun();           // 下蹲
            }
        }
    
            //【地面移动】
            void GoundMoveFun()
            {   
                // 左右移动
                float horizontalMove = Input.GetAxisRaw("Horizontal");      // 返回-1，0，1
                myRigidbody2D.velocity = new Vector2(horizontalMove * moveSpeed, myRigidbody2D.velocity.y);     // x方向移动
                // 左右翻转
                if (horizontalMove != 0)    // 若不是静止
                {
                    this.transform.localScale = new Vector3(horizontalMove, 1, 1);      // x方向翻转
                }
            }
            
            //【跳跃、下落】
            void SkipDropFun()
            {
                
                //【判断是否按下Jump按键】
                if (Input.GetButtonDown("Jump") && currentJumpCount > 0)    // 只有在可跳跃时，才算按下，否则此次按下无效
                {
                    isJumpPressed = true;
                    currentJumpCount--;        // 跳跃次数减1
                    JumpFun();                 // 执行跳跃
                    
                }

                //【跳跃过程判断】
                if (myRigidbody2D.velocity.y > 0  && !isHurt && !isGound)   // !isGound是防止在平地上因误差而产生的Bug
                {
                    isDrop = false;
                    isJump = true;
                }
                    
                //【下落过程判断】
                if (myRigidbody2D.velocity.y < 0 && !isGound)   // !isGound是防止在平地上因误差而产生的Bug
                {
                    isDrop = true;
                    isJump = false;
                    //【因走路掉落使跳跃次数-1】
                    if (!isAlreadyJump)         // 若之前没跳过，即是走路而引起的掉落
                    {
                        currentJumpCount--;     // 当前可跳跃次数-1
                        isAlreadyJump = true;   // 算进行过跳跃
                    }
                }
                
                
                //【到达地面，跳跃次数恢复】
                if (isGound && !isJumpPressed)     // !isJump一定要判断，因为否则起跳时，会与Ground接触导致触发跳跃次数恢复，会多跳一次
                {
                    if (!isJump)
                    {
                        currentJumpCount = jumpCount;   // 可跳跃次数更新
                        isAlreadyJump = false; // 刷新：未进行跳跃
                    }
                    isJump = false;     // 到地面时不在跳跃
                    isDrop = false;     // 到地面时不在下落
                }
            }
            
            //【触发跳跃函数】
            void JumpFun()
            {
                isJump = true;      // 在跳跃
                isAlreadyJump = true;   // 进行过跳跃
                myRigidbody2D.velocity = new Vector2(myRigidbody2D.velocity.x, jumpForce);      // y方向获得瞬时速度
                isJumpPressed = false;  // 使其停止，防止一直按着jump键，导致一直进入该if
                
                // 音频播放
                AudioManagerSrc jumpAudio = myAudioManager.GetComponent<AudioManagerSrc>();      // 获取myAudioManager中的脚本
                jumpAudio.PlayJumpAudio();       // 播放跳跃音频
            }
            
            //【下蹲函数】
            void CourchFun()
            {
                bool crouchGetButton = Input.GetButton("Crouch");       // 当按键被按下的时候
                if (crouchGetButton && isGound)      // 当按下”下蹲“键时
                {
                    isCrouch = true;        // 下蹲
                    DecreaseBoxY();         // ColliderBox的变为下蹲时的尺寸
                }
                if(!crouchGetButton && !isHitCeiling)    // 当松开”下蹲“键时, 并且天花板上没有东西时
                {
                    isCrouch = false;       // 不再下蹲
                    RelaseBoxY();           // ColliderBox的尺寸还原
                }
            }
            
                //【下蹲时Box的压扁】
                void DecreaseBoxY()
                {
                    myCollider2D.offset = crouchBoxOffset;      // box的Offset更新
                    myCollider2D.size = crouchBoxSize;        // box的Size更新
                }
                //【下蹲结束时Box的返回原初大小】
                void RelaseBoxY()
                {
                    myCollider2D.offset = idleBoxOffset;    // box的Offset更新
                    myCollider2D.size = idleBoxSize;        // box的Size更新
                }
        
                
            
         
        //【Animator总函数】
        void AnimatorAllFunction()
        {
            //【左右移动Anim】
            myAnimator.SetFloat("runningAnimPara_F", Mathf.Abs(myRigidbody2D.velocity.x));           // 设置AnimatorController的Running变量参数，根据左右速度设置
            
            //【跳跃、下落Anim】
                //【跳跃Anim】
                if (isJump && !isDrop)
                {
                    myAnimator.SetBool("jumpingAnimPara_B", true);          // 跳跃true
                    myAnimator.SetBool("droppingAnimPara_B", false);        // 下落false
                }
                //【下落Anim】
                else if (isDrop && !isJump)
                {
                    myAnimator.SetBool("droppingAnimPara_B", true);         // 下落true
                    myAnimator.SetBool("jumpingAnimPara_B", false);         // 跳跃false
                }
                //【下落 回归站立Anim】
                else if (!isDrop && !isJump)
                {
                    myAnimator.SetBool("droppingAnimPara_B", false);         // 下落false
                    myAnimator.SetBool("jumpingAnimPara_B", false);         // 跳跃false
                }
             
            //【下蹲Anim】
            if (isCrouch)
            {
                myAnimator.SetBool("crouchAnimaPara_B", true);      // 下蹲动画true
            }
            else
            {
                myAnimator.SetBool("crouchAnimaPara_B", false);      // 下蹲动画false
            }
                
                
            //【受伤Anim】
            if (isHurt)     // 若受伤了，播放受伤动画
            {
                myAnimator.SetBool("hurttingAnimaPara_B", true);            // 受伤true
            }
            else           // 若没受伤，返回正常站立
            {
                myAnimator.SetBool("hurttingAnimaPara_B", false);            // 受伤false
            }
        }

    ////--------------------------------------------------------Event Function 事件触发函数
    //【事件函数】当其他碰撞体碰到其他触发器的时候(Unity自带模板)
    private void OnTriggerEnter2D(Collider2D collision)
    {
        //【收集物品】
        if (collision.CompareTag("CollectionCherryTag"))      // 若碰到的Box的Tag为"Collection_Cherry_Tag"；collider.CompareTag比collider.tag性能好
        {
            // 音频播放
            AudioManagerSrc eatCherryAudio = myAudioManager.GetComponent<AudioManagerSrc>();      // 新建一个 AudioManagerSrc类的对象
            eatCherryAudio.PlayEatCherryAudio();       // 播放吃樱桃音频
            
            cherryNum++;                                // 收集樱桃+1
            collision.enabled = false;
            Destroy(collision.gameObject);              // 销毁碰撞体所在的Object
            
            //【UI改变】
            EatingCherry_UIFun();       // 调用UI改变函数                       
        }

        //【触发死亡】
        if (collision.CompareTag("DeathLineTag"))
        {
            // 音频播放
            AudioManagerSrc deathAudio = myAudioManager.GetComponent<AudioManagerSrc>();      // 新建一个 AudioManagerSrc类的对象
            deathAudio.PlayDeathAudio();       // 播放死亡音频
            
            Invoke("ReStartThisScene", 2.0f);       // Invoke延迟委托——延迟2s后 运行ReStartThisScene函数
        }
    }
        //【重启当前关卡函数】
        void ReStartThisScene()
        {
            SceneManager.LoadScene(SceneManager.GetActiveScene().name);     // 重新加载当前关卡。这里使用了SceneManager类的2个方法
        }
    
    //【碰撞敌人:事件函数】
    private void OnCollisionEnter2D(Collision2D col)        // 当2个碰撞体相互接触时
    {
        // 建立一个EnemyController类，执行里面的函数
        EnemyController ThisEnemy = col.gameObject.GetComponent<EnemyController>();     // 获得这个碰撞体所在物体的脚本： EnemyController类，赋值给ThisEnemy，之后可以调用该类里面的函数
        
        if (col.gameObject.CompareTag("EnemyTag"))     // 若碰撞体所在物体的Tag为"EnemyTag"
        {
            if (isDrop && this.transform.position.y > col.gameObject.transform.position.y)      // 若正在角色正在下落,且角色的y值高度在敌人之上
            {
                // Enemy死亡
                ThisEnemy.isDeathFun();         // 调用EnemyController类的函数，执行Enemy的死亡
                
                // 自动跳跃+跳跃次数刷新
                JumpFun();      // 执行跳跃
                currentJumpCount = jumpCount - 1;           // 刷新跳跃次数：可初跳跃次数-1
            }
            else            // 角色受伤
            {
                if (this.transform.position.x < col.gameObject.transform.position.x)   // 若角色在敌人左边撞到
                {
                    myRigidbody2D.velocity = new Vector2(-5.0f, 1.0f);                  // 给一个向左上的瞬时速度
                    isHurt = true;      // 受伤了
                }
                else             // 若角色在敌人右边撞到
                {
                    myRigidbody2D.velocity = new Vector2(5.0f, 1.0f);                  // 给一个向右上的瞬时速度
                    isHurt = true;      // 受伤了
                }
                
                // 音频播放
                AudioManagerSrc hurtAudio = myAudioManager.GetComponent<AudioManagerSrc>();      // 新建一个 AudioManagerSrc类的对象
                hurtAudio.PlayHurtAudio();       // 播放受伤音频
                
                isdelayTimeOn = true;       // 开启延时，需要一段时间不能控制角色
            }

        }
    }

    ////------------------------------------------------------------UI Function
    //【收集樱桃UI】
    void EatingCherry_UIFun()
    {
        cherryNumUI.text = "x " + cherryNum.ToString();          // UI的Text显示改变
    }
}