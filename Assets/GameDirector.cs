using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using System;
using TMPro;
using Random = UnityEngine.Random;
using UnityEngine.EventSystems;

public class GameDirector : MonoBehaviour
{
    GameObject bgm;
    StageGenerator stageGenerator;
    AdmobGamePlay admobGamePlay;
    public bool gameOver;
    bool isSceneTutorial;
    PlayerController player;
    GameObject gameOverUI;
    GameObject gameOverImage;
    public bool isPause;
    GameObject buttons;
    GameObject scoreBoardUI;
    GameObject pauseUI;
    public Image pauseButton;
    public Sprite pauseImage;
    public Sprite playImage;
    Animation scoreBoardAnimation;
    GameObject bonusUI;
    GameObject levelupUI;
    public GameObject enemy;
    public GameObject heavyEnemy;
    Animation levelupAnimation;
    public TextMeshProUGUI scoreBoard;
    public TextMeshProUGUI bonusBoard;
    public TextMeshProUGUI levelupText;
    public　float score;
    public bool isScored;
    public float bonus;
    public float enemyRate = 1f;
    public float enemyBulletSpeed = 1f;
    public float enemyDropRate;
    float bonusTime;
    int levelupCounter;
    int level;
    int levelupCondition;
    int bonusCounter;
    

    bool playOnce;
    // Start is called before the first frame update
    private void Awake()
    {
        if(SceneManager.GetActiveScene().name == "GameScene")
        {
            admobGamePlay = GameObject.Find("Advertisement").GetComponent<AdmobGamePlay>();
        }
        bgm = GameObject.Find("GameBGM").transform.GetChild(0).gameObject;
        bgm.SetActive(false);
        isSceneTutorial = false;
        isPause = false;
        if (SceneManager.GetActiveScene().name == "TutorialScene")
        {
            isSceneTutorial = true;
        }
        else
        {
            scoreBoard.text = "0";
            score = 0;
            bonusCounter = 0;
            bonusTime = 0;
            bonus = 0;
            levelupCounter = 0;
            level = 0;
            levelupCondition = Random.Range(5, 9);
            enemyRate = 1f;
            enemyBulletSpeed = 1f;
            enemyDropRate = 1f;
        }
    }

    void Start()
    {
        player = GameObject.Find("Player").GetComponent<PlayerController>();
        gameOverUI = transform.GetChild(1).gameObject;
        gameOverImage = gameOverUI.transform.GetChild(0).gameObject;
        buttons = gameOverUI.transform.GetChild(1).gameObject;
        pauseUI = transform.GetChild(9).gameObject;
        pauseUI.SetActive(false);
        gameOverUI.SetActive(false);
        gameOver = false;
        playOnce = true;
        if (!isSceneTutorial)
        {
            stageGenerator = GameObject.Find("StageGenerator").GetComponent<StageGenerator>();
            scoreBoardUI = transform.GetChild(2).gameObject;
            scoreBoardAnimation = scoreBoardUI.GetComponent<Animation>();
            bonusUI = scoreBoardUI.transform.GetChild(1).gameObject;
            levelupUI = transform.GetChild(3).gameObject;
            levelupAnimation = levelupUI.GetComponent<Animation>();
            bonusUI.SetActive(false);
            levelupUI.SetActive(false);
        }
        
    }

    // Update is called once per frame
    void Update()
    {
        if (gameOver && playOnce)
        {
            gameOverUI.SetActive(true);
            buttons.SetActive(false);
            gameOverImage.GetComponent<Animation>().Play();
            Coroutine coroutine = StartCoroutine(DelayMethod(0.5f, () => {
                buttons.SetActive(true);
                buttons.GetComponent<Animation>().Play();
            }));

            if (!isSceneTutorial)
            {
                if(ScoreBoard.highScore < score)
                {
                    ScoreBoard.highScore = (int)score;
                    ScoreBoard.level = (int)level;
                }
            }

            playOnce = false;
        }
        if (!isSceneTutorial)
        {
            if (isScored)
            {
                levelupCounter++;
                bonusTime = 0;
                if (bonusTime < 3f)
                {
                    bonusCounter++;
                    if (bonusCounter == 1)
                    {
                        bonus = 1.2f;
                    }
                    else if (bonusCounter == 2)
                    {
                        bonusUI.SetActive(true);
                        bonus = 1.4f;
                        bonusBoard.text = "x2";
                    }
                    else if (bonusCounter == 3)
                    {
                        bonus = 1.6f;
                        bonusBoard.text = "x3";
                    }
                    else if (bonusCounter == 4)
                    {
                        bonus = 1.8f;
                        bonusBoard.text = "x4";
                    }
                    else if (bonusCounter > 4)
                    {
                        bonusBoard.text = "x5";
                    }
                }

                if (levelupCounter >= levelupCondition)
                {
                    int dice = Random.Range(1, 5);
                    levelupCondition = Random.Range(5, 9);
                    switch (dice)
                    {
                        case 1:
                            enemyRate *= 0.9f;
                            if (enemyRate < 0.1)
                            {
                                enemyRate = 0.1f;
                            }
                            levelupText.text = "敵武器レート上昇";
                            break;
                        case 2:
                            enemyBulletSpeed *= 1.1f;
                            levelupText.text = "敵弾丸速度上昇";
                            break;
                        case 3:
                            stageGenerator.enemyIncreaseRate *= 0.9f;
                            levelupText.text = "敵増加";
                            break;
                        case 4:
                            enemyDropRate *= 1.1f;
                            levelupText.text = "敵ドロップ率低下";
                            break;

                    }
                    levelupUI.SetActive(true);
                    levelupUI.GetComponent<AudioSource>().Play();
                    levelupAnimation.Play();
                    level++;
                    levelupCounter = 0;
                }

                scoreBoard.text = score.ToString();
                scoreBoardAnimation.Play();
                isScored = false;
            }

            if (!levelupAnimation.isPlaying)
            {
                levelupUI.SetActive(false);
            }

            if (bonusTime < 3f)
            {
                bonusTime += Time.deltaTime;
            }
            else if (bonusTime < 10f)
            {
                bonusUI.SetActive(false);
                bonusTime = 100f;
                bonusCounter = 0;
                bonus = 1;
            }
        }       

        
    }

    public void GetScore(float point)
    {
        score += point * bonus;
        isScored = true;
    }

    private IEnumerator DelayMethod(float waitTime, Action action)
    {
        yield return new WaitForSeconds(waitTime);
        action();
    }

    public void RetryButton()
    {
        Time.timeScale = 1f;
        
        if (isSceneTutorial)
        {
            SceneManager.LoadScene("TutorialScene");
        }
        else
        {
            admobGamePlay.StartAd();
            SceneManager.LoadScene("GameScene");
        }
    }

    public void MenuButton()
    {
        
        Time.timeScale = 1f;
        if (!isSceneTutorial)
        {
            admobGamePlay.StartAd();
        }
        bgm.SetActive(true);
        SceneManager.LoadScene("MenuScene");
    }

    public void PauseButton()
    {
        if (isPause)
        {
            bgm.SetActive(false);
            //player.isShootEnabled = false;
            Time.timeScale = 1f;
            pauseButton.sprite = pauseImage;
            pauseUI.SetActive(false);
            isPause = false;
        }
        else
        {
            bgm.SetActive(true);
            //player.isShootEnabled = false;
            Time.timeScale = 0f;
            pauseButton.sprite = playImage;
            pauseUI.SetActive(true);
            isPause = true;
        }
    }
}
