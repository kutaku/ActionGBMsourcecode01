using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjectCollision1 : MonoBehaviour   //プレイヤーと踏んづけられた物の橋渡しをするスクリプト
{
    [Header("これを踏んだ時のプレイヤーが跳ねる高さ")] public float boundHeight;

    /// <summary>
    /// このオブジェクトをプレイヤーが踏んだかどうか
    /// </summary>
    [HideInInspector]public bool playerStepOn;

    

}
