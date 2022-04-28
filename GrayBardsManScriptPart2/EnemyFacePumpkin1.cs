using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyFacePumpkin1 : MonoBehaviour
{

    #region //インスペクターで設定する
    [Header("加算スコア")] public int myScore = 1000;
    [Header("ダメージSE")] public AudioClip damageSE;
    [Header("やられた時に鳴らすSE")] public AudioClip deadSE;
    #endregion

    #region//プライベート変数
    private Rigidbody2D rb = null;
    private SpriteRenderer sr = null;
    private Animator anim = null;
    private ObjectCollision1 oc = null;
    private BoxCollider2D col = null;
    private bool isDeadSE;
    private bool isPlayerAttackBool;
    private int FPhp = 60;
    #endregion

    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        sr = GetComponent<SpriteRenderer>();    //インスタンスを捕まえる  
        anim = GetComponent<Animator>();
        oc = GetComponent<ObjectCollision1>();
        col = GetComponent<BoxCollider2D>();
    }

   

    public void OnTriggerEnter2D(Collider2D collision)
    {
        //攻撃時の当たり判定
        if (collision.gameObject.tag == "Attack" || collision.gameObject.tag == "Syuriken")
        {
            if (!isPlayerAttackBool)
            {
                FPhp -= 20;
                if (GManager1.instance != null)
                {
                    GManager1.instance.PlaySE(damageSE);
                }
                isPlayerAttackBool = true;
               
                Invoke("StopPlayerAttack", 0.2f); //0.2秒後に連続判定防止フラグを戻す
            }

            if (FPhp <= 0)
            {
                if (!isDeadSE)
                {
                    this.tag = "DeadEnemy";
                    isDeadSE = true;
                    if (GManager1.instance != null)
                    {
                        GManager1.instance.score += myScore;
                        GManager1.instance.PlaySE(deadSE);
                    }
                    anim.Play("EnemyFacePumpkinDead");
                    Destroy(gameObject, 0.5f);
                }
            }
        }

    }


    //フラグを指定した秒後に戻す
    private void StopPlayerAttack()
    {
        isPlayerAttackBool = false;
    }


}
