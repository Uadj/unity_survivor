using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Rock : MonoBehaviour
{
    [SerializeField]
    private int hp;
    [SerializeField]
    private float destroyTime;
    [SerializeField]
    private SphereCollider col;
    [SerializeField]
    private GameObject go_rock; // 일반 바위
    [SerializeField]
    private GameObject go_debris; // 깨진 바위
    
    public void mining() // 채굴

    {
        hp--;
        if (hp < 0)
        {
            Destruction();
        }
    }
    private void Destruction()
    {
        col.enabled = false;
        go_rock.SetActive(false);
        go_debris.SetActive(true);
        Destroy(go_debris, 2f);
    }
}
