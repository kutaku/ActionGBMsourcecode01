using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Heart1 : MonoBehaviour
{
    private Text heartText = null;
    private int oldHeartNum = 0;

    // Start is called before the first frame update
    void Start()
    {
        heartText = GetComponent<Text>();
        if (GManager1.instance != null)
        {
            heartText.text = "×" + GManager1.instance.heartNum;
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
        if (oldHeartNum != GManager1.instance.heartNum)
        {
            heartText.text = "×" + GManager1.instance.heartNum;
            oldHeartNum = GManager1.instance.heartNum;        //スコアが変わった時だけ更新
        }

    }
}
