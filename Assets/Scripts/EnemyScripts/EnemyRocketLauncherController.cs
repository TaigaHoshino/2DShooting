using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyRocketLauncherController : MonoBehaviour
{
    public GameObject rocketPrefab;
    GameObject player;
    GameDirector gameDirector;
    SpriteRenderer spriteRenderer;
    float bulletSpeed;
    float fireRate;
    float distance;
    float time;
    AudioSource shootAudio;
    // Start is called before the first frame update
    void Start()
    {
        player = GameObject.Find("Player");
        shootAudio = GetComponent<AudioSource>();
        spriteRenderer = GetComponent<SpriteRenderer>();
        GameObject enemyBody = transform.parent.gameObject;
        enemyBody = enemyBody.transform.parent.gameObject;
        gameDirector = enemyBody.transform.GetChild(0).gameObject
            .GetComponent<EnemyController>().gameDirector;
        bulletSpeed = 11f;
        fireRate = 1.6f * gameDirector.enemyRate;
        time = fireRate;
    }

    // Update is called once per frame
    void Update()
    {
        if (gameDirector.gameOver)
        {
            GetComponent<EnemyRocketLauncherController>().enabled = false;
        }
        time += Time.deltaTime;
        if (time > fireRate)
        {
            distance = Vector3.Distance(player.transform.position, transform.position);
            if (distance < 15)
            {
                Shoot();
                time = 0;
            }
        }
    }

    public void Shoot()
    {
        shootAudio.Play();
        GameObject bullet = Instantiate(rocketPrefab, generateShootPosition(), Quaternion.identity) as GameObject;
        bullet.transform.rotation = transform.rotation;
        Vector3 shotForward = Vector3.Scale((player.transform.position - transform.position), new Vector3(1, 1, 0)).normalized;
        bullet.GetComponent<Rigidbody2D>().velocity = shotForward * bulletSpeed;
    }

    public Vector3 generateShootPosition()
    {
        Vector3 vector = new Vector3(-0.15f, 0.2f, 0f);
        Vector3 pos = spriteRenderer.transform.TransformPoint(vector);
        return pos;
    }
}
