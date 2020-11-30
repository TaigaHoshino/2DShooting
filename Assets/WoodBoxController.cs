using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WoodBoxController : MonoBehaviour
{
    GameObject player;
    public void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "Bullet" || collision.gameObject.tag == "EnemyBullet")
        {
            var direction = collision.transform.position - transform.position;
            direction = direction.normalized;
            GetComponent<Rigidbody2D>().constraints = RigidbodyConstraints2D.None;
            GetComponent<Rigidbody2D>().AddForce(-direction * 200);
        }
        else if (collision.gameObject.tag == "Rocket" || collision.gameObject.tag == "EnemyRocket")
        {
            var direction = collision.transform.position - transform.position;
            direction = direction.normalized;
            GetComponent<Rigidbody2D>().constraints = RigidbodyConstraints2D.None;
            GetComponent<Rigidbody2D>().AddForce(-direction * 100);
            GetComponent<Rigidbody2D>().AddForce(transform.up * 500);
        }
        else if (collision.gameObject.tag == "Grenade" || collision.gameObject.tag == "EnemyGrenade")
        {
            var direction = collision.transform.position - transform.position;
            direction = direction.normalized;
            GetComponent<Rigidbody2D>().constraints = RigidbodyConstraints2D.None;
            GetComponent<Rigidbody2D>().AddForce(-direction * 100);
            GetComponent<Rigidbody2D>().AddForce(transform.up * 300);
        }
        else if (collision.gameObject.tag == "ExplosiveCan")
        {
            var direction = collision.transform.position - transform.position;
            direction = direction.normalized;
            GetComponent<Rigidbody2D>().constraints = RigidbodyConstraints2D.None;
            GetComponent<Rigidbody2D>().AddForce(-direction * 100);
            GetComponent<Rigidbody2D>().AddForce(transform.up * 700);
        }
    }

    void Start()
    {
        player = GameObject.Find("Player");
    }

    void Update()
    {
        float distance = transform.position.x - player.transform.position.x;

        if (distance < -30f)
        {
            Destroy(gameObject);
        }
    }
}
