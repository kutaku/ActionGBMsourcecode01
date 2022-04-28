using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BattleBossCollision1 : MonoBehaviour
{
    [Header("ボスの咆哮SE")] public AudioClip bossVoiceSE;
    [Header("ボス戦開始時にオンにする見えない壁Obj")] public GameObject invisibleWallObj;
    [Header("戦闘前被弾防止バリアObj")] public GameObject invisibleBarrierObj;
    [Header("ボスHPゲージObj")] public GameObject bossHPObj;
    [Header("ボスBGMObj")] public GameObject bossBGMObj;
    [Header("STRONG BATTLE!!のUI")] public GameObject StrongObj;
    [Header("タイムカウンターObj")] public GameObject timeObj;

    private void Start()
    {
        invisibleBarrierObj.SetActive(true);
        gameObject.SetActive(true);
        invisibleWallObj.SetActive(false);
        bossHPObj.SetActive(false);
        bossBGMObj.SetActive(false);
        StrongObj.SetActive(false);
        timeObj.SetActive(false);
    }

    public void OnTriggerEnter2D(Collider2D collision)
    {
        //playerが通った時咆哮をし、当たり判定オブジェクトを非表示にした後、見えない壁を表示する
        if (collision.gameObject.tag == "Player" )
        {
            if (GManager1.instance != null)
            {
                GManager1.instance.PlaySE(bossVoiceSE);
            }
            invisibleBarrierObj.SetActive(false);
            gameObject.SetActive(false);
            invisibleWallObj.SetActive(true);
            bossHPObj.SetActive(true);
            StrongObj.SetActive(true);

            //1f後にBGMをスタートさせる
            Invoke("BGMStart", 1f);
        }
    }

    public void BGMStart()
    {
        StrongObj.SetActive(false);
        bossBGMObj.SetActive(true);
        timeObj.SetActive(true);
    }

}
