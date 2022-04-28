using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class StageNum1 : MonoBehaviour
{
    private Text stageText = null;
    private int oldStageNum = 0;

    // Start is called before the first frame update
    void Start()
    {
        stageText = GetComponent<Text>();
        if (GManager1.instance != null)
        {
            stageText.text = "STAGE " + GManager1.instance.stageNum;
        }
        else
        {
            Debug.Log("ゲームマネージャー置き忘れています");
            Destroy(this);
        }

    }

    // Update is called once per frame
    void Update()
    {
        if (oldStageNum != GManager1.instance.stageNum)
        {
            stageText.text = "STAGE " + GManager1.instance.stageNum;
            oldStageNum = GManager1.instance.stageNum;        //スコアが変わった時だけ更新
        }

    }
}
