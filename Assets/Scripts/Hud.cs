using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class Hud : MonoBehaviour
{
    [SerializeField]
    private GunController theGunController;
    private Gun currentGun;

    //건 HUD
    [SerializeField]
    private GameObject go_BulletHUD;
    //탄알 개수
    [SerializeField]
    private Text[] text_Bullet;

    // Start is called before the first frame update

    // Update is called once per frame
    void Update()
    {
        CheckBullet();
    }
    private void CheckBullet()
    {
        theGunController.GetGun();
        text_Bullet[0].text = currentGun.carryBulletCount.ToString();
        text_Bullet[1].text = currentGun.reloadBulletCount.ToString();
        text_Bullet[2].text = currentGun.currentBulletCount.ToString();
    }
}
