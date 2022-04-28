using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;       //スライダーUIを使うときは忘れずに書く

public class BossDoragon1 : MonoBehaviour
{
    [Header("ダメージSE")] public AudioClip damageSE;
    [Header("攻撃時SE")] public AudioClip attackSE;
    [Header("倒された時のSE")] public AudioClip deadSE;
    [Header("HPゲージsliderObj")] public GameObject bossHPObj;
    [Header("ボスBGMObj")] public GameObject bossBGMObj;
    [Header("加算スコア")] public int myScore = 30000;
    [SerializeField] private GameObject attackEffectObj;     //ボスの攻撃時のキュウリオブジェクト
    [Header("攻撃エフェクトの親オブジェクト")] public GameObject ParentBossEffectAtkObj;   //BossEffectAtkオブジェクトはオフ(チェックを外す)にしておく。その子オブジェクトはオンにしておく。
    [Header("タイムカウンターObj")] public GameObject timeObj;
    [HideInInspector] public bool isBossClear;

    #region
    private Animator anim;
    private Slider slid;
    private int bossHP = 250;
    private bool isPlayerAttackBool;  //攻撃連続判定防止フラグ
    private bool isDeadSE;  //SEを1回だけ呼び出す
    private bool isAttack;
    #endregion


    // Start is called before the first frame update
    void Start()
    {
        anim = GetComponent<Animator>();
        slid = bossHPObj.GetComponent<Slider>();
        ParentBossEffectAtkObj.SetActive(false);

        //"Attack"を10秒後に呼びだされ、3秒ごとに呼びだす。
        if (!isAttack)
        {
            InvokeRepeating("Attack", 10, 3);
        }
    }

    // Update is called once per frame
    void Update()
    {
        slid.value = bossHP;
    }


    public void OnTriggerEnter2D(Collider2D collision)
    {
        if (!isPlayerAttackBool)
        {
        //攻撃時の当たり判定
            if (collision.gameObject.tag == "Attack")
            {
                this.tag = "DamageEnemy";
                bossHP -= 25;
                anim.Play("BossDragonDamage");
                anim.Play("BossDragonStand");
                if (GManager1.instance != null)
                {
                    GManager1.instance.PlaySE(damageSE);
                }
                isPlayerAttackBool = true;
                Invoke("StopPlayerAttack", 1f); //1秒後に連続判定防止フラグを戻す
            }
        }

        if (collision.gameObject.tag == "Syuriken")
        {
            bossHP -= 2;
            anim.Play("BossDragonDamage");
            anim.Play("BossDragonStand");
            if (GManager1.instance != null)
            {
                GManager1.instance.PlaySE(damageSE);
            }
        }

        if (bossHP <= 0)
        {
            if (!isDeadSE)
            {
                GManager1.instance.PlaySE(deadSE);
                isDeadSE = true;
                isAttack = true;
                anim.Play("BossDragonDead");
                this.tag = "DeadEnemy";
                bossHPObj.SetActive(false);
                bossBGMObj.SetActive(false);
                GManager1.instance.score += myScore;
                Invoke("BossStageClear", 1f);
                Destroy(gameObject, 1.5f);
                Destroy(attackEffectObj);
                Destroy(timeObj);
            }
        }

    }

    public void OnTriggerExit2D(Collider2D collision)
    {
        if(collision.gameObject.tag == "Player")
        {
            this.tag = "Untagged";
        }
    }

    //ボスの攻撃エフェクトを呼び出す
    public void Attack()
    {
        Instantiate(attackEffectObj, transform.position, Quaternion.identity);
        GManager1.instance.PlaySE(attackSE);
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
