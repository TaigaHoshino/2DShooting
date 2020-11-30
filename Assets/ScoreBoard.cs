using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class ScoreBoard : MonoBehaviour
{
    public static int highScore;
    public static int level;
    public TextMeshProUGUI scoreText;
    // Start is called before the first frame update
    void Start()
    {
        gameObject.SetActive(true);
        scoreText.text = "現在のハイスコア\n" + highScore.ToString()
            + "\n到達したレベル\n" + level.ToString() + ".Lv";
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
