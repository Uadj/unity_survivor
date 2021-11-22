using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharactorMove : MonoBehaviour
{

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
    private bool isRun = false;
    private bool isCrouch = false;
    private bool isGround = true;
    private float applyspeed;
    [SerializeField]
    private float JumpForce;
    private CapsuleCollider capsuleCollider;
    private GunController theGunController;
    // Start is called before the first frame update
    void Start()
    {
        myrigid = GetComponent<Rigidbody>();
        applyspeed = walkspeed;
        capsuleCollider = GetComponent<CapsuleCollider>();
        originPosY = theCamera.transform.localPosition.y;
        applycrouchPosy = originPosY;
        theGunController = FindObjectOfType<GunController>();
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
    }
    private void Move()
    {
        float _moveDirX = Input.GetAxis("Horizontal");
        float _moveDirZ = Input.GetAxis("Vertical");
        Vector3 _moveHorizontal = transform.right * _moveDirX;
        Vector3 _moveVertical = transform.forward * _moveDirZ;
        Vector3 Velocity = (_moveHorizontal + _moveVertical).normalized * applyspeed;
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
    }
    private void IsGround()
    {
        isGround = Physics.Raycast(transform.position, Vector3.down, capsuleCollider.bounds.extents.y+0.1f);
    }
    private void TryRun()
    {
        if (Input.GetKey(KeyCode.LeftShift))
        {
            Running();
        }
        if (Input.GetKeyUp(KeyCode.LeftShift))
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
        applyspeed = runspeed;

    }
    private void RunnignCancel()
    {
        isRun = false;
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
