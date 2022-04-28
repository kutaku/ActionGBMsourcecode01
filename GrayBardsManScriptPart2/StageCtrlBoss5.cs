using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class StageCtrlBoss5 : MonoBehaviour
{
    [Header("ボス朱雀オブジェクト")] public BossSuzaku1 BossClearTrigger;
    [Header("ボスを倒してクリアSE")] public AudioClip bossClearSE;
    [Header("ステージクリア演出UI")] public GameObject stageClearObj;
    [Header("フェードObj")] public FadeImage1 fadeObj;


    private int nextStageNum;
    private bool startFade = false;
    private bool doSceneChange = false;
    private bool doClear = false;
    private bool doLaterFade = false;

    // Start is called before the first frame update
    void Start()
    {
        stageClearObj.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {
        if (BossClearTrigger.isBossClear && !doClear)
        {
            Debug.Log("ボスノックダウンを呼びます");
            BossKnockDown();
            doClear = true;
        }


        //ステージを切り替える
        if (fadeObj != null && startFade && !doSceneChange && doLaterFade)
        {
            if (fadeObj.IsFadeOutComplete())
            {
                //次のステージ
                GManager1.instance.NextStage();
                GManager1.instance.stageNum = nextStageNum;
                GManager1.instance.isStageClear = false;
                SceneManager.LoadScene("stage" + nextStageNum);
                doSceneChange = true;
            }
        }

    }

    //ボス戦でボスを倒したときの処理
    public void BossKnockDown()
    {
        GManager1.instance.isStageClear = true;     //フラグをオン、演出用オブジェクトをアクティブに
        stageClearObj.SetActive(true);
        GManager1.instance.PlaySE(bossClearSE);
        Invoke("LaterStartFade", 5f);
    }

    /// <summary>
    /// ステージを切り替えます
    /// </summary>
    /// <param name="num">ステージ番号</param>
    public void ChangeScene(int num)
    {
        if (fadeObj != null)
        {
            nextStageNum = num;
            fadeObj.StartFadeOut();
            startFade = true;
        }
    }

    //ボスを打倒し演出が終わった後にフェードを開始する
    private void LaterStartFade()
    {
        Debug.Log("フェードを呼びます");
        ChangeScene(GManager1.instance.stageNum + 1);
        doLaterFade = true;
    }

}
