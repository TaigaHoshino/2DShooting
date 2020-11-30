using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using TMPro;
using Random = UnityEngine.Random;

public class HelicopterController : MonoBehaviour
{
    GameObject player;
    public GameObject superBigExplosion;
    GameObject parent;
    GameObject enemyCanvas;
    GameObject enemyDrop;
    GameDirector gameDirector;
    GunsController gunsController;
    public TextMeshProUGUI enemyDropText;
    float playerPosX;
    float playerPosY;
    float playerPosDistanceX;
    float pastPlayerPosX;
    float playerPosDistanceY;
    float pastPlayerPosY;
    float time;
    float damagedTime;
    float randomX;
    float randomY;
    float helicopterSpeed;
    bool isHelicopterDamaged;
    bool playOnce;
    int helicopterHp;
    AudioSource hitSound;
    AudioSource helicopterSound;

    public void OnTriggerEnter2D(Collider2D collision)
    {
        if (!playOnce)
        {
            if (collision.gameObject.tag == "Bullet")
            {
                isHelicopterDamaged = true;
                helicopterHp -= 10;
                hitSound.Play();
            }
            else if (collision.gameObject.tag == "Rocket")
            {
                isHelicopterDamaged = true;
                helicopterHp -= 100;
                hitSound.Play();
            }
            else if (collision.gameObject.tag == "Grenade")
            {
                isHelicopterDamaged = true;
                helicopterHp -= 50;
                hitSound.Play();
            }
            else if (collision.gameObject.tag == "ExplosiveCan")
            {
                isHelicopterDamaged = true;
                helicopterHp -= 100;
                hitSound.Play();
            }

            if (helicopterHp <= 0)
            {
                gameDirector.GetScore(200);
                helicopterSound.Stop();
                GetComponent<Rigidbody2D>().gravityScale = 1;
                GameObject explosion = Instantiate(superBigExplosion) as GameObject;
                explosion.transform.position = transform.position;
                explosion.GetComponent<CircleCollider2D>().enabled = false;
                explosion.GetComponent<AudioSource>().Play();
                explosion.GetComponent<ParticleSystem>().Play();
                enemyCanvas.transform.position = transform.position;
                enemyCanvas.SetActive(true);
                EnemyDrop();
                enemyDrop.GetComponent<Animation>().Play();
                GameObject enemyOnHelicopter = transform.GetChild(2).gameObject;
                enemyOnHelicopter = enemyOnHelicopter.transform.GetChild(0).gameObject;
                enemyOnHelicopter.GetComponent<EnemyOnHelicopterController>().HelicopterDown();
                playOnce = true;
                Coroutine coroutine = StartCoroutine(DelayMethod(3.0f, () => {
                    Destroy(parent);
                }));
            }
        }
    }

    // Start is called before the first frame update
    void Start()
    {
        player = GameObject.Find("Player");
        gunsController = GameObject.Find("Guns").GetComponent<GunsController>();
        gameDirector = GameObject.FindWithTag("GameDirector").GetComponent<GameDirector>();
        AudioSource[] audios = GetComponents<AudioSource>();
        hitSound = audios[0];
        helicopterSound = audios[1];
        parent = transform.parent.gameObject;
        enemyCanvas = parent.transform.GetChild(1).gameObject;
        enemyDrop = enemyCanvas.transform.GetChild(0).gameObject;
        enemyCanvas.SetActive(false);
        playerPosDistanceX = player.transform.position.x;
        pastPlayerPosX = player.transform.position.x;
        playerPosDistanceY = player.transform.position.y;
        pastPlayerPosY = player.transform.position.y;
        helicopterHp = 100;
        helicopterSpeed = 10f;
        randomX = Random.Range(-5, 6);
        randomY = -5;
        time = 0;
        damagedTime = 0;
        playOnce = false;
    }

    // Update is called once per frame
    void Update()
    {
        if (gameDirector.gameOver)
        {
            GetComponent<HelicopterController>().enabled = false;
        }

        playerPosX = player.transform.position.x;
        playerPosDistanceX = playerPosX - pastPlayerPosX;
        pastPlayerPosX = playerPosX;

        playerPosY = player.transform.position.y;
        playerPosDistanceY = playerPosY - pastPlayerPosY;
        pastPlayerPosY = playerPosY;
        time += Time.deltaTime;
        if(time < 2)
        {
            helicopterSpeed *= 0.95f;
            transform.Translate(playerPosDistanceX + helicopterSpeed * randomX * 0.008f,
                playerPosDistanceY * 0.8f + helicopterSpeed * randomY * 0.004f,
                0);
        }
        else
        {
            helicopterSpeed = 10f;
            if (playerPosX + 3f < transform.position.x  && transform.position.x < playerPosX + 5f)
            {
                randomX = Random.Range(-3, 4);    
            }
            else if(transform.position.x<= playerPosX + 3f)
            {
                randomX = Random.Range(1, 4);
                
            }
            else
            {
                randomX = Random.Range(-3, 0); 
            }

            if(playerPosY + 5.5f < transform.position.y && transform.position.y < playerPosY + 7f)
            {
                randomY = Random.Range(-3, 4);
            }
            else if(transform.position.y <= playerPosY + 5.5f)
            {
                randomY = Random.Range(1, 4);
            }
            else
            {
                randomY = Random.Range(-3, 0);
            }
            time = 0;
        }

        if (isHelicopterDamaged)
        {
            damagedTime += Time.deltaTime;
            SpriteRenderer renderer = GetComponent<SpriteRenderer>();
            var color = renderer.color;
            color.a = Mathf.Sin(Time.time * 70f) / 2 + 0.5f;
            renderer.color = color;
            if (damagedTime > 0.7f)
            {
                isHelicopterDamaged = false;
                damagedTime = 0;
                color = renderer.color;
                color.a = 1;
                renderer.color = color;
            }
        }

        float distance = transform.position.x - player.transform.position.x;

        if (distance < -30f)
        {
            Destroy(parent);
        }
    }

    public void EnemyDrop()
    {
        float dice = Random.Range(0, 100 * gameDirector.enemyDropRate);
        if (30 < dice && dice <= 70)
        {
            enemyDropText.text = "プレイヤー体力+1";
            player.GetComponent<PlayerController>().PlayerHealing();
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
        else if (90 <= dice && dice < 100)
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
