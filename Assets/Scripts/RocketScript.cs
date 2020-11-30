using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RocketScript : MonoBehaviour
{
    GameObject player;
    public GameObject bigExplosion;
    public GameObject rocketFire;
    GameObject fire;
    float rocketSpeed;
    float distance;
    bool playOnce;
    // Start is called before the first frame update
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.tag != "Player" && collision.gameObject.tag != "EnemyBullet"
            && !playOnce)
        {
            fire.GetComponent<ParticleSystem>().Stop();
            SpriteRenderer renderer = GetComponent<SpriteRenderer>();
            var color = renderer.color;
            color.a = 0;
            renderer.color = color;
            GetComponent<Rigidbody2D>().velocity = Vector3.zero;
            GameObject explosion = Instantiate(bigExplosion) as GameObject;
            explosion.transform.position = transform.position;
            GetComponent<CircleCollider2D>().enabled = true;
            explosion.GetComponent<ParticleSystem>().Play();
            explosion.GetComponent<AudioSource>().Play();
            playOnce = true;
            Coroutine coroutine = StartCoroutine(DelayMethod(0.1f, () => {
                Destroy(gameObject);
            }));
            //GetComponent<ParticleSystem>().Play();
        }

    }

    void Start()
    {
        player = GameObject.Find("Player");
        GetComponent<CircleCollider2D>().enabled = false;
        fire = Instantiate(rocketFire) as GameObject;
        fire.transform.position = transform.position;
        fire.GetComponent<ParticleSystem>().Play();
        rocketSpeed = 1f;
        playOnce = false;
    }

    // Update is called once per frame
    void Update()
    {
        distance = Vector3.Distance(player.transform.position, transform.position);
        if (distance > 20f)
        {
            Destroy(gameObject);
        }
        rocketSpeed *= 1 + 0.1f*Time.deltaTime;
        if(rocketSpeed < 1.15f)
        {
            GetComponent<Rigidbody2D>().velocity = GetComponent<Rigidbody2D>().velocity * rocketSpeed;
        }
        fire.transform.position = transform.position;
    }

    private IEnumerator DelayMethod(float waitTime, Action action)
    {
        yield return new WaitForSeconds(waitTime);
        action();
    }
}
