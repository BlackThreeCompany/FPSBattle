using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMove : MonoBehaviour
{
    public float lookSensitivity;
    float currentRotationX;
    public int cameraRotationLimit;

    float moveSpeed = 20.0f;
    public float jumpForce = 10.0f;

    bool isGround = false;

    public Vector3 moveDir = Vector3.zero;

    public Camera theCamera;

    Rigidbody rbody;
    CapsuleCollider capsuleCollider;
    private void Awake()
    {
        rbody = GetComponent<Rigidbody>();
        capsuleCollider = GetComponent<CapsuleCollider>();
    }

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        CheckGround();
        MoveStart();
        CameraRotation();
        CharacterRotation();
        Jump();
    }

    void MoveStart()
    {
        float _moveDirX = Input.GetAxis("Horizontal");
        float _moveDirZ = Input.GetAxis("Vertical");

        Vector3 _moveHorizontal = transform.right * _moveDirX;
        Vector3 _moveVertical = transform.forward * _moveDirZ;

        Vector3 _velocity = (_moveHorizontal + _moveVertical).normalized * moveSpeed;

        rbody.MovePosition(transform.position + _velocity * Time.deltaTime);
    }

    void CharacterRotation()
    {
        float y = Input.GetAxisRaw("Mouse X");
        Vector3 rotationY = new Vector3(0f, y, 0f) * lookSensitivity;
        rbody.MoveRotation(rbody.rotation * Quaternion.Euler(rotationY));
    }

    void CameraRotation()
    {
        float x = Input.GetAxisRaw("Mouse Y");
        float rotationX = x * lookSensitivity;
        currentRotationX -= rotationX;
        currentRotationX = Mathf.Clamp(currentRotationX, -cameraRotationLimit, cameraRotationLimit);

        theCamera.transform.localEulerAngles = new Vector3(currentRotationX, 0f, 0f);
    }

    void CheckGround()
    {
        RaycastHit hit;
        Debug.DrawRay(transform.position, Vector3.down * 1f, Color.red);

        if(Physics.Raycast(transform.position, Vector3.down,out hit, 1f))
        {
            if(hit.transform.tag == "Ground")
            {
                isGround = true;
                return;
            }
        }
        isGround = false;
    }

    void Jump()
    {
        if (isGround)
        {
            if (Input.GetButtonDown("Jump"))
            {
                rbody.velocity = transform.up * jumpForce;
            }
        }
    }
}
