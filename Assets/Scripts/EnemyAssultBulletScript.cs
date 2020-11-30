﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyAssultBulletScript : MonoBehaviour
{
    public GameObject bulletParticle;
    GameObject player;
    float distance;
    // Start is called before the first frame update
    void Start()
    {
        player = GameObject.Find("Player");
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.tag != "Enemy" &&
            collision.gameObject.tag != "BulletThroughFloor" &&
            collision.gameObject.tag != "Helicopter")
        {
            GameObject particle = Instantiate(bulletParticle) as GameObject;
            particle.transform.position = transform.position;
            particle.GetComponent<AudioSource>().Play();
            particle.GetComponent<ParticleSystem>().Play();
            Destroy(gameObject);
        }
    }

    // Update is called once per frame
    void Update()
    {
        distance = Vector3.Distance(player.transform.position, transform.position);
        if (distance > 20f)
        {
            Destroy(gameObject);
        }
    }
}