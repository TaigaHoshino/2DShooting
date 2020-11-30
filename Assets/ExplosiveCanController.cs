using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ExplosiveCanController : MonoBehaviour
{
    int canHp;
    bool playOnce;
    public GameObject superBigExplosion;
    bool isCanDamaged;
    float time;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (!playOnce)
        {
            if (collision.gameObject.tag == "Bullet")
            {
                canHp -= 25;
                var direction = collision.transform.position - transform.position;
                direction = direction.normalized;
                GetComponent<Rigidbody2D>().AddForce(-direction * 200);
                isCanDamaged = true;
            }
            else if (collision.gameObject.tag == "Rocket")
            {
                canHp -= 100;
            }
            else if (collision.gameObject.tag == "Grenade")
            {
                canHp -= 100;
            }

            if (canHp <= 0)
            {
                SpriteRenderer renderer = GetComponent<SpriteRenderer>();
                var color = renderer.color;
                color.a = 0;
                renderer.color = color;
                GameObject explosion = Instantiate(superBigExplosion) as GameObject;
                explosion.transform.position = transform.position;
                explosion.GetComponent<CircleCollider2D>().enabled = true;
                explosion.GetComponent<AudioSource>().Play();
                explosion.GetComponent<ParticleSystem>().Play();
                playOnce = true;
                Coroutine coroutine = StartCoroutine(DelayMethod(0.1f, () => {
                    explosion.GetComponent<CircleCollider2D>().enabled = false;
                    Destroy(gameObject);
                }));
            }
        }
        
    }

    // Start is called before the first frame update
    void Start()
    {
        canHp = 100;
        isCanDamaged = false;
    }

    // Update is called once per frame
    void Update()
    {
        if (isCanDamaged)
        {
            time += Time.deltaTime;
            SpriteRenderer renderer = GetComponent<SpriteRenderer>();
            var color = renderer.color;
            color.a = Mathf.Sin(Time.time * 50f) / 2 + 0.5f;
            renderer.color = color;
            if (time > 0.5f)
            {
                isCanDamaged = false;
                time = 0;
                color = renderer.color;
                color.a = 1;
                renderer.color = color;
            }
        }
    }

    private IEnumerator DelayMethod(float waitTime, Action action)
    {
        yield return new WaitForSeconds(waitTime);
        action();
    }
}
