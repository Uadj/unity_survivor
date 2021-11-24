using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Crosshair : MonoBehaviour
{
    [SerializeField]
    private Animator animator;
    private float gunAccuracy;
    [SerializeField]
    private GameObject go_CrosshairHUD;

    public void WalkingAnimation(bool _flag)
    {
        animator.SetBool("Walk", _flag);
    }
    public void RunningAnimation(bool _flag)
    {
        animator.SetBool("Run", _flag);
    }
    public void CrouchAnimation(bool _flag)
    {
        animator.SetBool("Crouch", _flag);
    }
    public float GetAccuracy()
    {
        if (animator.GetBool("Walk"))
        {
            gunAccuracy = 0.08f;
        }
        else if (animator.GetBool("Crouch"))
        {
            gunAccuracy = 0.05f;
        }
        else
        {
            gunAccuracy = 0.03f;
        }
        return gunAccuracy;
    }
    public void Fire()
    {
        animator.SetTrigger("Fire");
    }
    // Update is called once per frame
    void Update()
    {
        
    }
}
