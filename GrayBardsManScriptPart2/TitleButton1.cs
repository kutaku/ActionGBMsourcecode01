using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

//theendのMAINカメラに貼り付ける
public class TitleButton1 : MonoBehaviour
{
    [SerializeField] private GameObject returnTitleObject;    //タイトルボタンを押したときに表示するオブジェクト(非アクティブにしておく)
    [SerializeField] private GameObject invalidationUIObject;     //ポーズUIの上から被せてポーズUI操作を無効にするUIオブジェクト(非アクティブにしておく)

    //タイトルに戻りますか？インスタンス
    private GameObject returnTitleUIInstance;
    //ポーズUIの上に被せて操作を無効にするUIオブジェクト
    private GameObject invalidationUIInstance;


    /// <summary>
    /// タイトルに戻る→タイトルに戻りますか？(データは保存されません)　はい、いいえ
    /// </summary>

    public void ReturnTitle()       //タイトルに戻りますかUIの表示
    {
        invalidationUIInstance = GameObject.Instantiate(invalidationUIObject) as GameObject;
        invalidationUIInstance.SetActive(true);

        returnTitleUIInstance = GameObject.Instantiate(returnTitleObject) as GameObject;
        returnTitleUIInstance.SetActive(true);
    }

    public void ReturnTitleNo()     //いいえボタンの処理
    {
        Destroy(returnTitleUIInstance);
        Destroy(invalidationUIInstance);
    }

    public void ReturnTitleYes()    //はいボタンの処理
    {
        SceneManager.LoadScene("titleScenes");
    }
}
