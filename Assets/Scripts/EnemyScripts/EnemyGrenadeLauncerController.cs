using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyGrenadeLauncerController : MonoBehaviour
{
    public GameObject grenadePrefab;
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
        spriteRenderer = GetComponent<SpriteRenderer>();
        shootAudio = GetComponent<AudioSource>();
        GameObject enemyBody = transform.parent.gameObject;
        enemyBody = enemyBody.transform.parent.gameObject;
        gameDirector = enemyBody.transform.GetChild(0).gameObject
            .GetComponent<EnemyController>().gameDirector;
        bulletSpeed = 11f * gameDirector.enemyBulletSpeed;
        fireRate = 1.3f * gameDirector.enemyRate;
    }

    void Update()
    {
        if (gameDirector.gameOver)
        {
            GetComponent<EnemyGrenadeLauncerController>().enabled = false;
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
        GameObject bullet = Instantiate(grenadePrefab, generateShootPosition(), Quaternion.identity) as GameObject;
        Vector3 shotForward = Vector3.Scale((player.transform.position - transform.position), new Vector3(1, 1, 0)).normalized;
        bullet.GetComponent<Rigidbody2D>().velocity = shotForward * bulletSpeed;

    }

    public Vector3 generateShootPosition()
    {
        Vector3 vector = new Vector3(0.1f, 1f, 0f);
        Vector3 pos = spriteRenderer.transform.TransformPoint(vector);
        return pos;
    }
}
