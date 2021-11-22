using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GunController : MonoBehaviour
{
    // 현재 장착된 총
    [SerializeField]
    private Gun currentGun;
    //효과음
    private AudioSource audiosource;
    //연사속도
    private float currentFireRate;
    //상태변수
    private bool isReload = false;
    [HideInInspector]
    public bool isFineSightMode = false;
    [SerializeField]
    private Vector3 originPos;
    //피격 충돌 정보
    private RaycastHit hitinfo;
    //피격 이펙트
    [SerializeField]
    private GameObject hit_effect_prefab;
    [SerializeField]
    private Camera theCam;
    void Update()
    {
        GunFireRateCalc();
        TryFire();
        TryReload();
        TryFineSight();
    }
    //연사속도 계산
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
        if (!isReload)
        {
            if (currentGun.currentBulletCount > 0)
            {
                Shoot();
            }
            else
            {
                CancelFineSight();
                StartCoroutine(ReloadCoroutine());
            }
        }
    }
    //발사 후 계산
    private void Shoot()
    {
        currentGun.currentBulletCount--;
        currentFireRate = currentGun.fireRate;
        Debug.Log("발사");
        Hit(); 
        currentGun.muzzleFlash.Play();
        PlaySe(currentGun.fire_Sound);
        StopAllCoroutines();
        StartCoroutine(RetroActionCoroutine());
    }
    private void Hit()
    {
        if(Physics.Raycast(theCam.transform.position, theCam.transform.forward, out hitinfo, currentGun.range))
        {
            Debug.Log(hitinfo.transform.name);
            GameObject clone = Instantiate(hit_effect_prefab, hitinfo.point, Quaternion.LookRotation(hitinfo.normal));
            Destroy(clone, 1f);
        }
    }
    private void TryReload()
    {
        if (!isReload && Input.GetKeyDown(KeyCode.R) && currentGun.currentBulletCount < currentGun.reloadBulletCount)
        {
            CancelFineSight();
            StartCoroutine(ReloadCoroutine());
        }
        
    }
    //재장전
    IEnumerator ReloadCoroutine()
    {
        if (currentGun.carryBulletCount > 0)
        {
            isReload = true;
            currentGun.anim.SetTrigger("Reload");
            currentGun.carryBulletCount += currentGun.currentBulletCount;
            currentGun.currentBulletCount = 0;
            yield return new WaitForSeconds(currentGun.reloadTime);
            if (currentGun.carryBulletCount > currentGun.reloadBulletCount)
            {
                currentGun.currentBulletCount = currentGun.reloadBulletCount;
                currentGun.carryBulletCount -= currentGun.reloadBulletCount;

            }
            else
            {
                currentGun.currentBulletCount = currentGun.carryBulletCount;
                currentGun.carryBulletCount = 0;
            }
        }
        isReload = false;
    }
    //정조준 시도
    private void TryFineSight()
        {
            if (!isReload&&Input.GetButtonDown("Fire2"))
            {
                FineSight();
            }
        }
    //정조준 취소
    public void CancelFineSight()
    {
        if (isFineSightMode)
        {
            FineSight();
        }
    }
    //정조준 로직 가동
        private void FineSight()
        {
            isFineSightMode = !isFineSightMode;
            currentGun.anim.SetBool("FineSightMode", isFineSightMode);
            if (isFineSightMode)
            {
                StopAllCoroutines();
                StartCoroutine(FineSightActivateCoroutine());
            }
            else
            {
                StopAllCoroutines();
                StartCoroutine(FineSightDeactivateCoroutine());
            }
        }
    //정조준 코루틴
        IEnumerator FineSightActivateCoroutine()
        {
            while (currentGun.transform.localPosition != currentGun.fineSightOriginPos)
            {
                currentGun.transform.localPosition = Vector3.Lerp(currentGun.transform.localPosition, currentGun.fineSightOriginPos, 0.2f);
                yield return null;
            }
        }
    //정조준 취소 코루틴
        IEnumerator FineSightDeactivateCoroutine()
        {
            while (currentGun.transform.localPosition != originPos)
            {
                currentGun.transform.localPosition = Vector3.Lerp(currentGun.transform.localPosition, originPos, 0.2f);
                yield return null;
            }
        }
    //반동 코루틴
        IEnumerator RetroActionCoroutine()
        {
            Vector3 recoilBack = new Vector3(currentGun.retroActionForce, originPos.y, originPos.z);
            Vector3 retroActionRecoilBack = new Vector3(currentGun.retroActionFineSightForce, currentGun.fineSightOriginPos.y, currentGun.fineSightOriginPos.z);
            if (!isFineSightMode)
            {
                currentGun.transform.localPosition = originPos;
                //반동 시작
                while (currentGun.transform.localPosition.x <= currentGun.retroActionForce - 0.02f)
                {
                    currentGun.transform.localPosition = Vector3.Lerp(currentGun.transform.localPosition, recoilBack, 0.4f);
                    yield return null;
                }
                //원위치
                while (currentGun.transform.localPosition != originPos)
                {
                    currentGun.transform.localPosition = Vector3.Lerp(currentGun.transform.localPosition, originPos, 0.1f);
                    yield return null;
                }
            }
            else
            {
                currentGun.transform.localPosition = currentGun.fineSightOriginPos;
                //반동 시작
                while (currentGun.transform.localPosition.x <= currentGun.retroActionForce - 0.02f)
                {
                    currentGun.transform.localPosition = Vector3.Lerp(currentGun.transform.localPosition, retroActionRecoilBack, 0.4f);
                    yield return null;
                }
                //원위치
                while (currentGun.transform.localPosition != originPos)
                {
                    currentGun.transform.localPosition = Vector3.Lerp(currentGun.transform.localPosition, currentGun.fineSightOriginPos, 0.1f);
                    yield return null;
                }
            }
        }
        // Start is called before the first frame update
        void Start()
        {
            audiosource = GetComponent<AudioSource>();
            originPos = Vector3.zero;

        }
        private void PlaySe(AudioClip _clip)
        {
            audiosource.clip = _clip;
            audiosource.Play();
        }

    }
