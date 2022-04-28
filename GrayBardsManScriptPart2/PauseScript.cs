using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;


public class PauseScript : MonoBehaviour
{
    //デフォルトではCanvas2Pause,Canvas3title,Canvas4retryStage,InvalidationWallUIは非アクティブにしておく
    //(子オブジェクトはすべてチェック(アクティブ状態)を入れておく)
    //レイヤー階層は上から順にCanvas3title == Canvas4retryStage > InvalidationWallUI > Canvas2Pause


    [SerializeField] private GameObject pauseUIObject;    //ポーズした時に表示するUIオブジェクト
    [SerializeField] private GameObject retryToStageUIObject;   //ステージをやり直すを押したときに表示するオブジェクト
    [SerializeField] private GameObject returnTitleObject;    //タイトルに戻るを押したときに表示するオブジェクト
    [SerializeField] private GameObject invalidationUIObject;     //ポーズUIの上から被せてポーズUI操作を無効にするUIオブジェクト

    //ポーズ解除フラグ
    private bool isUnPose = false;
    //時間を再び動かすフラグ
    private bool isTime = false;
    //ポーズUIのインスタンス
    private GameObject pauseUIInstance;
    //ステージをやり直すUIのインスタンス
    private GameObject retryToStageUIInstance;
    //タイトルに戻りますか？インスタンス
    private GameObject returnTitleUIInstance;
    //ポーズUIの上に被せて操作を無効にするUIオブジェクト
    private GameObject invalidationUIInstance;

    // Update is called once per frame
    void Update()
      {
        if (Input.GetKeyDown("x"))
        {
            if (pauseUIInstance == null)
            {
                pauseUIInstance = GameObject.Instantiate(pauseUIObject) as GameObject;
                pauseUIInstance.SetActive(true);
                Time.timeScale = 0f;
            }
            else
            {
                Destroy(pauseUIInstance);
                Time.timeScale = 1f;
            }
        }

         if (isUnPose)        //もしisUnPoseがtrueの時すでに生成されているポーズオブジェクトを破棄する処理
         {
            Destroy(pauseUIInstance);
            Time.timeScale = 1f;
            isUnPose = false;
         }

        if (isTime)
        {
            Time.timeScale = 1f;
            isTime = false;
        }


    }

    //ゲームに戻るボタンが押されたらタイムカウントを再開して、ポーズオブジェクトを破棄する
    public void ReturntoGame()
    {
        isUnPose = true;
     }

    //ステージをやり直すが押されたら生成されるUI
    public void RetrytoStage()
    {
        invalidationUIInstance = GameObject.Instantiate(invalidationUIObject) as GameObject;
        invalidationUIInstance.SetActive(true);

        retryToStageUIInstance = GameObject.Instantiate(retryToStageUIObject) as GameObject;
        retryToStageUIInstance.SetActive(true);
    }

    //ステージをやり直しますか？いいえボタン
    public void RetrytoStageNo()
    {
        Destroy(retryToStageUIInstance);
        Destroy(invalidationUIInstance);
    }

    //ステージをやり直しますか？はいボタン
    public void RetrytoStageYes()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
        GManager1.instance.ResettoStage();
        isTime = true;
    }


    /// <summary>
    /// タイトルに戻る→タイトルに戻りますか？(データは保存されません)　はい、いいえ
    /// </summary>

    public void ReturnTitle()       //タイトルに戻りますかUIの表示
    {
        invalidationUIInstance = GameObject.Instantiate(invalidationUIObject) as GameObject;
        invalidationUIInstance.SetActive(true);

        returnTitleUIInstance = GameObject.Instantiate(returnTitleObject) as GameObject;
        returnTitleUIInstance.SetActive(true);
    }

    public void ReturnTitleNo()     //いいえボタンの処理
    {
        Destroy(returnTitleUIInstance);
        Destroy(invalidationUIInstance);
    }

    public void ReturnTitleYes()    //はいボタンの処理
    {
        isTime = true;
        SceneManager.LoadScene("titleScenes");
    }

}

