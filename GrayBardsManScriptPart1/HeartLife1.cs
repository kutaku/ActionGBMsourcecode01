using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HeartLife1 : MonoBehaviour
{
    [Header("加算するライフの数")] public int myLife;
    [Header("プレイヤーの判定")] public PlayerTriggerCheck1 playerCheck;
    [Header("ライフを1増やすSE")] public AudioClip lifeSE;

    void Update()
    {
        if (playerCheck.isOn)
        {
            if (GManager1.instance != null)
            {
                GManager1.instance.heartNum += myLife;
                GManager1.instance.PlaySE(lifeSE);
                Destroy(this.gameObject);
            }
        }
    }
}
