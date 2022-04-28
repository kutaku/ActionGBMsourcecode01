using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;       //スライダーUIを使うときは忘れずに書く

public class BossTurtle1 : MonoBehaviour
{
    #region //インスペクターで設定する
    [Header("加算スコア")] public int myScore = 30000;
    [Header("移動速度")] public float speed;
    [Header("重力")] public float gravity =0; //スクリプト側で移動させるので0
    [Header("画面外でも行動するか")] public bool nonVisibleAct;
    [Header("接触判定")] public EnemyCollisionCheck1 checkCollision;
    [Header("ダメージSE")] public AudioClip damageSE;
    [Header("攻撃時スピン音SE")] public AudioClip attackSE;
    [Header("ボスが倒された時の爆発音")] public AudioClip deadSE; 
    [Header("HPゲージsliderObj")] public GameObject bossHPObj;
    [Header("ボスBGMObj")] public GameObject bossBGMObj;
    [Header("タイムカウンターObj")] public GameObject timeObj;
    #endregion

    #region//プライベート変数
    //ステコンに知らせるフラグ
    [HideInInspector] public bool isBossClear;
    private Rigidbody2D rb = null;
    private SpriteRenderer sr = null;
    private Animator anim = null;
    private ObjectCollision1 oc = null;
    private BoxCollider2D col = null;
    private Slider slid;
    private float continueTime = 0.0f;
    private float blinkTime = 0.0f;
    private int bossHP = 200;
    private bool isBossAttack;
    //private bool isBossWalk;
    private bool rightTleftF = false;
    private bool isDead = false;
    private bool isDamage = false;
    private bool isPlayerAttackBool;  //攻撃連続判定防止フラグ
    private bool isDeadSE;  //SEを1回だけ呼び出す
    private bool isBossEffectBool;  //ボスの攻撃を1回だけ呼び出す
    private bool isStartAttack;  //update内で明滅が終わったらフラグをオンにする
    #endregion


    private void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        sr = GetComponent<SpriteRenderer>();    //インスタンスを捕まえる  
        anim = GetComponent<Animator>();
        oc = GetComponent<ObjectCollision1>();
        col = GetComponent<BoxCollider2D>();
        slid = bossHPObj.GetComponent<Slider>();
    }

    private void Update()
    {
        slid.value = bossHP;

        if (isDamage)
        {
            //明滅　ついている時の戻る
            if (blinkTime > 0.2f)
            {
                sr.enabled = true;
                blinkTime = 0.0f;
            }
            //明滅　消えている時
            else if (blinkTime > 0.1f)
            {
                sr.enabled = false;
            }
            //明滅　ついている時
            else
            {
                sr.enabled = true;
            }

            //1秒経ったら明滅終わり
            if (continueTime > 1f)
            {
                isDamage = false;
                blinkTime = 0.0f;
                continueTime = 0.0f;
                isDamage = false;
                sr.enabled = true;

                //明滅が終わったら反撃開始
                isStartAttack = true;
            }
            else
            {
                blinkTime += Time.deltaTime;                //演出中は演出用の時間を進める
                continueTime += Time.deltaTime;
            }
        }

        if (isStartAttack && bossHP > 0)
        {
            //明滅が終わったら攻撃を始める
            isBossAttack = true;
            if (continueTime > 3f)
            {
                //3f経ったら攻撃を止める
                isBossAttack = false;
                isStartAttack = false;
                isBossEffectBool = false;
            }
            else
            {
                continueTime += Time.deltaTime;
            }
        }
    }


  
    // Update is called once per frame
    void FixedUpdate()
    {
        //アニメーションを適用
        SetAnimation();

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
                    if (isBossAttack)
                    {
                        //攻撃を受けたらオニオン甲羅に籠って素早く移動する
                        xVector = 4;
                        transform.localScale = new Vector3(-1, 1, 1);
                    }
                }
                else
                {
                    transform.localScale = new Vector3(1, 1, 1);
                    if (isBossAttack)
                    {
                        xVector = -4;
                        transform.localScale = new Vector3(1, 1, 1);
                    }
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
            if (!isDead && isDeadSE)
            {
                isDead = true;
                col.enabled = false;
            }
        }
    }

    public void OnTriggerEnter2D(Collider2D collision)
    {
        //攻撃時の当たり判定
        if (collision.gameObject.tag == "Attack")
        {
            if (!isPlayerAttackBool)
            {
                bossHP -= 20;
                if (GManager1.instance != null)
                {
                    GManager1.instance.PlaySE(damageSE);
                }
                isDamage = true;
                isPlayerAttackBool = true;
                if(bossHP > 0) 
                {
                    Invoke("IsAttackCheck", 0.5f);  //0.5秒後にスピン音
                }
                Invoke("StopPlayerAttack", 3f); //3秒後に連続判定防止フラグを戻す
            }
        }
        if (collision.gameObject.tag == "Syuriken")
        {
            Destroy(collision.gameObject);
        }

        if (bossHP <= 0)
        {
            if (!isDeadSE)
            {
                this.tag = "DeadEnemy";
                isDeadSE = true;
                isDamage = true;
                if (GManager1.instance != null)
                {
                    GManager1.instance.score += myScore;
                    GManager1.instance.PlaySE(deadSE);
                }
                bossHPObj.SetActive(false);
                bossBGMObj.SetActive(false);
                anim.Play("BossTurtleDead");
                Invoke("BossStageClear", 1f);
                Destroy(timeObj);
                Destroy(gameObject, 1f);
            }
        }
    }

    //アニメーションを設定する
    private void SetAnimation()
    {
        anim.SetBool("BossAttackBool", isBossAttack);
    }

    //ボスの攻撃モーション時にエフェクトを呼び出す
    private void IsAttackCheck()
    {
        if (!isBossEffectBool)
        {
            if (GManager1.instance != null)
            {
                GManager1.instance.PlaySE(attackSE);
            }
            isBossEffectBool = true;
        }
    }

    //ボスを倒したことをステージコントロールに伝える
    public void BossStageClear()
    {
        isBossClear = true;
    }

    //フラグを指定した秒後に戻す
    private void StopPlayerAttack()
    {
        isPlayerAttackBool = false;
        isBossAttack = false;
        isBossEffectBool = false;
    }

}
