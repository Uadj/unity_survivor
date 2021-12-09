using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Gun : MonoBehaviour
{
    public string gunName; //총이름
    public float range; //사정거리
    public float accuracy; //정확도
    public float fireRate; //연사속도
    public float reloadTime; //재장전 ㅗㄱ도
    public int damage;
    public int reloadBulletCount; //총알 재장전 갯수 탄알집 크기
    public int currentBulletCount; // 탄알집에 남아있는 갯수
    public int maxBulletCount; //최대 소지가능 탄알
    public int carryBulletCount; // 현재 소유 탄알
    public float retroActionForce; // 반동세기
    public float retroActionFineSightForce; //정조준 반반동세기
    public Vector3 fineSightOriginPos; // 정조준 전 원래 위치
    public Animator anim; //총 애니메이션
    public ParticleSystem muzzleFlash; //총구화염
    public AudioClip fire_Sound;
    public int GetcarrryBulletcount()
    {
        return carryBulletCount;
    }
    public int GetcurrentBulletcount()
    {
        return currentBulletCount;
    }
    public int GetreloadBulletCount()
    {
        return reloadBulletCount;
    }
    public int GetMaxBulletCount()
    {
        return maxBulletCount;
    }
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
