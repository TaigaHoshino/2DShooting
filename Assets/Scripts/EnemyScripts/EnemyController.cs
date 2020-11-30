using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using Random = UnityEngine.Random;


public class EnemyController : MonoBehaviour
{
    GameObject parent;
    GameObject player;
    GameObject enemyGuns;
    GameObject enemyCanvas;
    GameObject enemyDrop;
    public GameDirector gameDirector;
    GunsController gunsController;
    Animator animator;
    AudioSource hitSound;
    public TextMeshProUGUI enemyDropText;
    float distance;
    bool isDown;
    bool playOnce;

    public void OnTriggerEnter2D(Collider2D collision)
    {
        if (!playOnce)
        {
            if (collision.gameObject.tag == "Bullet")
            {
                hitSound.Play();
                var direction = collision.transform.position - transform.position;
                direction = direction.normalized;
                GetComponent<Rigidbody2D>().constraints = RigidbodyConstraints2D.None;
                GetComponent<Rigidbody2D>().AddForce(-direction * 200);
                animator.SetTrigger("EnemyHitted");
                isDown = true;
            }
            else if (collision.gameObject.tag == "Rocket")
            {
                hitSound.Play();
                var direction = collision.transform.position - transform.position;
                direction = direction.normalized;
                GetComponent<Rigidbody2D>().constraints = RigidbodyConstraints2D.None;
                GetComponent<Rigidbody2D>().AddForce(-direction * 100);
                GetComponent<Rigidbody2D>().AddForce(transform.up * 500);
                animator.SetTrigger("EnemyHitted");
                isDown = true;
            }
            else if (collision.gameObject.tag == "Grenade")
            {
                hitSound.Play();
                var direction = collision.transform.position - transform.position;
                direction = direction.normalized;
                GetComponent<Rigidbody2D>().constraints = RigidbodyConstraints2D.None;
                GetComponent<Rigidbody2D>().AddForce(-direction * 100);
                GetComponent<Rigidbody2D>().AddForce(transform.up * 300);
                animator.SetTrigger("EnemyHitted");
                isDown = true;
            }
            else if (collision.gameObject.tag == "ExplosiveCan")
            {
                hitSound.Play();
                var direction = collision.transform.position - transform.position;
                direction = direction.normalized;
                GetComponent<Rigidbody2D>().constraints = RigidbodyConstraints2D.None;
                GetComponent<Rigidbody2D>().AddForce(-direction * 100);
                GetComponent<Rigidbody2D>().AddForce(transform.up * 700);
                animator.SetTrigger("EnemyHitted");
                isDown = true;
            }

            if (isDown)
            {
                gameDirector.GetScore(100);
                enemyGuns.SetActive(false);
                enemyCanvas.transform.position = transform.position;
                enemyCanvas.SetActive(true);
                playOnce = true;
                EnemyDrop();
                enemyDrop.GetComponent<Animation>().Play();
                Coroutine coroutine = StartCoroutine(DelayMethod(2.0f, () => {
                    Destroy(parent);
                }));
            }
        }
            
    }
    // Start is called before the first frame update
    private void Awake()
    {
        gameDirector = GameObject.FindWithTag("GameDirector").GetComponent<GameDirector>();
    }

    void Start()
    {
        player = GameObject.Find("Player");
        gunsController = GameObject.Find("Guns").GetComponent<GunsController>();
        isDown = false;
        playOnce = false;
        hitSound = GetComponent<AudioSource>();
        animator = GetComponent<Animator>();
        parent = transform.parent.gameObject;
        enemyGuns = parent.transform.GetChild(1).gameObject;
        enemyCanvas = parent.transform.GetChild(2).gameObject;
        enemyDrop = enemyCanvas.transform.GetChild(0).gameObject;
        enemyCanvas.SetActive(false);
        transform.rotation = Quaternion.Euler(0, 0, 0);
        GetComponent<Rigidbody2D>().constraints = RigidbodyConstraints2D.FreezeRotation;
        enemyGuns.SetActive(true);
    }

    // Update is called once per frame
    void Update()
    {
        if (gameDirector.gameOver)
        {
            GetComponent<EnemyController>().enabled = false;
        }

        if (!isDown)
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
    }

    public void EnemyDrop()
    {
        float dice = Random.Range(0, 100 * gameDirector.enemyDropRate);
        if (20 < dice && dice <= 40)
        {
            gunsController.assultRifleAmmo += 40;
            enemyDropText.text = "アサルトライフル\n弾薬+40";
            if (gunsController.assultRifleAmmo > gunsController.assultRifleMagazine)
            {
                gunsController.assultRifleAmmo = gunsController.assultRifleMagazine;
            }
        }
        else if (40 < dice && dice <= 60)
        {
            gunsController.grenadeLauncherAmmo += 5;
            enemyDropText.text = "グレネードランチャー\n弾薬+6";
            if (gunsController.grenadeLauncherAmmo > gunsController.grenadeLauncherMagazine)
            {
                gunsController.grenadeLauncherAmmo = gunsController.grenadeLauncherMagazine;
            }
        }
        else if (60 <= dice && dice < 80)
        {
            gunsController.rocketLauncherAmmo += 2;
            enemyDropText.text = "ロケットランチャー\n弾薬+3";
            if (gunsController.rocketLauncherAmmo > gunsController.rocketLauncherMagazine)
            {
                gunsController.rocketLauncherAmmo = gunsController.rocketLauncherMagazine;
            }
        }
        else if(80 <= dice && dice < 95)
        {
            enemyDropText.text = "プレイヤー体力+1";
            player.GetComponent<PlayerController>().PlayerHealing();
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
