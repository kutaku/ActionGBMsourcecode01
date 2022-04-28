using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BattoleBossCollision2Alert : MonoBehaviour
{
    [Header("強敵遭遇アラーム音SE")] public AudioClip bossAlarmSE;
    [Header("STRONG BATTLE!!演出のUI")] public GameObject StrongUIObj;
    [Header("HPゲージsliderObj")] public GameObject bossHPObj;
    [Header("ボスBGMObj")] public GameObject bossBGMObj;
    [Header("タイムカウンターObj")] public GameObject TimeObj;
    [Header("ボスオブジェクト")] public GameObject bossMainObj;

    private bool startbool;

    // Start is called before the first frame update
    private void Start()
    {
        StrongUIObj.SetActive(false);
        bossBGMObj.SetActive(false);
        bossHPObj.SetActive(false);
        TimeObj.SetActive(false);
        bossMainObj.SetActive(false);
    }

    public void OnTriggerEnter2D(Collider2D collision)
    {
        if (!startbool)
        {
            if (collision.gameObject.tag == "Player")
            {
                if (GManager1.instance != null)
                {
                    GManager1.instance.PlaySE(bossAlarmSE);
                }
                bossMainObj.SetActive(true);
                bossHPObj.SetActive(true);
                StrongUIObj.SetActive(true);

                //2秒後にBGMをスタートさせる
                Invoke("BGMStart", 2f);
                startbool = true;
            }
        }
     }

        public void BGMStart()
        {
            StrongUIObj.SetActive(false);
            bossBGMObj.SetActive(true);
            TimeObj.SetActive(true);
            //3秒後に自身のオブジェクトを破棄
            Destroy(gameObject, 3f);
        }

}
