using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SuzakuMoveObject2 : MonoBehaviour
{
    [Header("ボス朱雀オブジェクト本体")] public BossSuzaku1 BossDeadTrigger;
    [Header("朱雀の咆哮のSE")] public AudioClip voiceSE;
    [Header("経路移動")] public GameObject[] movePoint;

    private float speed;    //速さ
    private Rigidbody2D rb = null;
    private Animator anim;
    private int nowPoint = 0;
    private bool returnPoint = false;
    private bool FallPoint = false;
    private Vector2 oldPos = Vector2.zero;
    private Vector2 myVelocity = Vector2.zero;

    // Start is called before the first frame update
    void Start()
    {
        speed = 2.5f;    //速さ

        anim = GetComponent<Animator>();
        rb = GetComponent<Rigidbody2D>();
        if (movePoint != null && movePoint.Length > 0 && rb != null)
        {
            rb.position = movePoint[0].transform.position;
            oldPos = rb.position;
        }

    }

    public Vector2 GetVelocity()
    {
        return myVelocity;
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        if (movePoint != null && movePoint.Length > 1 && rb != null && !BossDeadTrigger.isDeadSE)
        {
            //通常進行
            if (!returnPoint)
            {
                int nextPoint = nowPoint + 1;

                if (Vector2.Distance(transform.position, movePoint[nextPoint].transform.position) > 0.1f)
                {
                    //現在地から次のポイントへのベクトルを作成
                    Vector2 toVector = Vector2.MoveTowards(transform.position, movePoint[nextPoint].transform.position, speed * Time.deltaTime);

                    //次のポイントへ移動
                    rb.MovePosition(toVector);
                }
                //次のポイントを1つ進める
                else
                {
                    rb.MovePosition(movePoint[nextPoint].transform.position);
                    ++nowPoint;

                    //2のポイント位置についたら速度を上げて滑空する
                    if(nowPoint == 2)
                    {
                        if (!FallPoint)
                        {
                            GManager1.instance.PlaySE(voiceSE);
                            anim.Play("BossSuzakuFall");
                            speed = 10f;
                        }
                    }

                    //4のポイントの位置についたら通常アニメになる
                    if(nowPoint == 4)
                    {
                        anim.Play("BossSuzakuFly");
                        speed = 2.5f;
                    }


                    //現在地が配列の最後だった場合
                    if (nowPoint + 1 >= movePoint.Length)
                    {
                        returnPoint = true;
                    }
                }
            }
            else
            {
                int nextPoint = nowPoint - 1;

                if (Vector2.Distance(transform.position, movePoint[nextPoint].transform.position) > 0.1f)
                {
                    //現在地から次のポイントへのベクトルを作成
                    Vector2 toVector = Vector2.MoveTowards(transform.position, movePoint[nextPoint].transform.position, speed * Time.deltaTime);

                    //次のポイントへ移動
                    rb.MovePosition(toVector);
                }
                //次のポイントを1つ戻す
                else
                {
                    rb.MovePosition(movePoint[nextPoint].transform.position);
                    --nowPoint;


                    if (nowPoint == 4)
                    {
                        Debug.Log("滑空します！！");
                        GManager1.instance.PlaySE(voiceSE);
                        anim.Play("BossSuzakuFall");
                        speed = 10f;
                        
                    }

                    if (nowPoint == 2)
                    {
                        Debug.Log("通常アニメに戻ります。。");
                        anim.Play("BossSuzakuFly");
                        speed = 2.5f;
                    }

                    //現在地が配列の最後だった場合
                    if (nowPoint + 1 >= movePoint.Length)
                    {
                        returnPoint = true;
                    }

                    //現在地が配列の最初だった場合
                    if (nowPoint <= 0)
                    {
                        returnPoint = false;
                    }

                }
            }
        }
        myVelocity = (rb.position - oldPos) / Time.deltaTime;
        oldPos = rb.position;
    }
}
