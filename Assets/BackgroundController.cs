using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BackgroundController : MonoBehaviour
{
    GameObject[] backgrounds;
    float mainCameraPosX;
    float mainCameraPosY;
    float distanceX;
    float distanceY;
    float mainCameraPastPosX;
    float mainCameraPastPosY;
    float background2Interval;
    float background3Interval;
    float background4Interval;
    // Start is called before the first frame update
    void Start()
    {
        backgrounds = new GameObject[]{transform.GetChild(0).gameObject,
            transform.GetChild(1).gameObject,
            transform.GetChild(2).gameObject,
            transform.GetChild(3).gameObject};
        mainCameraPastPosX = transform.position.x;
        mainCameraPastPosY = transform.position.y;
    }

    // Update is called once per frame
    void Update()
    {
        mainCameraPosX = transform.position.x;
        mainCameraPosY = transform.position.y;
        distanceX = mainCameraPosX - mainCameraPastPosX;
        distanceY = mainCameraPosY - mainCameraPastPosY;
        mainCameraPastPosX = mainCameraPosX;
        mainCameraPastPosY = mainCameraPosY;
        backgrounds[0].transform.position = new Vector3(mainCameraPosX,
            mainCameraPosY);
        backgrounds[1].transform.Translate(-distanceX * 0.6f, -distanceY * 0.03f, 0);
        backgrounds[2].transform.Translate(-distanceX, -distanceY * 0.1f, 0);
        backgrounds[3].transform.Translate(-distanceX * 1.3f, -distanceY * 0.2f, 0);

        background2Interval += -distanceX * 0.6f;
        if (background2Interval <= -8.9f)
        {
            background2Interval = 8.9f;
            backgrounds[1].transform.Translate(17.8f, 0, 0);
        }
        else if(background2Interval >= 8.9f)
        {
            background2Interval = -8.9f;
            backgrounds[1].transform.Translate(-17.8f, 0, 0);
        }

        background3Interval += -distanceX;
        if (background3Interval <= -8.9f)
        {
            background3Interval = 8.9f;
            backgrounds[2].transform.Translate(17.8f, 0, 0);
        }
        else if (background3Interval >= 8.9f)
        {
            background3Interval = -8.9f;
            backgrounds[2].transform.Translate(-17.8f, 0, 0);
        }

        background4Interval += -distanceX * 1.3f;
        if (background4Interval <= -8.9f)
        {
            background4Interval = 8.9f;
            backgrounds[3].transform.Translate(17.8f, 0, 0);
        }
        else if (background4Interval >= 8.9f)
        {
            background4Interval = -8.9f;
            backgrounds[3].transform.Translate(-17.8f, 0, 0);
        }




    }
}
