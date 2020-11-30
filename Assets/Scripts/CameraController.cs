using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour
{
    GameObject player;
    GameDirector gameDirector;
    float attenRate = 3.0f;
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
            GetComponent<CameraController>().enabled = false;
        }

        var pos = new Vector3(player.transform.position.x + 4,
            player.transform.position.y + 1,
            -3);
        transform.position = Vector3.Lerp(transform.position, pos, Time.deltaTime * attenRate);
    }
}
