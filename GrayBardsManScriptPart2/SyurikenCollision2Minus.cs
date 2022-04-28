using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SyurikenCollision2Minus : MonoBehaviour
{
    public float deleteTime = 2;    //消去時間
    [Header("手裏剣のスピード")] public float shootSpeed = 0.05f;

    private void Start()
    {
        Destroy(gameObject, deleteTime);    //一定時間で消す
    }

    public void Update()
    {
        transform.Translate(-shootSpeed , 0, 0);  //X軸に指定した速度でマイナス方向に飛ばす

        //画面外に出た時このオブジェクトを削除する
        if (!GetComponent<Renderer>().isVisible)
        {
            Destroy(this.gameObject);
        }

    }


    private void OnCollisionEnter2D(Collision2D collision)
    {
        //接触したゲームオブジェクトの子にする
        transform.SetParent(collision.transform);
        //当たりを無効化する
        GetComponent<Collider2D>().enabled = false;
        //物理シミュレーションを無効化する
        GetComponent<Rigidbody2D>().simulated = false;
    }

}
