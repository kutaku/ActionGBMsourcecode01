using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


public class FadeImage2fadein : MonoBehaviour
{
    //Fadeオブジェクトにアタッチする。フェードイン専用。

    //フェードインに使う変数
    private Image img = null;
    private float timer = 0.0f;
    private int frameCount = 0;
    private bool fadeIn = false;
    private bool fadeOut = false;
    private bool compFadeIn = false;

    /// <summary>
    /// フェードインを開始する
    /// </summary>
    public void StartFadeIn()
    {
        if (fadeIn || fadeOut)
        {
            return;
        }
        fadeIn = true;
        compFadeIn = false;
        timer = 0.0f;
        img.color = new Color(1, 1, 1, 1);
        img.fillAmount = 1;
        img.raycastTarget = true;
    }

    /// <summary>
    /// フェードインが完了したかどうか
    /// </summary>
    /// <returns></returns>
    public bool IsFadeInComplete()
    {
        return compFadeIn;
    }

    // Start is called before the first frame update
    void Start()
    {
        img = GetComponent<Image>();
        //5秒後にフェードインを開始する
        Invoke("StartFadeIn", 5f);
    }

    // Update is called once per frame
    void Update()
    {
        if (frameCount > 2)
        {
            if (fadeIn)
            {
                FadeInUpdate();
            }
        }
        ++frameCount;
    }

    //フェードイン中
    private void FadeInUpdate()
    {
        if (timer < 1f)
        {
            img.color = new Color(1, 1, 1, 1 - timer);
            img.fillAmount = 1 - timer;
        }
        //フェード完了
        else
        {
            FadeInComplete();
        }
        timer += Time.deltaTime;
    }


    //フェードイン完了
    public void FadeInComplete()
    {
        img.color = new Color(1, 1, 1, 0);
        img.fillAmount = 0;
        img.raycastTarget = false;
        timer = 0.0f;
        fadeIn = false;
        compFadeIn = true;
    }

}
