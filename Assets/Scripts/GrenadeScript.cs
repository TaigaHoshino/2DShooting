using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GrenadeScript : MonoBehaviour
{
    GameObject player;
    public GameObject smallExplosion;
    float distance;
    bool playOnce;
    // Start is called before the first frame update
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.tag != "Player" && !playOnce)
        {
            SpriteRenderer renderer = GetComponent<SpriteRenderer>();
            TrailRenderer TLrenderer = GetComponent<TrailRenderer>();
            var color = renderer.color;
            color.a = 0;
            renderer.color = color;
            TLrenderer.enabled = false;
            GetComponent<Rigidbody2D>().velocity = Vector3.zero;
            GameObject explosion = Instantiate(smallExplosion) as GameObject;
            explosion.transform.position = transform.position;
            GetComponent<CircleCollider2D>().enabled = true;
            explosion.GetComponent<AudioSource>().Play();
            explosion.GetComponent<ParticleSystem>().Play();
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
    }

    private IEnumerator DelayMethod(float waitTime, Action action)
    {
        yield return new WaitForSeconds(waitTime);
        action();
    }
}
