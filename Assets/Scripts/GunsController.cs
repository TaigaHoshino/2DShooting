using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using TMPro;

public class GunsController : MonoBehaviour
{
    GameObject player;
    public PlayerController playerController;
    GameObject[] guns;
    int gunNumber;
    float gunsPositionX;
    public TextMeshProUGUI magazineUI;
    public float gunsDashPositionX;
    Image gunsUI;
    public Sprite[] gunsImage;
    object[,] lists;
    public float playerBulletSpeed;
    public float playerRate;
    string handgunMagazine = "∞";
    public int assultRifleMagazine = 200;
    public int grenadeLauncherMagazine = 20;
    public int rocketLauncherMagazine = 8;
    public int assultRifleAmmo;
    public int grenadeLauncherAmmo;
    public int rocketLauncherAmmo;
    // Start is called before the first frame update
    private void Awake()
    {
        player = GameObject.Find("Player");
        playerController = player.GetComponent<PlayerController>();
        playerBulletSpeed = 1f;
        playerRate = 1f;
    }

    void Start()
    {       
        guns = new GameObject[]{transform.GetChild(0).gameObject,
            transform.GetChild(1).gameObject,
            transform.GetChild(2).gameObject,
            transform.GetChild(3).gameObject};
        gunsUI = GameObject.Find("GunsImage").GetComponent<Image>();

        int i;
        for (i = 0; i < guns.Length; i++)
        {
            guns[i].SetActive(false);
        }

        if(SceneManager.GetActiveScene().name == "TutorialScene")
        {
            assultRifleAmmo = assultRifleMagazine;
            grenadeLauncherAmmo = grenadeLauncherMagazine;
            rocketLauncherAmmo = rocketLauncherMagazine;
        }
        else
        {
            assultRifleAmmo = 50;
            grenadeLauncherAmmo = 6;
            rocketLauncherAmmo = 2;
        }
        guns[0].SetActive(true);
        gunsUI.sprite = gunsImage[0];
        gunsPositionX = 0;
        gunsDashPositionX = 0;

        lists = new object[,] {
            {handgunMagazine, handgunMagazine },
            {assultRifleMagazine, assultRifleAmmo },
            {grenadeLauncherMagazine, grenadeLauncherAmmo },
            {rocketLauncherMagazine, rocketLauncherAmmo }
        };
    }

    // Update is called once per frame
    void Update()
    {
        transform.position = new Vector3(player.transform.position.x - 0.1f
            + gunsPositionX + gunsDashPositionX,
            player.transform.position.y + 0.05f,
            player.transform.position.z);

        switch (gunNumber)
        {
            case 1:
                lists[1, 1] = assultRifleAmmo;
                break;
            case 2:
                lists[2, 1] = grenadeLauncherAmmo;
                break;
            case 3:
                lists[3, 1] = rocketLauncherAmmo;
                break;
        }

        magazineUI.text = lists[gunNumber, 1] + "/" + lists[gunNumber, 0];

        if (PlayerController.key != 0)
        {
            transform.localScale = new Vector3(PlayerController.key, 1, 1);
            if(PlayerController.key < 0)
            {
                gunsPositionX = 0.2f;
            }
            else
            {
                gunsPositionX = 0;
            }
        }
            var pos = Camera.main.WorldToScreenPoint(transform.localPosition);
            var rotation = Quaternion.LookRotation(Vector3.forward, Input.mousePosition - pos);
            transform.localRotation = rotation;
    }

    public void WeaponSelectLeftButton()
    {
        gunNumber--;
        if (gunNumber >= 0)
        {
            guns[gunNumber + 1].SetActive(false);
            guns[gunNumber].SetActive(true);
            gunsUI.sprite = gunsImage[gunNumber];
        }
        else
        {
            guns[gunNumber + 1].SetActive(false);
            gunNumber = guns.Length - 1;
            guns[gunNumber].SetActive(true);
            gunsUI.sprite = gunsImage[gunNumber];
        }
        playerController.isShootEnabled = true;
    }

    public void WeaponSelectLeftButtonUp()
    {
        playerController.isShootEnabled = false;
    }

    public void WeaponSelectRightButton()
    {
        gunNumber++;
        if(gunNumber < guns.Length)
        {
            guns[gunNumber - 1].SetActive(false);
            guns[gunNumber].SetActive(true);
            gunsUI.sprite = gunsImage[gunNumber];
        }
        else
        {
            guns[guns.Length - 1].SetActive(false);
            gunNumber = 0;
            guns[gunNumber].SetActive(true);
            gunsUI.sprite = gunsImage[gunNumber];
        }
        playerController.isShootEnabled = true;
    }

    public void PlayerMagazineSizeUp()
    {
        assultRifleMagazine += 50;
        grenadeLauncherMagazine += 5;
        rocketLauncherMagazine += 2;

        lists[1, 0] = assultRifleMagazine;
        lists[2, 0] = grenadeLauncherMagazine;
        lists[3, 0] = rocketLauncherMagazine;

    }

    public void WeaponSelectRightButtonUp()
    {
        playerController.isShootEnabled = false;
    }
}
