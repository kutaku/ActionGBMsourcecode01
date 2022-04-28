using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class ToBeContinued1 : MonoBehaviour
{
    [Header("�t�F�[�hObj")] public FadeImage1 fadeObj;

    private bool startFade = false;
    private bool doSceneChange = false;
    private bool doLaterFade = false;

    // Start is called before the first frame update
    void Start()
    {
        //7�b��Ƀ^�C�g���ɖ߂�
        Invoke("LaterStartFade", 7f); 
    }

    // Update is called once per frame
    void Update()
    {
        //�X�e�[�W��؂�ւ���
        if (fadeObj != null && startFade && !doSceneChange && doLaterFade)
        {
            if (fadeObj.IsFadeOutComplete())
            {
                //���̃X�e�[�W
                SceneManager.LoadScene("titleScenes");
                doSceneChange = true;
            }
        }
    }

    //�w�肵���b��Ƀt�F�[�h���J�n����
    private void LaterStartFade()
    {
        Debug.Log("�t�F�[�h���Ăт܂�");
        doLaterFade = true;
        if (fadeObj != null)
        {
            fadeObj.StartFadeOut();
            startFade = true;
        }
    }
}
