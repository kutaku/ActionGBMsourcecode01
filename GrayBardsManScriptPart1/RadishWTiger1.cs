using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;       //スライダーUIを使うときは忘れずに書く

public class RadishWTiger1 : MonoBehaviour
{

    #region //インスペクターで設定する
    [Header("加算スコア")] public int myScore = 50000;
    [Header("移動速度")] public float speed;
    [Header("重力")] public float gravity =0; //スクリプトで制御するので０
    [Header("画面外でも行動するか")] public bool nonVisibleAct;
    [Header("接触判定")] public EnemyCollisionCheck1 checkCollision;
    [Header("ダメージSE")] public AudioClip damageSE;
    [Header("攻撃時SE")] public AudioClip attackSE;
    [Header("倒された時の咆哮SE")] public AudioClip deadSE;
    [Header("HPゲージsliderObj")] public GameObject bossHPObj;
    [Header("ボスBGMObj")] public GameObject bossBGMObj;
    [Header("タイムカウンターObj")] public GameObject timeObj;
    #endregion

    //ステコンに知らせるフラグ
    [HideInInspector] public bool isBossClear;

    #region//プライベート変数
    private Rigidbody2D rb = null;
    private SpriteRenderer sr = null;
    private Animator anim = null;
    private ObjectCollision1 oc = null;
    private BoxCollider2D col = null;
    private Slider slid;
    private int bossHP = 300;   //ボスHPスライダーUIのMAXの値も同じ300にする
    private float continueTime = 0.0f;
    private float blinkTime = 0.0f;

    private bool isBossAttack;
    private bool isBossRun;
    private bool rightTleftF = false;
    private bool isDead = false;
    private bool isDamage = false;
    private bool isPlayerAttackBool;  //攻撃連続判定防止フラグ
    private bool isDeadSE;  //SEを1回だけ呼び出す
    private bool isBossEffectBool;  //ボスの攻撃を1回だけ呼び出す
    private bool isStartAttack;  //update内で明滅が終わったらフラグをオンにする

    #endregion

    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        sr = GetComponent<SpriteRenderer>();    //インスタンスを捕まえる  
        anim = GetComponent<Animator>();
        oc = GetComponent<ObjectCollision1>();
        col = GetComponent<BoxCollider2D>();
        slid = bossHPObj.GetComponent<Slider>();
        GameObject attackCheck = transform.GetChild(1).gameObject;
        attackCheck.SetActive(false);
        anim.Play("RadishWTigerBossStand");
    }

    void Update()
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
            GameObject attackCheck = transform.GetChild(1).gameObject;
            attackCheck.SetActive(true);
            if (continueTime > 0.5f)
            {
                //0.5f経ったら攻撃を止める
                isBossAttack = false;
                attackCheck.SetActive(false);
                isStartAttack = false;
                isBossRun = true;
                isBossEffectBool = false;
                this.tag = "BossEnemy";
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
                if (isBossAttack)
                {
                    //ボス攻撃中は移動を止める
                    xVector = 0;
                    isBossRun = false;
                }
                else if (rightTleftF)
                {
                    xVector = 1;
                    isBossRun = true;
                    transform.localScale = new Vector3(-1, 1, 1);
                }
                else
                {
                    isBossRun = true;
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
           　　 this.tag = "Untagged";
                if (!isPlayerAttackBool)
                {
                    bossHP -= 25;
                    if (GManager1.instance != null)
                    {
                        GManager1.instance.PlaySE(damageSE);
                    }
                    isDamage = true;
                    isPlayerAttackBool = true;
                    Invoke("StopPlayerAttack", 1.2f); //1.2秒後に連続判定防止フラグを戻す
                }
        }
        if (collision.gameObject.tag == "Syuriken")
        {
           bossHP -= 2;
           if (GManager1.instance != null)
           {
               GManager1.instance.PlaySE(damageSE);
           }
        }

            if (bossHP <= 0)
            {
                if (!isDeadSE)
                {
                    isDeadSE = true;
                    isDamage = true;
                    if (GManager1.instance != null)
                    {
                    GManager1.instance.PlaySE(deadSE);
                    GManager1.instance.score += myScore;
                    }

                    bossHPObj.SetActive(false);
                    bossBGMObj.SetActive(false);
                    Invoke("BossStageClear", 1f);
                    Destroy(gameObject, 1.5f);
                    Destroy(timeObj);
                }
            }
    }


    //アニメーションを設定する
    private void SetAnimation()
    {
        anim.SetBool("BossRunBool", isBossRun);
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
        GameObject attackCheck = transform.GetChild(1).gameObject;
        attackCheck.SetActive(false);
        isBossRun = true;
        this.tag = "BossEnemy";
    }

}
