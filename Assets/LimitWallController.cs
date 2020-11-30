using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LimitWallController : MonoBehaviour
{
    GameObject player;
    GameDirector gameDirector;
    float playerPosX;
    // Start is called before the first frame update
    void Start()
    {
        player = GameObject.Find("Player");
        gameDirector = GameObject.FindWithTag("GameDirector").GetComponent<GameDirector>();
        playerPosX = player.transform.position.x;
    }

    // Update is called once per frame
    void Update()
    {
        if (gameDirector.gameOver)
        {
            GetComponent<LimitWallController>().enabled = false;
        }

        transform.position = new Vector3(playerPosX - 20f, player.transform.position.y, 0);
        if (player.transform.position.x > playerPosX)
        {
            playerPosX = player.transform.position.x;
        }
    }
}
