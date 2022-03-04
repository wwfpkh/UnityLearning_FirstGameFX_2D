using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EagleEnemyController : EnemyController         // 继承父类：EnemyController
{
    
    //【组件变量】
    private Rigidbody2D myRigidbody2D;      // 刚体组件变量
    private Transform upEmpty;            // 物体Transform变量：获取子上空物体
    private Transform downEmpty;           // 物体Transform变量：获取子下空物体
    //【上下移动边界范围】
    private float upRange;                // 左范围
    private float downRange;               // 右范围
    //【移动参数】
    private float moveDirction = 1.0f;       // 移动方向,默认向上
    public float moveSpeed = 3.0f;           // 移动速度
    
    ////--------------------------Start
    void Start()
    {
        Initialization();       // 初始化函数
        MoveRangeFun();         // 上下移动范围设置
    }
    
        //【初始化函数】
        void Initialization()
        {
            base.Start();           // 调用base父级的Start()函数; 
            myRigidbody2D = this.GetComponent<Rigidbody2D>();       // 获得该物体的刚体组件
            upEmpty = this.transform.GetChild(0);                 // 获得第一个子物体的Tranform值
            downEmpty = this.transform.GetChild(1);                // 获得第二个子物体的Tranform值
        }
        
        //【设置左右移动范围】
        void MoveRangeFun()
        {
            upRange = upEmpty.position.y;       // 上边界：获得上侧空物体的绝对位置y值
            downRange = downEmpty.position.y;     // 下边界：获得下侧空物体的绝对位置y值
            Destroy(upEmpty.gameObject);          // 销毁上子物体
            Destroy(downEmpty.gameObject);         // 销毁下子物体
        }

     ////----------------------------------Update
    void Update()
    {
        MoveMentFunction();     // 移动函数
    }
        
    
        //【移动函数】
        void MoveMentFunction()
        {
            moveDirectionFunction();            // 角色移动方向设置函数
            myRigidbody2D.velocity = new Vector2(myRigidbody2D.velocity.x, moveDirction * moveSpeed);        // 角色上下移动
        }
        
            //【角色移动方向设置函数】
            void moveDirectionFunction()
            {
                if (this.transform.position.y >= upRange)        // 若物体高度 >= 上限
                {
                    moveDirction = -1.0f;       // 向下移动
                }
                else if (this.transform.position.y <= downRange)     // 若物体高度 <= 下限
                {
                    moveDirction = 1.0f;        // 向上移动
                }
            }
}
