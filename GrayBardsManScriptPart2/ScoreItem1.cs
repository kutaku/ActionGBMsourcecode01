using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScoreItem1 : MonoBehaviour
{
    [Header("加算するスコア")] public int myScore;
    [Header("プレイヤーの判定")] public PlayerTriggerCheck1 playerCheck;
    [Header("アイテム取得時に鳴らすSE")] public AudioClip itemSE;


    // Update is called once per frame
    void Update()
    {
         if (playerCheck.isOn)
        {
            if(GManager1.instance != null)
            {
                GManager1.instance.score += myScore;
                GManager1.instance.PlaySE(itemSE);
                Destroy(this.gameObject);
            }
        }
    }
}
