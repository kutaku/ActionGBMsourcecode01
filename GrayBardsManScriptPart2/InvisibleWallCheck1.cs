using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InvisibleWallCheck1 : MonoBehaviour
{

    /// <summary>
    /// 判定内にプレイヤーがいる
    /// </summary>
    private bool isTouch = false;
    private string playerTag = "Player";

    //ここに処理を書く

    private void Update()
    {
        if (!isTouch)
        {
            float dx = Input.GetAxis("Horizontal") * Time.deltaTime * 3;
            float dz = Input.GetAxis("Vertical") * Time.deltaTime * 3;
            transform.position = new Vector3(transform.position.x + dx, 0, transform.position.z + dz);
        }

    }

    #region//接触判定
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.tag == playerTag)
        {
                isTouch = true;
        }
    }

    #endregion



}
