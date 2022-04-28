using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;       //スライダーUIを使うときは忘れずに書く


public class Player1 : MonoBehaviour
{
    #region//インスペクターで設定する
    [Header("移動速度")] public float speed;
    [Header("ジャンプ速度")] public float jumpSpeed;
    [Header("ジャンプする高さ")] public float jumpHeight;
    [Header("ジャンプする長さ")] public float jumpLimitTime;
    [Header("踏みつけ判定の高さの割合")] public float stepOnRate;
    [Header("重力")] public float gravity;
    [Header("接地判定 ")] public GroundCheck1 ground;
    [Header("頭をぶつけた判定")] public GroundCheck1 head;
    [Header("ステージコントロール")] public StageCtrl1 stageCtrl;
    [Header("ダッシュの速さ表現")] public AnimationCurve dashCurve;
    [Header("ジャンプの速さ表現")] public AnimationCurve jumpCurve;
    [Header("ジャンプする時に鳴らすSE")] public AudioClip jumpSE;
    [Header("やられたら鳴らす")] public AudioClip downSE;
    [Header("コンティニュー時に鳴らす")] public AudioClip continueSE;
    [Header("斬る時に鳴らす")] public AudioClip attackSE;
    [Header("HPゲージsliderObj")] public GameObject playerHPObj;

    #endregion

    #region//プライベート変数
    private Animator anim = null;   //何も代入しない場合も中身はnull
    private Rigidbody2D rb = null;
    private CapsuleCollider2D capcol = null;
    private SpriteRenderer sr = null;
    private MoveObject1 moveObj = null;
    private Slider slid;
    
    private bool isGround = false;
    private bool isHead = false;
    private bool isJump = false;
    private bool isRun = false;
    private bool isDown = false;
    private bool isOtherJump = false;
    private bool isContinue = false;
    private bool nonDownAnim = false;
    private bool isClearMotion = false;
    private float continueTime = 0.0f;
    private float blinkTime = 0.0f;
    private float jumpPos = 0.0f;   //ジャンプした位置を記録する変数
    private float otherJumpHeight = 0.0f;
    private float jumpTime = 0.0f;  //滞空時間
    private float dashTime = 0.0f;
    private float beforeKey = 0.0f;
    private float playerHP;             //プレイヤーHPの内部処理
    private string enemyTag = "Enemy";
    private string bossEnemyTag = "BossEnemy";
    private string deadAreaTag = "DeadArea";
    private string hitAreaTag = "HitArea";
    private string moveFloorTag = "MoveFloor";
    private string fallFloorTag = "FallFloor";
    private bool isAttack = false;
    private bool isAttackSE = false;
    private bool isDashAttack = false;
    private bool isDashAttackTrans = false;
    private bool isSyuriken = false;
    #endregion



    void Start()
    {
        //コンポーネントのインスタンスを捕まえる
        anim = GetComponent<Animator>();
        rb = GetComponent<Rigidbody2D>();
        capcol = GetComponent<CapsuleCollider2D>();
        sr = GetComponent<SpriteRenderer>();
        slid = playerHPObj.GetComponent<Slider>();

        //プレイヤーのデフォルトHPは300
        playerHP = 300;
    }

    private void Update()
    {
        slid.value = playerHP;


        if (isContinue)     //フラグがオンの時、時間経過でオンオフ切り替え
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
            if (continueTime > 1.0f)
            {
                isContinue = false;
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

        //スペースキーを離した時に攻撃フラグを戻す
        if (Input.GetKeyUp("space"))
        {
            isAttackSE = false;
            isAttack = false;
            isDashAttack = false;
        }
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        if (!isDown && !GManager1.instance.isGameOver && !GManager1.instance.isStageClear)
        {
            //接地判定を得る
            isGround = ground.IsGround();
            isHead = head.IsGround();

            //各座標軸の速度を求める
            float xSpeed = GetXSpeed();
            float ySpeed = GetYSpeed();


            //アニメーションを適用
            SetAnimation();

            //移動速度を設定
            Vector2 addVelocity = Vector2.zero;
            if (moveObj != null )
            {
                addVelocity = moveObj.GetVelocity();
            }
            rb.velocity = new Vector2(xSpeed, ySpeed) + addVelocity;  //物理エンジンを使用する,プレイヤーの動く床の速度を足す
        }
        else
        {
            if (!isClearMotion && GManager1.instance.isStageClear)
            {
                anim.Play("playerClear");
                isClearMotion = true;
            }
            rb.velocity = new Vector2(0, -gravity);  //ダウン中は重力のみ適用
        }

    }


    /// <summary>
    /// Y成分で必要な計算をし、速度を返す。
    /// </summary>
    /// <returns>Y軸の速さ</returns>
    private float GetYSpeed()       //Y軸の速度を決定して返すメソッド
    {
        float verticalKey = Input.GetAxis("Vertical");
        float ySpeed = -gravity;    //Y軸に対し何もしなければ重力を働かせる

        if (Input.GetKey("space") && verticalKey > 0)      //ジャンプ中にスペースキーが押されたら斬撃をする
        {
            if (!isAttackSE)
            {
                //1回だけ斬撃SEを鳴らす
                GManager1.instance.PlaySE(attackSE);
                isAttackSE = true;
            }
            isAttack = true;

        }
       

        if (Input.GetKey(KeyCode.B))        //ジャンプ中にBキーが押されたら手裏剣を投げる
        {
            isSyuriken = true;
        }
        else
        {
            //FixedUpdate内で1f毎に常に呼ばれる
            isSyuriken = false;
        }

        if (isOtherJump)
        {

            //現在の高さが飛べる高さより下か
            bool canHeight = jumpPos + otherJumpHeight > transform.position.y;
            //ジャンプ時間が長くなりすぎていないか
            bool canTime = jumpLimitTime > jumpTime;

            if (canHeight && canTime && !isHead)
            {
                ySpeed = jumpSpeed;
                jumpTime += Time.deltaTime;     //上昇している間に進んだゲーム内時間を毎回足すので合計はジャンプした時間
            }
            else
            {
                isOtherJump = false;
                jumpTime = 0.0f; //上昇できなくなった時時間をリセット
            }

        }

        else if (isGround)    //もし地面から
        {
            if (verticalKey > 0)     //ジャンプした時
            {
                if (verticalKey > 0)
                {
                    GManager1.instance.PlaySE(jumpSE);
                }
                ySpeed = jumpSpeed;
                jumpPos = transform.position.y; //ジャンプした高さを記録する
                isJump = true;
                jumpTime = 0.0f;
            }
            else
            {
                isJump = false;
            }
        }
        else if (isJump)
        {
            //上キーが押されているか
            bool pushUpKey = verticalKey > 0;
            //現在の高さが飛べる高さより下か
            bool canHeight = jumpPos + jumpHeight > transform.position.y;
            //ジャンプ時間が長くなりすぎていないか
            bool canTime = jumpLimitTime > jumpTime;

            if (pushUpKey && canHeight && canTime && !isHead)
            {
                ySpeed = jumpSpeed;
                jumpTime += Time.deltaTime;     //上昇している間に進んだゲーム内時間を毎回足すので合計はジャンプした時間
            }
            else
            {
                isJump = false;
                jumpTime = 0.0f; //上昇できなくなった時時間をリセット
            }

        }

        if (isJump || isOtherJump)
        {
            ySpeed *= jumpCurve.Evaluate(jumpTime);
        }
        return ySpeed;


    }


    /// <summary>
    /// X成分で必要な計算をし、速度を返す
    /// </summary>
    /// <returns>X軸の速さ</returns>
    private float GetXSpeed()
    {
        //キー入力されたら行動する
        float horizontalKey = Input.GetAxis("Horizontal");
        float xSpeed = 0.0f;


        if (Input.GetKey("space"))      //スペースキーが押されたら斬撃SEを鳴らす
        {
            if (!isAttackSE)
            {
                //1回だけ斬撃SEを鳴らす
                GManager1.instance.PlaySE(attackSE);
                isAttackSE = true;
            }
        }


        if (horizontalKey > 0 && Input.GetKey(KeyCode.B) || horizontalKey < 0 && Input.GetKey(KeyCode.B) )        //走ってるときにAキーが押されたら手裏剣を投げる
        {
            isSyuriken = true;
            dashTime += Time.deltaTime;
            if (horizontalKey < 0)
            {
                xSpeed = -speed;    //左を押したら負の速度
            }
            else
            {
                xSpeed = speed;     //右を押したら正の速度
            }
        }
        else if (Input.GetKey(KeyCode.B))
        {
            isSyuriken = true;
        }
        else if (horizontalKey > 0)           // 右が押されている場合
        {
            transform.localScale = new Vector3(1, 1, 1);
            isRun = true;
            isAttack = false;
            isSyuriken = false;
            dashTime += Time.deltaTime;
            xSpeed = speed;     //右を押したら正の速度

           //左右キーが押されている時にスペースキーが押されたらダッシュアタックをする
           if (Input.GetKey("space") && !isDashAttackTrans)
           {
           isDashAttack = true;
           Invoke("StopDashAttack", 3);    //3秒後にフラグを戻す
           }
        }
        else if (horizontalKey < 0)          //左が押されている場合
        {
            transform.localScale = new Vector3(-1, 1, 1);   //軸を反転する
            isRun = true;
            isAttack = false;
            isSyuriken = false;
            dashTime += Time.deltaTime;
            xSpeed = -speed;    //左を押したら負の速度

            //左右キーが押されている時にスペースキーが押されたらダッシュアタックをする
            if (Input.GetKey("space") && !isDashAttackTrans)
            {
                isDashAttack = true;
                Invoke("StopDashAttack", 3);    //3秒後にフラグを戻す
            }
        }
        else if (Input.GetKey("space"))      //スペースキーが押されたら斬撃をする
        {
            isAttack = true;
        }
        else                                          //入力なし
        {
            //FixedUpdate内で1f毎に常に呼ばれる
            isRun = false;
            isSyuriken = false;
            xSpeed = 0.0f;       //何も押していない時速度を0に
            dashTime = 0.0f;        //ダッシュする時間のリセット
        }

        //前回の入力からダッシュの反転を判断して速度を変える
        if (horizontalKey > 0 && beforeKey < 0)
        {
            dashTime = 0.0f;
        }
        else if (horizontalKey < 0 && beforeKey > 0)
        {
            dashTime = 0.0f;
        }
        beforeKey = horizontalKey;

        //アニメーションカーブを速度に適用
        xSpeed *= dashCurve.Evaluate(dashTime);
        beforeKey = horizontalKey;
        return xSpeed;

    }

    /// <summary>
    /// アニメーション設定をする
    /// </summary>
    private void SetAnimation()
    {
        anim.SetBool("jumpBool", isJump || isOtherJump);
        anim.SetBool("groundBool", isGround);
        anim.SetBool("runBool", isRun);
        anim.SetBool("spaceBool", isAttack);
        anim.SetBool("DashAtkBool", isDashAttack);
        anim.SetBool("syurikenBool", isSyuriken);
    }

    /// <summary>
    /// コンティニュー待機状態か
    /// </summary>
    /// <returns></returns>
    public bool IsContinueWaiting()
    {
        if (GManager1.instance.isGameOver)
        {
            return false;
        }
        else
        {
            return IsDownAnimEnd() || nonDownAnim;
        }

    }

    //ダウンアニメーションが完了しているかどうか
    private bool IsDownAnimEnd()
    {
        if (isDown && anim != null)
        {
            AnimatorStateInfo currentState = anim.GetCurrentAnimatorStateInfo(0);
            if (currentState.IsName("playerDown"))
            {
                if (currentState.normalizedTime >= 1)
                {
                    return true;
                }
            }
        }
        return false;
    }

    //斬撃アニメーション時に呼びだす
    private bool IsAttackCheck()
    {
        //デフォルトではプレイヤーオブジェクトの3個目にあるAttackCheck子オブジェクトのチェックを外しておく(非アクティブにしておく)
        GameObject attackCheck = transform.GetChild(2).gameObject;
        attackCheck.SetActive(false);
        
        if (isAttack || isDashAttack)
        {
            attackCheck.SetActive(true);
            return true;
        }
        else
        {
            attackCheck.SetActive(false);
            return false;
        }
    }

    //手裏剣を投げるアニメーション時に呼びだす処理
    private bool IsSyurikenThrow()
    {
        if (isSyuriken)
        {
            return true;
        }
        else
        {
            return false;
        }
    }

    /// <summary>
    /// コンティニューする
    /// </summary>
    public void ContinuePlayer()
    {
        GManager1.instance.PlaySE(continueSE);
        isDown = false;
        anim.Play("playerStand");
        isJump = false;
        isOtherJump = false;
        isRun = false;
        isContinue = true;
        isAttack = false;
        isDashAttack = false;
        nonDownAnim = false;
    }

    //やられた時の処理
    private void ReceiveDamage(bool downAnim)
    {
        GameObject attackCheck = transform.GetChild(2).gameObject;
        attackCheck.SetActive(false);

        if (isDown || GManager1.instance.isStageClear)     //もしダウン状態ではない時
        {
            return;
        }
        else
        {
            if (downAnim)
            {

                anim.Play("playerDown");
            }
            else
            {
                playerHP = 300;
                nonDownAnim = true;
            }
            isDown = true;
            GManager1.instance.PlaySE(downSE);
            GManager1.instance.SubHeartNum();
        }

    }


    private void StopDashAttack()
    {
        isDashAttackTrans = false;
    }


    //接触判定
    private void OnCollisionEnter2D(Collision2D collision)
    {
        bool enemy = (collision.collider.tag == enemyTag);
        bool bossEnemy = (collision.collider.tag == bossEnemyTag);
        bool moveFloor = (collision.collider.tag == moveFloorTag);
        bool fallFloor = (collision.collider.tag == fallFloorTag);


        if (enemy || moveFloor || fallFloor)
        {

            //踏みつけ判定になる高さ
            float stepOnHeight = (capcol.size.y * (stepOnRate / 100f));

            //踏みつけ判定のワールド座標
            float judgePos = transform.position.y - (capcol.size.y / 2f) + stepOnHeight;

            foreach (ContactPoint2D p in collision.contacts)
            {
                if (p.point.y < judgePos)     //衝突位置が足元より下
                {
                    if (enemy || fallFloor )
                    {
                        ObjectCollision1 o = collision.gameObject.GetComponent<ObjectCollision1>();
                        if (o != null)
                        {
                            if (enemy)
                            {
                                otherJumpHeight = o.boundHeight;    //踏んづけたものから跳ねる高さを取得する
                                o.playerStepOn = true;                       //踏んづけたものに対して踏んづけた事を通知する
                                jumpPos = transform.position.y;        //ジャンプした位置を記録する
                                isOtherJump = true;
                                isJump = false;
                                jumpTime = 0.0f;
                            }
                            else if (fallFloor)
                            {
                                o.playerStepOn = true;
                            }
                           
                        }
                        else
                        {
                            Debug.Log("ObjectCollision1が付いてません");
                        }
                    }
                    else if (moveFloor)
                    {
                        moveObj = collision.gameObject.GetComponent<MoveObject1>();
                    }
                    
                }
                else
                {
                    if (enemy)
                    {
                     
                        playerHP -= 75;
                        isContinue = true;      //明滅する
                        anim.Play("playerDown");
                        GManager1.instance.PlaySE(downSE);

                        if (playerHP <= 0)
                        {
                            //GManager1がライフを減らす前に参照してしまうので0になる前の残りライフ１を参照する
                            if (GManager1.instance.heartNum == 1)
                            {
                                Debug.Log("ダウンアニメをします");
                                ReceiveDamage(true);
                            }
                            else
                            {
                                //HPが0になったらダウンする
                                Debug.Log("HPが0になったのでスタート地点へ戻ります");
                                ContinuePlayer();
                                ReceiveDamage(false);
                            }
                        }
                        else
                        {
                            Debug.Log("起き上がります");
                            Invoke("PStandAnim", 0.5f);
                        }

                        break;
                    }
                }
            }
        }

        if (bossEnemy)
        {
            playerHP -= 75;
            isContinue = true;      //明滅する
            anim.Play("playerDown");
            GManager1.instance.PlaySE(downSE);
            Invoke("PStandAnim", 0.5f);

            if (playerHP <= 0)
            {
                //HPが0になったらダウンする
                Debug.Log("プレイヤーHPが0になりましたのでステージをやり直します");
                ContinuePlayer();
                stageCtrl.RetryBossStage();
                ReceiveDamage(true);
            }

        }
    }

    private void PStandAnim()
    {
        anim.Play("playerStand");
    }

    private void OnCollisionExit2D(Collision2D collision)
    {
        if (collision.collider.tag == moveFloorTag)
        {
            //動く床から離れた
            moveObj = null;
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.tag == deadAreaTag)
        {
            ReceiveDamage(false);       //落下判定時はダウンアニメーションをしない
        }
        else if (collision.tag == hitAreaTag)
        {
            ReceiveDamage(true);       //トゲの判定時ダウンアニメーションする
            playerHP = 300;
        }
    }
}
