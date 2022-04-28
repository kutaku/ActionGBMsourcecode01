using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class gatemon1 : MonoBehaviour
{
    private Animator anim;
    private bool isOpen;

    // Start is called before the first frame update
    void Start()
    {
        anim = GetComponent<Animator>();
    }

    //プレイヤーが接触した時に門を開ける
    public void OnTriggerEnter2D(Collider2D collision)
    {
        //攻撃時の当たり判定
        if (collision.gameObject.tag == "Player")
        {
            if (!isOpen) 
            {
                anim.Play("gateOpen");
                isOpen = true;
            }
        }
    }

}
