using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GatlingGunController : MonoBehaviour
{
    GameObject enemy;
    GameObject player;
    GameDirector gameDirector;
    float armPosX;
    SpriteRenderer spriteRenderer;
    public GameObject bulletPrefab;
    public static float bulletSpeed;
    public static float fireRate;
    float distance;
    float time;
    AudioSource shootAudio;
    // Start is called before the first frame update
    void Start()
    {
        player = GameObject.Find("Player");
        enemy = transform.parent.gameObject;
        enemy = enemy.transform.GetChild(0).gameObject;
        gameDirector = enemy.GetComponent<HeavyEnemyBodyController>().gameDirector;
        shootAudio = GetComponent<AudioSource>();
        spriteRenderer = GetComponent<SpriteRenderer>();
        time = fireRate;
        bulletSpeed = 9f;
        fireRate = 0.25f;
    }

    // Update is called once per frame
    void Update()
    {
        if (gameDirector.gameOver)
        {
            GetComponent<GatlingGunController>().enabled = false;
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
        if (enemy != null)
        {
            transform.position = new Vector3(enemy.transform.position.x - 0.1f + armPosX,
            enemy.transform.position.y + 0.05f,
            enemy.transform.position.z);
            var rotation = Quaternion.LookRotation(new Vector3(0, 0.15f, 1), player.transform.position - transform.position);
            transform.localRotation = rotation;
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
        GameObject bullet = Instantiate(bulletPrefab, generateShootPosition(), Quaternion.identity) as GameObject;
        Vector3 shotForward = Vector3.Scale((player.transform.position - generateShootPosition()
            + new Vector3(Random.Range(-2, 3), Random.Range(-2, 3), 0)), new Vector3(1, 1, 0)).normalized;
        bullet.GetComponent<Rigidbody2D>().velocity = shotForward * bulletSpeed;

    }

    public Vector3 generateShootPosition()
    {
        Vector3 vector = new Vector3(0.7f, 1.5f, 0f);
        Vector3 pos = spriteRenderer.transform.TransformPoint(vector);
        return pos;
    }
}
