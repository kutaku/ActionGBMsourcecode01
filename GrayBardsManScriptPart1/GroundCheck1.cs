using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GroundCheck1 : MonoBehaviour
{
    [Header("エフェクトがついた床を判定するか")] public bool checkPlatformGround = true;

    private string groundTag = "Ground";
    private string platformTag = "GroundPlatform";
    private string moveFloorTag = "MoveFloor";
    private string fallFloorTag = "FallFloor";
    private bool isGround = false;      //接地判定用
    private bool isGroundEnter, isGroundStay, isGroundExit;

    //物理判定の更新毎に呼ぶ必要がある。接地判定を返すメソッド
    public bool IsGround()
    {
        if(isGroundEnter || isGroundStay)
        {
            isGround = true;
        }   
        else if (isGroundExit)
        {
            isGround = false;
        }
        isGroundEnter = false;
        isGroundStay = false;
        isGroundExit = false;
        return isGround;
    }


    private void OnTriggerEnter2D(Collider2D collision)     //判定内に侵入したら
    {
        if (collision.tag == groundTag)         //タグがGroundなら通る
        {
            isGroundEnter = true;
        }
        else if(checkPlatformGround && (collision.tag == platformTag || collision.tag == moveFloorTag || collision.tag == fallFloorTag))
        {
            isGroundEnter = true;
        }
    }

    private void OnTriggerStay2D(Collider2D collision)      //判定内に入り続けていたら
    {
        if (collision.tag == groundTag)         //タグがGroundなら通る
        {
            isGroundStay = true;
        }
        else if (checkPlatformGround && (collision.tag == platformTag || collision.tag == moveFloorTag || collision.tag == fallFloorTag))
        {
            isGroundStay = true;
        }
    }

    private void OnTriggerExit2D(Collider2D collision)      //判定内から出たら
    {
        if (collision.tag == groundTag)         //タグがGroundなら通る
        {
            isGroundExit = true;
        }
        else if (checkPlatformGround && (collision.tag == platformTag || collision.tag == moveFloorTag || collision.tag == fallFloorTag))
        {
            isGroundExit = true;
        }
    }

}
