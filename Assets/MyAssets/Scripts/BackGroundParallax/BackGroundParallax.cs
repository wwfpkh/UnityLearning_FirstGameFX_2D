using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BackGroundParallax : MonoBehaviour
{
    public Transform cameraPos;             // 相机的位置
    public float moveRatio = 1.1f;          // 背景随相机运动比例 = 1时完全跟随相机移动
    private Vector3 cameraStartPos;         // 相机的初始位置
    [SerializeField]private Vector3 backStartPos;           // 背景的初始位置
    
    ////-----------------------------------Start
    void Start()
    {
        cameraStartPos = cameraPos.position;      // 传入初始相机的位置
        backStartPos = this.GetComponent<Transform>().position;     // 传入初始背景的位置 
    }

    ////---------------------------------Update
    void Update()
    {
        float camBiasX = cameraPos.position.x - cameraStartPos.x;       // 相机的偏移量
        this.transform.position = new Vector3(backStartPos.x + camBiasX * moveRatio, this.transform.position.y, this.transform.position.z);     // 背景的x值变动
    }
}
