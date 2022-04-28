using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MomijiGard1 : MonoBehaviour
{
    [SerializeField] private Vector3 speed;

    //指定した速さで回転し続ける
    private void LateUpdate()
    {
        transform.Rotate(speed, Space.Self);
    }
}
