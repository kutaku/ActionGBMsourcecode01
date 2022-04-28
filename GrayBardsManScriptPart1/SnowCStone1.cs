using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SnowCStone1 : MonoBehaviour
{
    [Header("砕けた時のSE")] public AudioClip deadSE;
    [Header("加算スコア")] public int myScore = 3000;

    private Animator anim;
    private int sCStoneHP = 40;
    private bool isPlayerAttackBool;
    private bool isDeadSE;


    // Start is called before the first frame update
    void Start()
    {
        anim = GetComponent<Animator>();
    }


    public void OnTriggerEnter2D(Collider2D collision)
    {
        if (!isPlayerAttackBool)
        {
            //攻撃時の当たり判定
            if (collision.gameObject.tag == "Attack")
            {
                isPlayerAttackBool = true;
                sCStoneHP -= 10;
                Invoke("StopPlayerAttack", 0.5f); //0.3秒後に連続判定防止フラグを戻す
            }
        }

        if(sCStoneHP >= 40)
        {
            //通常の姿
            anim.Play("SnowCStoneStand");
        }
        else if (sCStoneHP >= 30 && sCStoneHP <= 39)
        {
            //ヒビが入る
            anim.Play("SnowCStoneCrack");
        }
        else if(sCStoneHP <= 20 && sCStoneHP >= 1)
        {
            //ヒビがたくさん入る
            anim.Play("SnowCStoneCrackMany");
        }
        else if (sCStoneHP <= 0)
        {
            if (!isDeadSE)
            {
                //粉砕される
                GManager1.instance.PlaySE(deadSE);
                isDeadSE = true;
                anim.Play("SnowCStoneBreak");
                GManager1.instance.score += myScore;
                Destroy(gameObject, 1f);
            }
        }

    }

    private void StopPlayerAttack()
    {
        isPlayerAttackBool = false;
    }


}
