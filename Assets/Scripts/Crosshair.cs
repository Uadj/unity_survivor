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
    public void Fire()
    {
        animator.SetTrigger("Fire");
    }
    // Update is called once per frame
    void Update()
    {
        
    }
}
