using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SyurikenOfPlayer1 : MonoBehaviour
{
    public GameObject syurikenObj;
    public GameObject kunaiMinusObj;
    [Header("手裏剣を投げるSE")] public AudioClip syurikenSE;
    [Header("発射感覚")]public float shootDelay = 0.5f;


    private bool isRight;   //右フラグ
    private bool isLeft;    //左フラグ
    private bool inAttack = false;  //攻撃中フラグ

    private void Start()
    {
        //"StopAttack"を1秒後に呼びだされ、1秒ごとに呼びだす。
        InvokeRepeating("StopAttack",1,1);
    }


    void Update()
    {
        //攻撃中ではない時
        if (inAttack == false)
        {
            //Bキーが押された時、手裏剣オブジェクトのインスタンスを作る
            if (Input.GetKeyDown(KeyCode.B))
            {
                inAttack = true;

                if (isLeft)
                {
                    //左が押されている場合マイナス方向に飛ぶ手裏剣を作る
                    Instantiate(kunaiMinusObj, transform.position, Quaternion.identity);
                    GManager1.instance.PlaySE(syurikenSE);
                }
                else if (isRight)
                {
                    //右が押されている場合プラス方向に飛ばす
                    Instantiate(syurikenObj, transform.position, Quaternion.identity);
                    GManager1.instance.PlaySE(syurikenSE);
                }
                else
                {
                    //それ以外はプラスの方向に飛ぶ手裏剣を作る
                    Instantiate(syurikenObj, transform.position, Quaternion.identity);
                    GManager1.instance.PlaySE(syurikenSE);
                }

            }
        }

        if (Input.GetKeyDown(KeyCode.RightArrow))
        {
            isLeft = false;
            isRight = true;
        }else if (Input.GetKeyDown(KeyCode.LeftArrow))
        {
            isRight = false;
            isLeft = true;
        }

        //攻撃フラグを下す遅延行為
        Invoke("StopAttack", shootDelay);
    }


    public void StopAttack()
    {
         //攻撃フラグを下ろす
        inAttack = false;
    }

}

