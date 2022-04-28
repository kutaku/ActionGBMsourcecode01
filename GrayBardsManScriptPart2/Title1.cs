using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class Title1 : MonoBehaviour
{
    [Header("フェード")] public FadeImage1 fade;
    [Header("ゲームスタート時に鳴らすSE")] public AudioClip startSE;

    private bool firstPush = false;
    private bool goNextScene = false;

    //スタートボタンを押されたら呼ばれる
    public void PressStart()
    {
        Debug.Log("Press Start!");

        if (!firstPush) 
        {
            GManager1.instance.PlaySE(startSE);
            fade.StartFadeOut();
            firstPush = true;
        }
    }

    private void Update()
    {
        if (!goNextScene && fade.IsFadeOutComplete())
        {
            //次のシーンへ行く命令を書く
            SceneManager.LoadScene("stage8");
            goNextScene = true;
        }

    }

}
