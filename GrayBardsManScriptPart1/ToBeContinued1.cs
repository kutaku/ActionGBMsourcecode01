using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class ToBeContinued1 : MonoBehaviour
{
    [Header("フェードObj")] public FadeImage1 fadeObj;

    private bool startFade = false;
    private bool doSceneChange = false;
    private bool doLaterFade = false;

    // Start is called before the first frame update
    void Start()
    {
        //7秒後にタイトルに戻る
        Invoke("LaterStartFade", 7f); 
    }

    // Update is called once per frame
    void Update()
    {
        //ステージを切り替える
        if (fadeObj != null && startFade && !doSceneChange && doLaterFade)
        {
            if (fadeObj.IsFadeOutComplete())
            {
                //次のステージ
                SceneManager.LoadScene("titleScenes");
                doSceneChange = true;
            }
        }
    }

    //指定した秒後にフェードを開始する
    private void LaterStartFade()
    {
        Debug.Log("フェードを呼びます");
        doLaterFade = true;
        if (fadeObj != null)
        {
            fadeObj.StartFadeOut();
            startFade = true;
        }
    }
}
