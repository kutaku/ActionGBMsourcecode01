using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


public class TheEnd1 : MonoBehaviour
{
    //CanvasMessageWindowオブジェクトにアタッチする

    [Header("メッセージのフェードスピード")] public float fadespeed = 0.3f;
    [Header("上昇量")] public float moveDis = 10.0f;
    [Header("上昇時間")] public float moveTime = 1.0f;
    [Header("メッセージのキャンバスグループパネルオブジェクト")] public CanvasGroup cg;
    [Header("フェードアウトする黒幕Fadeオブジェクト")] public GameObject FadeObj;
    
    //theend文字UIの位置調整用変数
    private Vector3 defaultPos;
    private float timer = 0.0f;
    
    void Start()
    {
        if (cg == null)
        {
            Debug.Log("ボタンインスペクターの設定が足りません");
            Destroy(this);
        }
        else
        {
            cg.alpha = 0.0f;
            defaultPos = cg.transform.position;
            cg.transform.position = defaultPos - Vector3.up * moveDis;
            FadeObj.SetActive(true);
        }

    }

    void Update()
    {
        //上昇しながら文字がフェードインする
        if (cg.transform.position.y < defaultPos.y || cg.alpha < 1.0f)
        {
            cg.alpha = timer / moveTime;
            cg.transform.position += Vector3.up * (moveDis / moveTime) * fadespeed * Time.deltaTime;
            timer += fadespeed * Time.deltaTime;
        }
        //文字フェードイン完了
        else
        {
            cg.alpha = 1.0f;
        }
    }
}
