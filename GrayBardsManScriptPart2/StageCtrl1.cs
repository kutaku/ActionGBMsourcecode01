using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class StageCtrl1 : MonoBehaviour
{
    [Header("プレイヤーゲームオブジェクト")] public GameObject playerObj;
    [Header("コンティニュー位置")] public GameObject[] continuePoint;
    [Header("ゲームオーバーUI")] public GameObject gameOverObj;
    [Header("フェード")] public FadeImage1 fade;
    [Header("ゲームオーバー時に鳴らすSE")] public AudioClip gameOverSE;
    [Header("リトライ時に鳴らすSE")] public AudioClip retrySE;
    [Header("ステージクリアSE")] public AudioClip stageClearSE;
    [Header("入口に入るSE")] public AudioClip EntranceSE;
    [Header("ステージクリア演出UI")] public GameObject stageClearObj;
    [Header("ステージクリア判定")] public PlayerTriggerCheck1 stageClearTrigger;
    [Header("入口に入る判定オブジェクト")] public PlayerTriggerCheck1 enterEntranceTrigger;


    private Player1 p;
    private int nextStageNum;
    private bool startFade = false;
    private bool retryGame = false;
    private bool doSceneChange = false;
    private bool doClear = false;
    private bool doEntrance = false;
    private bool doGameOver = false;
    private bool doLaterFade = false;


    // Start is called before the first frame update
    void Start()
    {
        if(playerObj != null && continuePoint != null && continuePoint.Length > 0 && gameOverObj != null && fade != null)
        {
            gameOverObj.SetActive(false);
            stageClearObj.SetActive(false);
            playerObj.transform.position = continuePoint[0].transform.position;
            p = playerObj.GetComponent<Player1>();
            if(p == null)
            {
                Debug.Log("プレイヤーじゃない物がアタッチされています！");
            }
        }
        else
        {
            Debug.Log("設定が足りません！");
        }
        
    }

    // Update is called once per frame
    void Update()
    {
        //ゲームオーバー時の処理
        if(GManager1.instance.isGameOver && !doGameOver)
        {
            gameOverObj.SetActive(true);
            GManager1.instance.PlaySE(gameOverSE);
            doGameOver = true;
        }
        //プレイヤーがやられた時の処理
        else if(p != null && p.IsContinueWaiting() && !doGameOver)
        {
            if(continuePoint.Length > GManager1.instance.continueNum)       //コンティニューしたい位置の目印の設定が足りているか
            {
                playerObj.transform.position = continuePoint[GManager1.instance.continueNum].transform.position;
                p.ContinuePlayer();
            }
            else
            {
                Debug.Log("コンティニューポイントの設定が足りません！");
            }
        }
        else if (enterEntranceTrigger != null && enterEntranceTrigger.isOn && !doGameOver && !doClear && !doEntrance)    //判定内に入ったら、入口へ入る。
        {
            EnterEntrance();
            doEntrance = true;
            doLaterFade = true;
        }
        else if(stageClearTrigger != null && stageClearTrigger.isOn && !doGameOver && !doClear)     //クリア判定に入ったら、クリア処理を行う
        {
            StageClear();
            doClear = true;
            doLaterFade = true;
        }

        //ステージを切り替える
        if (fade != null && startFade && !doSceneChange && doLaterFade)
        {
            if (fade.IsFadeOutComplete())
            {
                //ゲームリトライ
                if (retryGame)
                {
                    GManager1.instance.RetryGame();
                    //後編からスタートなので9から始める
                    nextStageNum = 9;
                    Debug.Log("ゲームをやり直します");
                }
                //次のステージ
                else
                {
                    GManager1.instance.NextStage();
                    GManager1.instance.stageNum = nextStageNum;
                }
                GManager1.instance.isStageClear = false;
                SceneManager.LoadScene("stage" + nextStageNum);
                doSceneChange = true;
            }
        }
    }


    /// <summary>
    /// 最初から始める
    /// </summary>
    public void Retry()
    {
        ChangeScene(1);     //最初のステージに戻るので1
        retryGame = true;
    }

    /// <summary>
    /// ステージを切り替えます
    /// </summary>
    /// <param name="num">ステージ番号</param>
    public void ChangeScene(int num)
    {
        if (fade != null)
        {
            nextStageNum = num;
            fade.StartFadeOut();
            startFade = true;
        }
        doLaterFade = true;
    }

    /// <summary>
    /// ステージをクリアした
    /// </summary>
    public void StageClear()
    {
        GManager1.instance.isStageClear = true;         //フラグをオン、演出用オブジェクトをアクティブに
        stageClearObj.SetActive(true);
        GManager1.instance.PlaySE(stageClearSE);
    }

    //入口に入った時の処理
    public void EnterEntrance()
    {
        GManager1.instance.isEntrance = true;
        GManager1.instance.PlaySE(EntranceSE);
        ChangeScene(GManager1.instance.stageNum + 1);
    }


    //ボス戦でステージをやり直す
    public void RetryBossStage()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
        GManager1.instance.ResetBossStage();
    }

    //ボス戦でゲーム―オーバーからやり直す
    public void RetryBossGameOver()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
        GManager1.instance.ResettoStage();
    }

}
