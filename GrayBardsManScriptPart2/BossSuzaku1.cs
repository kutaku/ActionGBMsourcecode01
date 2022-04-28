using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;       //スライダーUIを使うときは忘れずに書く

public class BossSuzaku1 : MonoBehaviour
{
    [Header("ダメージSE")] public AudioClip damageSE;
    [Header("朱雀の咆哮のSE")] public AudioClip voiceSE;
    [Header("唐辛子発射投下SE")] public AudioClip atkSE;
    [Header("紅葉ガードSE")] public AudioClip guardSE;
    [Header("接触判定")] public EnemyCollisionCheck1 checkCollision;
    [Header("HPゲージsliderObj")] public GameObject bossHPObj;
    [Header("ボスBGMObj")] public GameObject bossBGMObj;
    [Header("加算スコア")] public int myScore = 30000;
    [SerializeField] private GameObject attackEffectObj;     //ボスの攻撃時の唐辛子オブジェクト
    [Header("唐辛子攻撃エフェクトの親オブジェクト")] public GameObject ParentBossEffectAtkObj;   //BossEffectAtkオブジェクトはオフ(チェックを外す)にしておく。その子オブジェクトはオンにしておく。
    [Header("紅葉ガードオブジェクト")] public GameObject MomijiObj;
    [Header("タイムカウンターオブジェクト")] public GameObject TimeObj;
    [HideInInspector] public bool isBossClear;  //ステコンに倒したことを知らせるフラグ
    [HideInInspector] public bool isDeadSE;  //SEを1回だけ呼び出す
    [Header("ボスの移動ポイントの親オブジェクト")] public GameObject MoveStopObj;

    #region//プライベート関数
    private Animator anim;
    private Slider slid;
    private SpriteRenderer sr = null;
    private Rigidbody2D rb = null;
    private BoxCollider2D col = null;
    private ObjectCollision1 oc = null;
    private bool rightTleftF = false;
    private float blinkTime = 0.0f;
    private float continueTime = 0.0f;
    private bool isDamage = false;
    private bool isDead = false;
    private int bossHP = 400;   //ボスHPスライダーオブジェクトのMAXの値も同じ値にする
    private bool isPlayerAttackBool;  //攻撃連続判定防止フラグ
    #endregion


    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        sr = GetComponent<SpriteRenderer>();    //インスタンスを捕まえる  
        col = GetComponent<BoxCollider2D>();
        oc = GetComponent<ObjectCollision1>();
        anim = GetComponent<Animator>();
        slid = bossHPObj.GetComponent<Slider>();
        ParentBossEffectAtkObj.SetActive(false);
        MomijiObj.SetActive(false);

        //"Attack"を10秒後に呼びだされ、3秒ごとに呼びだす。
        InvokeRepeating("Attack", 10f, 3f);
        InvokeRepeating("Guard", 12f, 8f);
    }

    // Update is called once per frame
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
                sr.enabled = true;
            }
            else
            {
                blinkTime += Time.deltaTime;                //演出中は演出用の時間を進める
                continueTime += Time.deltaTime;
            }
        }
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        //アニメーションを適用
        SetAnimation();

             if (sr.isVisible)
            {
                if (checkCollision.isOn)
                {
                    rightTleftF = !rightTleftF;
                }
                if (rightTleftF)
                {
                    transform.localScale = new Vector3(-1, 1, 1);
                }
                else
                {
                    transform.localScale = new Vector3(1, 1, 1);
                }

            }
            else
            {
                rb.Sleep();     //画面に映っていない時物理演算を切る
            }
        
    }


    public void OnTriggerEnter2D(Collider2D collision)
    {
        if (!isPlayerAttackBool)
        {
            //攻撃時の当たり判定
            if (collision.gameObject.tag == "Attack")
            {
                isDamage = true;
                this.tag = "DamageEnemy";
                bossHP -= 25;
                if (GManager1.instance != null)
                {
                    GManager1.instance.PlaySE(damageSE);
                }
                isPlayerAttackBool = true;
                if (!isDead)
                {
                    Invoke("StopPlayerAttack", 1f); //1秒後に連続判定防止フラグを戻す
                }
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
                GManager1.instance.PlaySE(voiceSE);
                isDeadSE = true;
                isDead = true;
                anim.Play("BossSuzakuDead");
                this.tag = "DeadEnemy";
                bossHPObj.SetActive(false);
                bossBGMObj.SetActive(false);
                GManager1.instance.score += myScore;
                Invoke("BossStageClear", 1f);
                Destroy(gameObject, 1.5f);
                Destroy(attackEffectObj);
                Destroy(MomijiObj);
                Destroy(TimeObj);
                Destroy(MoveStopObj);
            }
        }

    }

    public void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "Player")
        {
            this.tag = "Untagged";
        }
    }

    //アニメーションを設定する
    private void SetAnimation()
    {
        //anim.SetBool("BossAttackBool", isBossAttack);
    }

    //ボスの攻撃エフェクトを呼び出す
    public void Attack()
    {
        Instantiate(attackEffectObj, transform.position, Quaternion.identity);
        if (GManager1.instance != null)
        {
            GManager1.instance.PlaySE(atkSE);
        }
    }

    public void Guard()
    {
        MomijiObj.SetActive(true);
        if (GManager1.instance != null)
        {
            GManager1.instance.PlaySE(guardSE);
        }
        //指定した秒後に非アクティブにする
        Invoke("MomijiOff", 1f);
    }

    private void MomijiOff()
    {
        MomijiObj.SetActive(false);
    }

    //ボスを倒したことをステージコントロールに伝える
    public void BossStageClear()
    {
        isBossClear = true;
    }

    private void StopPlayerAttack()
    {
        this.tag = "BossEnemy";
        isPlayerAttackBool = false;
    }

}
