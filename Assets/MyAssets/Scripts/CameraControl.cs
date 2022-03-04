using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraControl : MonoBehaviour
{
    public Transform playerTransform;   // 传入角色的坐标，Camera跟随之
    public Vector3 cameraBias = Vector3.zero;          // 相机的相对偏移      
        
    //【相机位置函数】
    void CameraPosFun()
    {
        this.transform.position = new Vector3(playerTransform.position.x, playerTransform.position.y, -10.0f);      // 相机绝对位置跟随角色
        this.transform.position += cameraBias;      // 相机绝对位置偏移
    }
    
    // Update is called once per frame
    void Update()
    {
        CameraPosFun();     // 相机位置函数
    }
}
