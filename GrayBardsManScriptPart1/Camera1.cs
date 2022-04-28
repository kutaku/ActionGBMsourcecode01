using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Camera1 : MonoBehaviour
{
    public GameObject player;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        transform.position = new Vector3(player.transform.position.x, 1, -10);

        if (transform.position.x < 0)
        {
            transform.position = new Vector3(0, 1, -10);
        }

        if (transform.position.x >= 18)
        {
            transform.position = new Vector3(18, 1, -10);
        }
    }
}
