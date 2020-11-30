using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WoodStandController : MonoBehaviour
{
    GameObject player;
    // Start is called before the first frame update
    void Start()
    {
        player = GameObject.Find("Player");
    }

    // Update is called once per frame
    void Update()
    {
        float distance = transform.position.x - player.transform.position.x;
        if (distance < -30f)
        {
            Destroy(gameObject);
        }
    }
}
