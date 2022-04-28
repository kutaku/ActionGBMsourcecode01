using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyPumpkin1 : MonoBehaviour
{

    #region //インスペクターで設定する
    [Header("加算スコア")] public int myScore = 300;
    [Header("移動速度")] public float speed = 3;
    [Header("重力")] public float gravity = 0;
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
    private bool rightTleftF = false;
    private bool isDead = false;
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

    // Update is called once per frame
    void FixedUpdate()
    {
        if (!oc.playerStepOn)   // プレイヤーに踏まれたかどうか
        {
            if (sr.isVisible || nonVisibleAct)
            {
                if (checkCollision.isOn)
                {
                    rightTleftF = !rightTleftF;
                }
                int xVector = -1;
                if (rightTleftF)
                {
                    xVector = 1;
                    transform.localScale = new Vector3(-1, 1, 1);
                }
                else
                {
                    transform.localScale = new Vector3(1, 1, 1);
                }
                rb.velocity = new Vector2(xVector * speed, -gravity);

            }
            else
            {
                rb.Sleep();     //画面に映っていない時物理演算を切る
            }
        }
        else
        {
            if (!isDead)
            {
                anim.Play("EnemyPumpkinWingDead");
                rb.velocity = new Vector2(0, -gravity);
                isDead = true;
                col.enabled = false;
                if (GManager1.instance != null)
                {
                    GManager1.instance.PlaySE(deadSE);
                    GManager1.instance.score += myScore;
                }
                Destroy(gameObject, 3f);
            }
            else
            {
                transform.Rotate(new Vector3(0, 0, 5));
            }
        }
    }


    public void OnTriggerEnter2D(Collider2D collision)
    {
        //攻撃時の当たり判定
        if (collision.gameObject.tag == "Attack" || collision.gameObject.tag == "Syuriken")
        {
            if (!isDeadSE)
            {
                this.tag = "DeadEnemy";
                anim.Play("EnemyPumpkinWingDead");
                isDead = true;
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
