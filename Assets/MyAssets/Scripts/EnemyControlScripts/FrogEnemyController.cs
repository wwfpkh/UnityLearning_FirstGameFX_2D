using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FrogEnemyController : EnemyController      // 继承父类：EnemyController,继承父类可以
{

    //【组件变量】
    private Rigidbody2D myRigidbody2D;      // 刚体组件变量
    private Collider2D myCollider2D;        // 碰撞体组件 
    private Transform leftEmpty;            // 物体Transform变量：获取子左空物体
    private Transform rightEmpty;           // 物体Transform变量：获取子右空物体
    //【移动速度】
    public float moveSpeed = 3.0f;                 // 移动速度
    //【左右移动边界范围】
    private float leftRange;                // 左范围
    private float rightRange;               // 右范围
    //【角色是否朝左（初始）】
    public bool isOriginal_FaceToLeft;       // 初始角色朝向
    private float original_FaceDirection;       // -1表示朝左，1表示朝右，此参数与左右移动有关
    //【地面碰撞】
    private bool isGround;                  // 判断是否在地面上
    public LayerMask groundLayer;          // 地面图层
    //【当前状态】
    private bool jumpCondition;             // 跳跃状态
    private bool dropCOndition;             // 下落状态
    
    ////--------------------------Start
    void Start()
    {
        base.Start();           // 调用base父级的Start()函数;  
        Initialization();       // 初始化函数
        MoveRangeFun();         // 左右移动范围设置
    }
    
        //【初始化函数】
        void Initialization()
        {
            myRigidbody2D = this.GetComponent<Rigidbody2D>();       // 获得该物体的刚体组件
             myCollider2D = this.GetComponent<Collider2D>();         // 获得该物体的碰撞体组件
            // myAnimator = this.GetComponent<Animator>();             // 获得Animator组件
            leftEmpty = this.transform.GetChild(0);                 // 获得第一个子物体的Tranform值
            rightEmpty = this.transform.GetChild(1);                // 获得第二个子物体的Tranform值
        }
        
        //【设置左右移动范围】
        void MoveRangeFun()
        {
            leftRange = leftEmpty.position.x;       // 左边界：获得左侧空物体的绝对位置x值
            rightRange = rightEmpty.position.x;     // 右边界：获得右侧空物体的绝对位置x值
            Destroy(leftEmpty.gameObject);          // 销毁左子物体
            Destroy(rightEmpty.gameObject);         // 销毁右子物体
        }
        
        
    ////----------------------------------Update
    void Update()
    {
        isGround = myCollider2D.IsTouchingLayers(groundLayer);      // 判断是否在地面上
        FaceDirectionFunction();        // 判断角色移朝向
        MoveMentFunction();             // 角色移动函数
        //【在Animator组件中调用该事件】SkipDropFunction();             // 角色跳跃函数
        conditionFunction();            // 状态判断函数
        AnimationFunction();            // 动画播放函数
    }
    
        //【判断角色朝向】
        void FaceDirectionFunction()
        {
            
            if (isOriginal_FaceToLeft)          // 若初始朝向朝左
            {
                if (this.transform.position.x <= leftRange)      // 若角色的位置到达左侧边界
                {
                    this.transform.localScale = new Vector3(-1, 1, 1);      // 角色朝右
                }
                else if (this.transform.position.x >= rightRange)   // 若角色的位置到达右侧边界
                {
                    this.transform.localScale = new Vector3(1, 1, 1);      // 角色朝左
                }
                original_FaceDirection = -1;            // 朝左
            }
            else                                // 若初始朝向朝右
            {
                if (this.transform.position.x <= leftRange)      // 若角色的位置到达左侧边界
                {
                    this.transform.localScale = new Vector3(1, 1, 1);      // 角色朝右
                }
                else if (this.transform.position.x >= rightRange)   // 若角色的位置到达右侧边界
                {
                    this.transform.localScale = new Vector3(-1, 1, 1);      // 角色朝左
                }
                original_FaceDirection = 1;            // 朝右
            }
        }
        
        //【移动函数】
        void MoveMentFunction()
        {
            myRigidbody2D.velocity = new Vector2(this.transform.localScale.x * moveSpeed * original_FaceDirection, myRigidbody2D.velocity.y);        // 角色往左右朝向移动
        }
        
        //【跳跃、下落函数】
        void SkipDropFunction()
        {
            if (isGround)       // 若落在地面上
            {
                myRigidbody2D.velocity = new Vector2(myRigidbody2D.velocity.x, 30.0f);       // 跳起
            }
        }
        
        //【状态判断】
        void conditionFunction()
        {
            if (myRigidbody2D.velocity.y > 0)           // 若y方向速度 >0
            {
                jumpCondition = true;               // 跳跃状态true
                dropCOndition = false;              // 下落状态false
            }
            else if(myRigidbody2D.velocity.y < 0)       //  若y方向速度 <0
            {
                dropCOndition = true;            // 下落状态true
                jumpCondition = false;           // 跳跃状态false
            }
            else                                // 若 y方向速度为0
            {
                dropCOndition = false;            // 下落状态false
                jumpCondition = false;           // 跳跃状态false
            }
        }
        
        //【AnimFunction】
        void AnimationFunction()
        {
            //【跳跃动画】
            if (jumpCondition)
            {
                myAnimator.SetBool("isJumpingAnimation_B", true);       // 跳跃Bool True
            }
            else
            {
                myAnimator.SetBool("isJumpingAnimation_B", false);      // 跳跃Bool False
            }
            
            //【下落动画】
            if (dropCOndition)
            {
                myAnimator.SetBool("isDroppingAnimation_B", true);      // 下落Bool True
            }
            else
            {
                myAnimator.SetBool("isDroppingAnimation_B", false);     // 下落Bool False
            }
        }
}
