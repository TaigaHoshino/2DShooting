using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class AssultRifleController : MonoBehaviour
{
    // Start is called before the first frame update
    public GameObject bulletPrefab;
    PlayerController playerController;
    GunsController gunsController;
    SpriteRenderer spriteRenderer;
    float bulletSpeed;
    float fireRate = 0.10f;
    float time;
    AudioSource shootAudio;
    public AudioClip shoot;
    public AudioClip empty;
    // Start is called before the first frame update
    void Start()
    {
        bulletSpeed = 13f;
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
                if (Input.GetMouseButton(0) && gunsController.assultRifleAmmo > 0)
                {
                    Shoot();
                    time = 0;
                    gunsController.assultRifleAmmo -= 1;
                }
                else if(Input.GetMouseButton(0) && gunsController.assultRifleAmmo <= 0)
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
        GameObject bullet = Instantiate(bulletPrefab, generateShootPosition(), Quaternion.identity) as GameObject;
        var pos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        Vector3 shotForward = Vector3.Scale((pos - transform.position), new Vector3(1, 1, 0)).normalized;
        bullet.GetComponent<Rigidbody2D>().velocity = shotForward * bulletSpeed * gunsController.playerBulletSpeed;
    }

    public Vector3 generateShootPosition()
    {
        Vector3 vector = new Vector3(0.05f, 0.05f, 0f);
        Vector3 pos = spriteRenderer.transform.TransformPoint(vector);
        return pos;
    }
}
