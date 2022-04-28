using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//BossEffectAtkオブジェクトはオフ(チェックを外す)にしておく。CucumberAttackオブジェクトはオンにしておく。

public class CucumberAttack1 : MonoBehaviour
{

    private Animator anim;


    private void Start()
    {
        anim = GetComponent<Animator>();
    }

    public void Update()
    {
        transform.Translate(-0.05f, 0, 0);  //X軸に指定した速度で飛ばす。

        //画面外に出た時このオブジェクトを削除する
        if (!GetComponent<Renderer>().isVisible)
        {
            Destroy(this.gameObject);
        }

    }


    public void OnTriggerEnter2D(Collider2D collision)
    {
        //攻撃時の当たり判定
        if (collision.gameObject.tag == "Attack")
        {
            this.tag = "DamageEnemy";
            anim.Play("BossCucumberAttackDead");
            Destroy(this.gameObject,0.1f);
        }

        if(collision.gameObject.tag == "Player")
        {
            Destroy(this.gameObject, 0.1f);
        }

    }

   

}
