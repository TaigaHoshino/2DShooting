using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjectController : MonoBehaviour
{
    GameObject player;
    GameDirector gameDirector;
    float distance;

    // Start is called before the first frame update
    void Start()
    {
        player = GameObject.Find("Player");
        gameDirector = GameObject.FindWithTag("GameDirector").GetComponent<GameDirector>();
    }

    // Update is called once per frame
    void Update()
    {
        if (gameDirector.gameOver)
        {
            GetComponent<ObjectController>().enabled = false;
        }
        else
        {
            distance = transform.position.x - player.transform.position.x;
            if (distance < -30f)
            {
                Destroy(gameObject);
            }
        }
    }
}
