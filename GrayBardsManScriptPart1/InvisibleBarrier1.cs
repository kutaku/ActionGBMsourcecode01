using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InvisibleBarrier1 : MonoBehaviour
{
    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.tag == "Syuriken")
        {
            Destroy(collision.gameObject);
        }
    }
}
