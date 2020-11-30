using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyOnHelicopterController : MonoBehaviour
{
    GameObject player;
    GameObject enemyGuns;
    GameDirector gameDirector;
    Animator animator;
    float distance;
    bool isDown;
    // Start is called before the first frame update
    void Start()
    {
        player = GameObject.Find("Player");
        gameDirector = GameObject.FindWithTag("GameDirector").GetComponent<GameDirector>();
        isDown = false;
        animator = GetComponent<Animator>();
        GameObject parent = transform.parent.gameObject;
        enemyGuns = parent.transform.GetChild(1).gameObject;
        transform.rotation = Quaternion.Euler(0, 0, 0);
        GetComponent<Rigidbody2D>().constraints = RigidbodyConstraints2D.FreezeRotation;
        enemyGuns.SetActive(true);
    }

    // Update is called once per frame
    void Update()
    {
        if (gameDirector.gameOver)
        {
            GetComponent<EnemyOnHelicopterController>().enabled = false;
        }

        if (!isDown)
        {
            var direction = player.transform.position - transform.position;
            direction = direction.normalized;

            if (direction.x < 0)
            {
                transform.localScale = new Vector3(-1, 1, 1);
                //enemyGuns.transform.localScale = new Vector3(-1, 1, 1);
            }
            else
            {
                transform.localScale = new Vector3(1, 1, 1);
                //enemyGuns.transform.localScale = new Vector3(1, 1, 1);
            }
        }
        else
        {
            enemyGuns.SetActive(false);
            GetComponent<Rigidbody2D>().constraints = RigidbodyConstraints2D.None;
            GetComponent<Rigidbody2D>().gravityScale = 1;
            animator.SetTrigger("EnemyHitted");
            Coroutine coroutine = StartCoroutine(DelayMethod(2.0f, () => {
                Destroy(gameObject);
            }));
        }

        distance = transform.position.x - player.transform.position.x;
    }

    private IEnumerator DelayMethod(float waitTime, Action action)
    {
        yield return new WaitForSeconds(waitTime);
        action();
    }

    public void HelicopterDown()
    {
        isDown = true;
    }
}
