using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class GrenadeLauncherController : MonoBehaviour
{
    public GameObject grenadePrefab;
    SpriteRenderer spriteRenderer;
    PlayerController playerController;
    GunsController gunsController;
    float grenadeSpeed;
    float fireRate = 0.8f;
    float time;
    AudioSource shootAudio;
    public AudioClip shoot;
    public AudioClip empty;
    // Start is called before the first frame update
    void Start()
    {
        grenadeSpeed = 20f;
        spriteRenderer = GetComponent<SpriteRenderer>();
        shootAudio = GetComponent<AudioSource>();
        gunsController = transform.parent.gameObject.GetComponent<GunsController>();
        playerController = gunsController.playerController;
        time = fireRate;
    }

    // Update is called once per frame
    void Update()
    {
        time += Time.deltaTime;
#if UNITY_EDITOR
        if (EventSystem.current.IsPointerOverGameObject())
        {
            return;
        }
#else 
    if (EventSystem.current.IsPointerOverGameObject(Input.GetTouch(0).fingerId)) {
        return;
    }
#endif
        if (!playerController.isShootEnabled)
        {
            if (time > fireRate * gunsController.playerRate)
            {
                if (Input.GetMouseButtonDown(0) && gunsController.grenadeLauncherAmmo > 0)
                {
                    Shoot();
                    time = 0;
                    gunsController.grenadeLauncherAmmo -= 1;
                }
                else if (Input.GetMouseButton(0) && gunsController.grenadeLauncherAmmo <= 0)
                {
                    shootAudio.PlayOneShot(empty);
                    time = 0;
                }
            }
        }
    }

    public void Shoot()
    {
        shootAudio.PlayOneShot(shoot);
        GameObject bullet = Instantiate(grenadePrefab, generateShootPosition(), Quaternion.identity) as GameObject;
        var pos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        Vector3 shotForward = Vector3.Scale((pos - transform.position), new Vector3(1, 1, 0)).normalized;
        bullet.GetComponent<Rigidbody2D>().velocity = shotForward * grenadeSpeed * gunsController.playerBulletSpeed;

    }

    public Vector3 generateShootPosition()
    {
        Vector3 vector = new Vector3(0.1f, 1f, 0f);
        Vector3 pos = spriteRenderer.transform.TransformPoint(vector);
        return pos;
    }
}
