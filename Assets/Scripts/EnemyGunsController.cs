using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyGunsController : MonoBehaviour
{
    GameObject enemy;
    GameObject player;
    GameDirector gameDirector;
    float armPosX;
    // Start is called before the first frame update
    void Start()
    {
        player = GameObject.Find("Player");
        enemy = transform.parent.gameObject;
        enemy = enemy.transform.GetChild(0).gameObject;
        gameDirector = enemy.GetComponent<EnemyController>().gameDirector;
        int random = Random.Range(1, 11);
        int i;
        for (i = 0; i < 4; i++)
        {
            transform.GetChild(i).gameObject.SetActive(false);
        }

        //transform.GetChild(0).gameObject.SetActive(true);

        if (0 < random && random <= 5)
        {
            transform.GetChild(0).gameObject.SetActive(true);
        }
        if (5 < random && random <= 7)
        {
            transform.GetChild(1).gameObject.SetActive(true);
        }
        if (7 < random && random <= 9)
        {
            transform.GetChild(2).gameObject.SetActive(true);
        }
        if (9 < random && random <= 10)
        {
            transform.GetChild(3).gameObject.SetActive(true);
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (gameDirector.gameOver)
        {
            GetComponent<EnemyGunsController>().enabled = false;
        }

        var direction = player.transform.position - transform.position;
        direction = direction.normalized;

        if (direction.x < 0)
        {
            transform.localScale = new Vector3(-1, 1, 1);
            armPosX = 0.15f;
        }
        else
        {
            transform.localScale = new Vector3(1, 1, 1);
            armPosX = 0;
        }
        if(enemy != null)
        {
            transform.position = new Vector3(enemy.transform.position.x - 0.1f + armPosX,
            enemy.transform.position.y + 0.05f,
            enemy.transform.position.z);
            var rotation = Quaternion.LookRotation(Vector3.forward, player.transform.position - transform.position);
            transform.localRotation = rotation;
        }        
    }
}
