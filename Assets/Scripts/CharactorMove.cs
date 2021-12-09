using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharactorMove : MonoBehaviour
{
    Status theStatus;
    private float crouchPosY;
    private float originPosY;
    private float applycrouchPosy;
    [SerializeField]
    private float crouchSpeed;
    [SerializeField]
    private float walkspeed;
    [SerializeField]
    private float runspeed;
    [SerializeField]
    private float lookSensitivity;
    [SerializeField]
    private float cameraRotationLimit;
    private float currentCameraRotationX;
    private Rigidbody myrigid;
    [SerializeField]
    private Camera theCamera;
    private bool isWalk = false;
    public bool isRun = false;
    private bool isCrouch = false;
    private bool isGround = true;
    //움직임 체크변수
    private Vector3 lastPos;

    private float applyspeed;
    [SerializeField]
    private float JumpForce;
    private CapsuleCollider capsuleCollider;
    private GunController theGunController;
    [SerializeField]
    private Gun theGun;
    [SerializeField]
    private Hand theHand;
    private Crosshair theCrosshair;
    // Start is called before the first frame update
    void Start()
    {
       // theGun = theGunController.GetGun();
        myrigid = GetComponent<Rigidbody>();
        applyspeed = walkspeed;
        capsuleCollider = GetComponent<CapsuleCollider>();
        originPosY = theCamera.transform.localPosition.y;
        applycrouchPosy = originPosY;
        theGunController = FindObjectOfType<GunController>();
        theCrosshair = FindObjectOfType<Crosshair>();
        theStatus = FindObjectOfType<Status>();
    }

    // Update is called once per frame
    void Update()
    {
        IsGround();
        CameraRotation();
        TryRun();
        TryCrouch();
        TryJump();
        CharactorRotation();
        Move();
       // MoveCheck();
    }
    private void MoveCheck()
    {
        if (!isRun)
        {
            if (Vector3.Distance(lastPos, transform.position)>=0.01f)
            {
                isWalk = true;
           
            }
            else
            {
                isWalk = false;
            }
            theCrosshair.WalkingAnimation(isWalk);
            if(GunController.isActivate) theGun.anim.SetBool("Walk",isWalk);
            if(HandController.isActivate) theHand.anim.SetBool("Walk", isWalk);
            lastPos = transform.position;
        }
    }
    private void Move()
    {
        float _moveDirX = Input.GetAxis("Horizontal");
        float _moveDirZ = Input.GetAxis("Vertical");
        Vector3 _moveHorizontal = transform.right * _moveDirX;
        Vector3 _moveVertical = transform.forward * _moveDirZ;
        Vector3 Velocity = (_moveHorizontal + _moveVertical).normalized * applyspeed;
        if (Velocity.x + Velocity.z >= 0.1f) isWalk = true;
        else isWalk = false;
                
        theCrosshair.WalkingAnimation(isWalk);
        if (GunController.isActivate) theGun.anim.SetBool("Walk", isWalk);
        if (HandController.isActivate) theHand.anim.SetBool("Walk", isWalk);
        myrigid.MovePosition(transform.position + Velocity * Time.deltaTime);
    }
    private void TryCrouch()
    {
        if (Input.GetKeyDown(KeyCode.LeftControl))
        {
            Crouch();
        }
    }
    private void TryJump()
    {
        if (Input.GetKeyDown(KeyCode.Space)&&isGround)
        {
            Jump();
        }
    }
    private void Crouch()
    {
        isCrouch = !isCrouch;
        theCrosshair.CrouchAnimation(isCrouch);
        if (isCrouch)
        {
            applyspeed = crouchSpeed;
            applycrouchPosy = crouchPosY;
        }
        else
        {
            applyspeed = walkspeed;
            applycrouchPosy = originPosY;
        }
        //  theCamera.transform.localPosition = new Vector3(theCamera.transform.localPosition.x, applycrouchPosy, theCamera.transform.localPosition.z);
        StartCoroutine(CrouchCoroutine());
    }
    IEnumerator CrouchCoroutine()
    {
        float _posY = theCamera.transform.localPosition.y;
        int count = 0;
        while (_posY != applycrouchPosy)
        {
            count++;
            _posY = Mathf.Lerp(_posY, applycrouchPosy, 0.3f);
            theCamera.transform.localPosition = new Vector3(0, _posY, 0);
            yield return null;
            if (count > 15) break;
        }
        theCamera.transform.localPosition = new Vector3(0, applycrouchPosy, 0f);
    }
    private void Jump()
    {
        if (isCrouch)
        {
            Crouch();
        }
        myrigid.velocity = transform.up * JumpForce;
        theStatus.DecreaseStamina(30);
    }
    private void IsGround()
    {
        isGround = Physics.Raycast(transform.position, Vector3.down, capsuleCollider.bounds.extents.y+0.1f);
    }
    private void TryRun()
    {
        if (Input.GetKey(KeyCode.LeftShift)&&isGround&&theStatus.GetCurrentSP()>0)
        {
            Running();
            theStatus.DecreaseStamina(2);
            
        }
        if (Input.GetKeyUp(KeyCode.LeftShift)||theStatus.GetCurrentSP()<=0)
        {
            RunnignCancel();
        }
    }
    private void Running()
    {
        if (isCrouch)
        {
            Crouch();
        }
        theGunController.CancelFineSight();
        isRun = true;
        theCrosshair.RunningAnimation(isRun);
        if (GunController.isActivate) theGun.anim.SetBool("Run", isRun);
        if (HandController.isActivate) theHand.anim.SetBool("Run", isRun);
        applyspeed = runspeed;

    }
    private void RunnignCancel()
    {
        isRun = false;
        theCrosshair.RunningAnimation(isRun);
        if (GunController.isActivate) theGun.anim.SetBool("Run", isRun);
        if (HandController.isActivate) theHand.anim.SetBool("Run", isRun);
        applyspeed = walkspeed;
    }
    private void CameraRotation()
    {
        float _xRotation = Input.GetAxis("Mouse Y");
        float _cameraRotationX = _xRotation * lookSensitivity;
        currentCameraRotationX -= _cameraRotationX;
        currentCameraRotationX = Mathf.Clamp(currentCameraRotationX, -cameraRotationLimit, cameraRotationLimit);
        theCamera.transform.localEulerAngles = new Vector3(currentCameraRotationX, 0f, 0f);
        

    }
    private void CharactorRotation()
    {
        float _yRotation = Input.GetAxis("Mouse X");
        Vector3 _characterRotationY = new Vector3(0f, _yRotation, 0f) * lookSensitivity;
        myrigid.MoveRotation(myrigid.rotation * Quaternion.Euler(_characterRotationY));
    }
}
