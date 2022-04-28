using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
//using UnityEngine.SceneManagement;

//  デフォルトではCanvas5timeUpオブジェクトは非アクティブにしておく(子オブジェクトはアクティブ)
// レイヤー階層はCanvas5timeUp > InvalidationWallUI

public class TimeCounter1 : MonoBehaviour
{
    [SerializeField] private GameObject timeUpUIObject; //タイムアップになった時に表示するオブジェクト
    //[SerializeField] private GameObject invalidationUIObject;     //画面上から被せて操作を無効にするUIオブジェクト
    [Header("タイムアップ時にやられたSE")] public AudioClip downSE;
    [Header("タイムアップ時に鳴らすSE")] public AudioClip timeUpSE;
    [Header("停止するBGM")] public AudioSource audioSource;

    private GameObject timeUpUIInstance;    //タイムアップのインスタンス
    private bool isTimeUP = false;  //タイムアップフラグ

    //カウントダウン
    public float countdown = 10.0f;
    //時間を表示するText型の変数
    public Text timeText;

    private void Start()
    {
        timeUpUIObject.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {


        //時間をカウントする
        countdown -= Time.deltaTime;

        //時間を表示する
        timeText.text = "TIME: " + countdown.ToString("f1");

        //countdownが0以下になったとき
        if (countdown <= 0)
        {
            if (!isTimeUP)      //1回だけ呼び出す
            {
                isTimeUP = true;
                timeText.text = "TIME UP!!";
                GManager1.instance.PlaySE(downSE);
                GManager1.instance.PlaySE(timeUpSE);
                audioSource.Stop();
                Time.timeScale = 0f;
                timeUpUIInstance = GameObject.Instantiate(timeUpUIObject) as GameObject;
                timeUpUIInstance.SetActive(true);

            }
        }
    }
}