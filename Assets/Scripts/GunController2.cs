using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GunController : MonoBehaviour
{
    [SerializeField]
    private Gun currentGun;
    private AudioSource audiosource;
    private float currentFireRate;
    private bool isReload = false;
    private bool isfineSightMode = false;
    [SerializeField]
    private Vector3 originPos;
    void Update()
    {
        GunFireRateCalc();
        TryFire();
        TryReload();
        TryFineSight();
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
        if(!isReload)
        {
            if(currentGun.currentBulletCount>0)
            {
                Shoot();
            }
            else
            {
                startCoroutine(ReloadCoroutine());
            }
        }
    }
    
    private void Shoot()
    {
        currentGun.currentBulletCount--;
        currentFireRate = currentGun.fireRate;
        Debug.Log("발사");
        currentGun.muzzleFlash.Play();
        PlaySe(currentGun.fire_Sound);
        StopAllCoroutine():
        StartCoroutine(RetroActionCoroutine());
    }
    private void TryReload()
    {
        if(!isReload && input.GetKeyDown(Keycode.R) && currentGun.currentBulletCount<currentGun.reloadBulletCount)
        {
            startCoroutine(ReloadCoroutine());
        }
    IEnumerator ReloadCoroutine()
    {
        if(currentGun.carryBulletCount >0)
        {
            isReload=true;
            currentGun.anim.SetTrigger("Reload");
            currentGun.carryBulletCount+=currentBulletCount;
            currentGun.currentBulletCount=0;
            yield return new WaitForSeconds(currentGun.reloadTime);
            if(currentGun.carryBulletCount>currentGun.reloadBulletCount)
            {
                currentGun.currentBulletCount = currentGun.reloadBulletCount;
                currentGun.carryBulletCount-=currentGun.reloadBulletCount;
                
            }
            else
            {
                currentGun.currentBullectCount=currentGun.carryBulletCount;
                currentGun.carryBulletCount=0;
            }
        }
        isReload = false;
    }
    private void TryFineSight()
    {
        if(Input.GetButtonDown("Fire2"))
        {
            FineSight();
        }
    }
    private void FineSight()
    {
        isFineSightMode = !isFineSightMode;
        currentGun.anim.SetBool("FineSightMode", isFineSightMode);
        if(isFineSightMode)
        {
            StopAllCoroutine();
            StartCoroutine(FineSightActivateCoroutine());
        }
        else
        {
            StopAllCoroutine();
            StartCoroutine(FineSightDeactivateCoroutine());
        }
    }
    IEnumerator FineSightActivateCoroutine()
    {
        while(currentGun.transform.localPosition != currentGun.fineSightOriginPos)
        {
            currentGun.transform.localPosition = Vector3.Lerp(currentGun.transform.localPosition, currentGun.finesSightOriginPos,0.2f);
            yield return null;
        }
    }
    IEnumerator FineSightDeactivateCoroutine()
    {
        while(currentGun.transform.localPosition != OriginPos)
        {
            currentGun.transform.localPosition = Vector3.Lerp(currentGun.transform.localPosition, OriginPos,0.2f);
            yield return null;
        }
    }
    IEnumerator RetroActionCoroutine()
    {
        Vector3 recoilBack = new Vector3(currentGun.retroActionForce,originPos.y,originPos.z);
        Vector3 retroActionRecoilBack = new Vector3(currentGun.retroActionFineSightForce, currentGun.fineSightModeForce.y,currentGun.fineSightModeForce.z);
        if(!isFineSightMode)
        {
            currentGun.transform.localPosition = originPos;
            //반동 시작
            while(currentGun.transform.localPosition.x <= currentGun.retroActionForce-0.02f)
            {
                currentGun.transform.localPosition = Vector3.Lerp(currentGun.transform.localPosition, recoilBack,0.4f);
                yield return null;
            }
            //원위치
            while(currentGun.trnasform.localPosition != originPos)
            {
                currentGun.transform.localPosition = Vector3.Lerp(currentGun.transform.localPosition,originPos,0.1f);
                yield return null;
            }
        }
        else
        {
            currentGun.transform.localPosition =  currentGun.fineSightOriginPos;
            //반동 시작
            while(currentGun.transform.localPosition.x <= currentGun.retroActionForce-0.02f)
            {
                currentGun.transform.localPosition = Vector3.Lerp(currentGun.transform.localPosition, retroActionRecoilBack,0.4f);
                yield return null;
            }
            //원위치
            while(currentGun.trnasform.localPosition != originPos)
            {
                currentGun.transform.localPosition = Vector3.Lerp(currentGun.transform.localPosition,currentGun.fineSightOriginPos,0.1f);
                yield return null;
            }
        }
    }
    // Start is called before the first frame update
    void Start()
    {
        audiosource = GetComponent<AudioSource>();

    }
    private void PlaySe(AudioClip _clip)
    {
        audiosource.clip = _clip;
        audiosource.Play();
    }
 
}
