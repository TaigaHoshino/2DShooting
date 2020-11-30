using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using System;
using UnityEngine.UI;

public class PlayerController : MonoBehaviour
{
    GameObject playerGuns;
    GameDirector gameDirector;
    Rigidbody2D rigid2D;
    Animator animator;
    AudioSource hitAudio;
    GunsController gunsController;
    public int numOfHearts;
    int maxHealth;
    public Image[] hearts;
    public static int key;
    float jumpForce = 350;
    float walkForce = 50;
    float maxWalkSpeed = 5;
    float threshold = 0.2f;
    float time;
    public bool isShootEnabled = false;
    bool isJumpButtonPushed = false;
    bool isPlayerDamaged;
    bool isPlayerDown;
    int doubleJump;

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if(collision.gameObject.tag == "Floor" || collision.gameObject.tag == "BulletThroughFloor")
        {
            doubleJump = 0;
            animator.SetBool("isJumping", false);
            isShootEnabled = false;
        }
    }
    public void OnTriggerEnter2D(Collider2D collision)
    {
        if (!isPlayerDamaged && !isPlayerDown)
        {
            if (collision.gameObject.tag == "EnemyBullet")
            {
                var direction = collision.transform.position - transform.position;
                direction = direction.normalized;
                //GetComponent<Rigidbody2D>().constraints = RigidbodyConstraints2D.None;
                GetComponent<Rigidbody2D>().AddForce(-direction * 200);
                hitAudio.Play();
                isPlayerDamaged = true;
                hearts[numOfHearts--].enabled = false;
                
                //isDown = true;
            }
            else if (collision.gameObject.tag == "EnemyRocket")
            {
                var direction = collision.transform.position - transform.position;
                direction = direction.normalized;
                //GetComponent<Rigidbody2D>().constraints = RigidbodyConstraints2D.None;
                GetComponent<Rigidbody2D>().AddForce(-direction * 100);
                GetComponent<Rigidbody2D>().AddForce(transform.up * 500);
                hitAudio.Play();
                isPlayerDamaged = true;
                hearts[numOfHearts--].enabled = false;
                //isDown = true;
            }
            else if (collision.gameObject.tag == "EnemyGrenade")
            {
                var direction = collision.transform.position - transform.position;
                direction = direction.normalized;
                //GetComponent<Rigidbody2D>().constraints = RigidbodyConstraints2D.None;
                GetComponent<Rigidbody2D>().AddForce(-direction * 100);
                GetComponent<Rigidbody2D>().AddForce(transform.up * 500);
                hitAudio.Play();
                isPlayerDamaged = true;
                hearts[numOfHearts--].enabled = false;
                //isDown = true;
            }
            else if (collision.gameObject.tag == "ExplosiveCan")
            {
                var direction = collision.transform.position - transform.position;
                direction = direction.normalized;
                //GetComponent<Rigidbody2D>().constraints = RigidbodyConstraints2D.None;
                GetComponent<Rigidbody2D>().AddForce(-direction * 100);
                GetComponent<Rigidbody2D>().AddForce(transform.up * 700);
                hitAudio.Play();
                isPlayerDamaged = true;
                hearts[numOfHearts--].enabled = false;
                //isDown = true;
            }

            if(numOfHearts == -1)
            {
                isPlayerDown = true;
                GetComponent<Rigidbody2D>().constraints = RigidbodyConstraints2D.None;
                animator.SetTrigger("isPlayerBeated");
                playerGuns.SetActive(false);
                gameDirector.gameOver = true;
                GetComponent<PlayerController>().enabled = false;
                Coroutine coroutine = StartCoroutine(DelayMethod(5f, () => {
                    Destroy(playerGuns);
                    Destroy(gameObject);
                }));
            }
        }

        if (collision.gameObject.tag == "Hole")
        {
            isPlayerDown = true;
            GetComponent<Rigidbody2D>().constraints = RigidbodyConstraints2D.None;
            animator.SetTrigger("isPlayerBeated");
            playerGuns.SetActive(false);
            gameDirector.gameOver = true;
            //GetComponent<PlayerController>().enabled = false;
            Coroutine coroutine = StartCoroutine(DelayMethod(5f, () => {
                Destroy(playerGuns);
                Destroy(gameObject);
            }));
        }
    }

    // Start is called before the first frame update
    void Start()
    {
        playerGuns = GameObject.Find("Guns");
        gameDirector = GameObject.FindWithTag("GameDirector").GetComponent<GameDirector>();
        rigid2D = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();
        hitAudio = GetComponent<AudioSource>();
        gunsController = GameObject.Find("Guns").GetComponent<GunsController>();
        doubleJump = 0;
        numOfHearts = -1;
        maxHealth = -1;
        isPlayerDown = false;
        isPlayerDamaged = false;
        for(int i = 0; i < hearts.Length; i++)
        {
            maxHealth++;
        }
        numOfHearts = 3;
        hearts[4].enabled = false;
        hearts[5].enabled = false;
    }

    // Update is called once per frame
    void Update()
    {
        //Debug.Log(animator.GetBool(1));

        key = 0;
        float speedx = rigid2D.velocity.x;
        animator.SetBool("isRunning", false);
        gunsController.gunsDashPositionX = 0;

//#if UNITY_EDITOR
        if (!gameDirector.isPause)
        {
            if (Input.GetKeyDown(KeyCode.W))
            {
                animator.SetBool("isJumping", true);
                if (doubleJump < 2)
                {
                    this.rigid2D.AddForce(transform.up * jumpForce);
                    doubleJump++;
                }

            }

            if (Input.GetKey(KeyCode.D))
            {
                animator.SetBool("isRunning", true);
                gunsController.gunsDashPositionX = 0.1f;
                if (speedx < maxWalkSpeed)
                {
                    key = 1;
                }
            }

            if (Input.GetKey(KeyCode.A))
            {
                animator.SetBool("isRunning", true);
                gunsController.gunsDashPositionX = -0.1f;
                if (-maxWalkSpeed < speedx)
                {
                    key = -1;
                }
            }
        }

//#else
        if (!gameDirector.isPause)
        {
            if (isJumpButtonPushed)
            {
                animator.SetBool("isJumping", true);
                if (doubleJump < 2)
                {
                    this.rigid2D.AddForce(transform.up * jumpForce);
                    doubleJump++;
                }

            }

            isJumpButtonPushed = false;

            if (Input.acceleration.x > threshold)
            {
                animator.SetBool("isRunning", true);
                gunsController.gunsDashPositionX = 0.1f;
                if (speedx < maxWalkSpeed)
                {
                    key = 1;
                }
            }

            if (Input.acceleration.x < -threshold)
            {
               animator.SetBool("isRunning", true);
               gunsController.gunsDashPositionX = -0.1f;
                if (-maxWalkSpeed < speedx)
                {
                    key = -1;
                }
            }
        }
        
//#endif

        if (key != 0)
        {
            transform.localScale = new Vector3(key, 1, 1);
        }

        this.rigid2D.AddForce(transform.right * key * this.walkForce);

        if (isPlayerDamaged)
        {
            time += Time.deltaTime;
            SpriteRenderer renderer = GetComponent<SpriteRenderer>();
            var color = renderer.color;
            color.a = Mathf.Sin(Time.time * 50f) / 2 + 0.5f;
            renderer.color = color;
            if (time > 2.0f)
            {
                isPlayerDamaged = false;
                time = 0;
                color = renderer.color;
                color.a = 1;
                renderer.color = color;
            }

        }
    }

    public void PlayerHealing()
    {      
        if(maxHealth - 1 >= numOfHearts && !isPlayerDown)
        {
            numOfHearts++;
            hearts[numOfHearts].enabled = true;
        }
    }

    public void JumpButtonDown()
    {
        isJumpButtonPushed = true;
        isShootEnabled = true;
    }

    public void JumpButtonUp()
    {
        isShootEnabled = false;
    }

    private IEnumerator DelayMethod(float waitTime, Action action)
    {
        yield return new WaitForSeconds(waitTime);
        action();
    }
}
