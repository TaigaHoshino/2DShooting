using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class GunController : MonoBehaviour
{
    public GameObject bulletPrefab;
    SpriteRenderer spriteRenderer;
    PlayerController playerController;
    GunsController gunsController;
    float bulletSpeed;
    float fireRate = 0.3f;
    float time;
    public string magazine = "∞";
    AudioSource shootAudio;
    // Start is called before the first frame update
    void Start()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
        shootAudio = GetComponent<AudioSource>();
        gunsController = transform.parent.gameObject.GetComponent<GunsController>();
        playerController = gunsController.playerController;
        bulletSpeed = 13f;
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
                if (Input.GetMouseButton(0))
                {
                    Shoot();
                    time = 0;
                }
            }
        }
        
    }

    public void Shoot()
    {
        shootAudio.Play();
        GameObject bullet = Instantiate(bulletPrefab, generateShootPosition(), Quaternion.identity) as GameObject;
        var pos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        Vector3 shotForward = Vector3.Scale((pos - transform.position), new Vector3(1, 1, 0)).normalized;
        bullet.GetComponent<Rigidbody2D>().velocity = shotForward * bulletSpeed * gunsController.playerBulletSpeed;
    }

    public Vector3 generateShootPosition()
    {    
        Vector3 vector = new Vector3(-0.1f, 0.5f, 0f);
        Vector3 pos = spriteRenderer.transform.TransformPoint(vector);
        return pos;
    }


}
