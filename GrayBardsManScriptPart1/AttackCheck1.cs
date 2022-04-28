using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//プレイヤーのAttackCheckオブジェクトにアタッチ
public class AttackCheck1 : MonoBehaviour
{

    //オブジェクトがアクティブになっている時
    private void OnEnable()
    {

        //"メソッド名", 〇秒後にメソッドを実行
        Invoke("AttackOff", 1);

    }

  private  void AttackOff()
    {
        gameObject.SetActive(false);
    }
}

