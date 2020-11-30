using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HelicopterWingController : MonoBehaviour
{
    float wingSpeed = 50;

    // Update is called once per frame
    void Update()
    {
        transform.Rotate(0, wingSpeed, 0);
    }
}
