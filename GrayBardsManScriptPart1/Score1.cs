using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Score1 : MonoBehaviour
{
    private Text scoreText = null;
    private int oldScore = 0;

    // Start is called before the first frame update
    void Start()
    {
        scoreText = GetComponent<Text>();
        if(GManager1.instance != null)
        {
            scoreText.text = "SCORE:" + GManager1.instance.score;
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
        if(oldScore != GManager1.instance.score)
        {
            scoreText.text = "SCORE:" + GManager1.instance.score;
            oldScore = GManager1.instance.score;        //スコアが変わった時だけ更新
        }
        
    }
}
