using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyKuri1 : MonoBehaviour
{

    #region //インスペクターで設定する
    [Header("加算スコア")] public int myScore = 300;
    [Header("移動速度")] public float speed = 3;
    [Header("重力")] public float gravity = 3;
    [Header("画面外でも行動するか")] public bool nonVisibleAct;
    [Header("接触判定")] public EnemyCollisionCheck1 checkCollision;
    [Header("やられた時に鳴らすSE")] public AudioClip deadSE;

    #endregion

    #region//プライベート変数
    private Rigidbody2D rb = null;
    private SpriteRenderer sr = null;
    private Animator anim = null;
    private ObjectCollision1 oc = null;
    private BoxCollider2D col = null;
    private bool isDeadSE;
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
            if (!isDeadSE)
            {
                this.tag = "DeadEnemy";
                anim.Play("EnemyKuriDead");
                if (GManager1.instance != null)
                {
                    GManager1.instance.PlaySE(deadSE);
                    GManager1.instance.score += myScore;
                }
                isDeadSE = true;
                Destroy(gameObject, 0.1f);
            }
        }
    }

}
