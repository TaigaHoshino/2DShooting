using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerSampleScript : MonoBehaviour
{
    Animator animator;
    static int counter;
    // Start is called before the first frame update
    void Start()
    {
        animator = GetComponent<Animator>();
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKey(KeyCode.Space))
        {
            animator.SetBool("Run", true);
        }
        else
        {
            animator.SetBool("Run", false);
        }

    }
}
