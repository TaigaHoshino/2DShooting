using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BGMController : MonoBehaviour
{
    private static bool isCreated = false;
    public Image BGMButtonImg;
    public Sprite playSound;
    public Sprite stopSound;
    AudioSource bgmAudio;
    bool isBGMPlaying = true;
    // Start is called before the first frame update
    void Start()
    {
        bgmAudio = GetComponent<AudioSource>();
        if (!isCreated)
        {
            DontDestroyOnLoad(gameObject);
            isCreated = true;
        }
        else
        {
            Destroy(gameObject);
        }
    }

    public void BGMButton()
    {
        if (isBGMPlaying)
        {
            BGMButtonImg.sprite = stopSound;
            bgmAudio.Stop();
            isBGMPlaying = false;
        }
        else
        {
            BGMButtonImg.sprite = playSound;
            bgmAudio.Play();
            isBGMPlaying = true;
        }
    }
}
