using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GunController : MonoBehaviour
{
    [SerializeField]
    private Gun currentGun;

    private float currentFireRate;

    void Update()
    {
        GunFireRateCalc();
    }

    private void GunFireRateCalc()
    {
        if (currentFireRate > 0)
        {
            currentFireRate -= Time.deltaTime;
            TryFire();
        }
    }
    private void TryFire()
    {
        if (Input.GetButton("Fire1") && currentFireRate <= 0)
        {
            Fire();
        }
    }
    private void Fire()
    {
        currentFireRate = currentGun.fireRate;
        Shoot();
    }
    private void Shoot()
    {
        Debug.Log("발사");
    }

    // Start is called before the first frame update
    void Start()
    {
        
    }

 
}
