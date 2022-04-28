using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GManager1 : MonoBehaviour
{
    public static GManager1 instance = null;
    [Header("スコア")]public int score;
    [Header("現在のステージ")] public int stageNum;
    [Header("現在の復帰位置")] public int continueNum;
    [Header("現在の残機")] public int heartNum;
    [Header("デフォルトの残機")]  public int  defaultHeartNum;
    [HideInInspector] public int timeCounter;    //制限時間
    [HideInInspector] public bool isGameOver;
    [HideInInspector] public bool isStageClear;
    [HideInInspector] public bool isEntrance;

    private AudioSource audioSource = null;

    private void Awake()
    {
        if(instance == null)        //インスタンスの初期化
        {
            instance = this;
            DontDestroyOnLoad(this.gameObject);
        }
        else
        {
            Destroy(this.gameObject);
        }
    }

    private void Start()
    {
        audioSource = GetComponent<AudioSource>();
    }

    /// <summary>
    /// 残機を1増やす
    /// </summary>
    public void AddHeartNum()
    {
        if(heartNum < 99)
        {
            ++heartNum;
        }
    }

    /// <summary>
    /// 残機を1減らす
    /// </summary>
    public void SubHeartNum()
    {
        if(heartNum > 0)
        {
            --heartNum;
            
            if (heartNum == 0)
            {
                //ゲーム―オーバーを呼び出す
                isGameOver = true;
                Debug.Log("GMからゲームオーバーを呼び出します");
            }
        }
        else
        {
            isGameOver = true;
        }
    }

    /// <summary>
    /// 最初から始めるときの処理
    /// </summary>
    public void RetryGame()
    {
        isGameOver = false;
        heartNum = defaultHeartNum;
        score = 0;
        stageNum = 8;       //後編からスタートなので8から始める
        continueNum = 0;
        timeCounter = 0;
    }

    //ステージをやり直す時用
    public void ResettoStage()
    {
        isGameOver = false;
        heartNum = defaultHeartNum;
        score = 0;
        continueNum = 0;
        timeCounter = 0;
    }

    //次のステージへ行ったときはスコアを引き継ぐ
    public void NextStage()
    {
        isGameOver = false;
        continueNum = 0;
        timeCounter = 0;
    }


    //ボス戦でプレイヤーがやられた時用
    public void ResetBossStage()
    {
        isGameOver = false;
        score = 0;
        continueNum = 0;
        timeCounter = 0;
    }


    public void PlaySE(AudioClip clip)
    {
        if(audioSource != null)
        {
            audioSource.PlayOneShot(clip);
        }
        else
        {
            Debug.Log("オーディオソースが設定されていません");
        }

    }
}
