using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyAssultRifleController : MonoBehaviour
{
    public GameObject bulletPrefab;
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
        bulletSpeed = 9f * gameDirector.enemyBulletSpeed;
        fireRate = 0.6f * gameDirector.enemyRate;
    }

    // Update is called once per frame
    void Update()
    {
        if (gameDirector.gameOver)
        {
            GetComponent<EnemyAssultRifleController>().enabled = false;
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
        Vector3 shotForward = Vector3.Scale((player.transform.position - transform.position), new Vector3(1, 1, 0)).normalized;
        bullet.GetComponent<Rigidbody2D>().velocity = shotForward * bulletSpeed;
    }

    public Vector3 generateShootPosition()
    {
        Vector3 vector = new Vector3(0.05f, 0.05f, 0f);
        Vector3 pos = spriteRenderer.transform.TransformPoint(vector);
        return pos;
    }
}
