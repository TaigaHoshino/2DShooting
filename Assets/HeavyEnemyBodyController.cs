using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using TMPro;
using Random = UnityEngine.Random;

public class HeavyEnemyBodyController : MonoBehaviour
{
    GameObject player;
    GameObject gatlingGun;
    GameObject parent;
    GameObject enemyCanvas;
    GameObject enemyDrop;
    PlayerController playerController;
    public GameDirector gameDirector;
    GunsController gunsController;
    Animator animator;
    AudioSource hitSound;
    public TextMeshProUGUI enemyDropText;
    float distance;
    bool playOnce;
    int heavyEnemyHp;
    bool isEnemyDamaged;
    float time;
    public void OnTriggerEnter2D(Collider2D collision)
    {
        if (!playOnce)
        {
            if (collision.gameObject.tag == "Bullet")
            {
                hitSound.Play();
                var direction = collision.transform.position - transform.position;
                direction = direction.normalized;
                GetComponent<Rigidbody2D>().AddForce(-direction * 200);
                isEnemyDamaged = true;
                heavyEnemyHp -= 15;
            }
            else if (collision.gameObject.tag == "Rocket")
            {
                hitSound.Play();
                var direction = collision.transform.position - transform.position;
                direction = direction.normalized;                
                GetComponent<Rigidbody2D>().AddForce(-direction * 100);
                GetComponent<Rigidbody2D>().AddForce(transform.up * 500);
                isEnemyDamaged = true;
                heavyEnemyHp -= 100;
            }
            else if (collision.gameObject.tag == "Grenade")
            {
                hitSound.Play();
                var direction = collision.transform.position - transform.position;
                direction = direction.normalized;
                GetComponent<Rigidbody2D>().AddForce(-direction * 100);
                GetComponent<Rigidbody2D>().AddForce(transform.up * 300);
                isEnemyDamaged = true;
                heavyEnemyHp -= 50;
            }
            else if (collision.gameObject.tag == "ExplosiveCan")
            {
                hitSound.Play();
                var direction = collision.transform.position - transform.position;
                direction = direction.normalized;
                //GetComponent<Rigidbody2D>().AddForce(-direction * 100);
                //GetComponent<Rigidbody2D>().AddForce(transform.up * 700);
                isEnemyDamaged = true;

                heavyEnemyHp -= 100;
            }

            if (heavyEnemyHp <= 0)
            {
                gameDirector.GetScore(200);
                GetComponent<Rigidbody2D>().constraints = RigidbodyConstraints2D.None;
                animator.SetTrigger("EnemyHitted");
                gatlingGun.SetActive(false);
                enemyCanvas.transform.position = transform.position;
                enemyCanvas.SetActive(true);
                EnemyDrop();
                playOnce = true;
                enemyDrop.GetComponent<Animation>().Play();
                Coroutine coroutine = StartCoroutine(DelayMethod(2.0f, () => {
                    Destroy(parent);
                }));
            }
        }

    }

    private void Awake()
    {
        gameDirector = GameObject.FindWithTag("GameDirector").GetComponent<GameDirector>();
    }
    // Start is called before the first frame update
    void Start()
    {
        player = GameObject.Find("Player");
        playerController = player.GetComponent<PlayerController>();
        gunsController = GameObject.Find("Guns").GetComponent<GunsController>();
        playOnce = false;
        isEnemyDamaged = false;
        heavyEnemyHp = 100;
        hitSound = GetComponent<AudioSource>();
        animator = GetComponent<Animator>();
        parent = transform.parent.gameObject;
        gatlingGun = parent.transform.GetChild(1).gameObject;
        enemyCanvas = parent.transform.GetChild(2).gameObject;
        enemyDrop = enemyCanvas.transform.GetChild(0).gameObject;
        enemyCanvas.SetActive(false);
        transform.rotation = Quaternion.Euler(0, 0, 0);
        GetComponent<Rigidbody2D>().constraints = RigidbodyConstraints2D.FreezeRotation;
        gatlingGun.SetActive(true);
    }

    // Update is called once per frame
    void Update()
    {
        if (gameDirector.gameOver)
        {
            GetComponent<HeavyEnemyBodyController>().enabled = false;
        }

        if (heavyEnemyHp > 0)
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

            distance = transform.position.x - player.transform.position.x;

            if (distance < -30f)
            {
                Destroy(parent);
            }
        }

        if (isEnemyDamaged)
        {
            time += Time.deltaTime;
            SpriteRenderer renderer = GetComponent<SpriteRenderer>();
            var color = renderer.color;
            color.a = Mathf.Sin(Time.time * 50f) / 2 + 0.5f;
            renderer.color = color;
            if (time > 0.5f)
            {
                isEnemyDamaged = false;
                time = 0;
                color = renderer.color;
                color.a = 1;
                renderer.color = color;
            }
        }
    }

    public void EnemyDrop()
    {
        float dice = Random.Range(0, 100 * gameDirector.enemyDropRate);
        if (20 < dice && dice <= 70)
        {
            enemyDropText.text = "プレイヤー体力+1";
            playerController.PlayerHealing();
        }
        else if (70 < dice && dice <= 80)
        {
            gunsController.playerRate *= 0.9f;
            enemyDropText.text = "プレイヤー武器レートUP";
        }
        else if (80 <= dice && dice < 90)
        {
            gunsController.PlayerMagazineSizeUp();
            enemyDropText.text = "プレイヤー弾薬所持量UP";
        }
        else if(90 <= dice && dice < 100)
        {
            enemyDropText.text = "プレイヤー弾丸速度UP";
            gunsController.playerBulletSpeed *= 0.9f;
        }
        else
        {
            enemyDropText.text = "";
        }
    }

    private IEnumerator DelayMethod(float waitTime, Action action)
    {
        yield return new WaitForSeconds(waitTime);
        action();
    }
}
