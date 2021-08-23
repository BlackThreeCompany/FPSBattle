using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using Photon.Realtime;
public class PlayerMove : MonoBehaviourPunCallbacks, IPunObservable
{
    public PhotonView pv;

    public GameObject Cam;


    CharacterController cc;

    public PlayerGunShoot gunShoot;

    public float hmove;
    public float vmove;
    public Vector3 dirmove;
    public float moveSpeed;

    public float GravitySpeed;
    public float yspeed = 0;

    public float jumpinput;
    public float JumpSpeed;

    
    public GameObject Gun1;
    public GameObject GunArm;

    public GameObject CamPos;


    public Vector3 APos;
    public Vector3 BPos;
    public Quaternion ARot;
    public Quaternion BRot;

    public Vector3 curPos;
    public Vector3 curVelocity;
    public Quaternion curRot;
    public Quaternion curArmRot;

    public Vector3 syncPos;
    public Vector3 syncVelocity;
    public Quaternion syncRot;
    public Quaternion syncArmRot;

    public float lag;
    // Start is called before the first frame update
    void Awake()
    {
        cc = GetComponent<CharacterController>();
        gunShoot = GetComponent<PlayerGunShoot>();

        if (pv.IsMine)
        {
            Cam = Camera.main.gameObject;
            this.gameObject.tag = "MyPlayer";
            StatManager.instance.GetViewId(pv.ViewID);
        }
        else
        {
            this.gameObject.tag = "ElsePlayer";
        }
    }
    void Start()
    {
        moveSpeed = StatManager.instance.PlayerMoveSpeed;
        GravitySpeed = StatManager.instance.GravitySpeed;
        JumpSpeed = StatManager.instance.JumpSpeed;
    }
    // Update is called once per frame
    void Update()
    {
        if (!pv.IsMine)
        {
            transform.position = Vector3.Lerp(transform.position, syncPos + syncVelocity * lag, Time.deltaTime * 15);
            //transform.rotation = Quaternion.Lerp(transform.rotation, syncRot,Time.deltaTime * 10);
            transform.rotation = syncRot;
            //transform.position = syncPos + syncVelocity * lag;
            GunArm.transform.rotation = syncArmRot;
            return;
        }

        gunShoot.GunShootUpdate();

        APos = transform.position;
        ARot = transform.rotation;

        Move();
        Gravity();
        Jump();
        Rotate();

        CameraMove.instance.go();
        Rotate();

        BPos = transform.position;
        BRot = transform.rotation;

        curRot = transform.rotation;
        curArmRot = GunArm.transform.rotation;
        

        curVelocity = BPos - APos;
        curPos = transform.position;
    }


    void Gravity()
    {
        yspeed -= GravitySpeed * Time.deltaTime;
        if (cc.isGrounded)
        {
       
            yspeed = -GravitySpeed ;
        }
        dirmove = new Vector3(0, yspeed, 0);
        cc.Move(dirmove);
    }
    void Move()
    {   
        hmove = Input.GetAxis("Horizontal");
        vmove = Input.GetAxis("Vertical");
        dirmove = new Vector3(hmove * moveSpeed, -GravitySpeed, vmove * moveSpeed);

        dirmove = transform.TransformDirection(dirmove);

        cc.Move(dirmove *  Time.deltaTime);
        
    }
    void Jump()
    {
        jumpinput = Input.GetAxis("Jump");
        if(cc.isGrounded && jumpinput == 1)
        {
            yspeed = JumpSpeed ;
            dirmove = new Vector3(0, yspeed, 0);
            cc.Move(dirmove);
            
        }
    }
    void Rotate()
    {
        transform.rotation = Quaternion.Euler(transform.rotation.x, Cam.transform.rotation.eulerAngles.y, transform.rotation.z);
        GunArm.transform.localRotation = Quaternion.Euler(Cam.transform.rotation.eulerAngles.x, 0, 0);
        
    }

    public void OnPhotonSerializeView(PhotonStream stream, PhotonMessageInfo info)
    {
        if (stream.IsWriting)
        {
            stream.SendNext(curPos);
            stream.SendNext(curVelocity);
            stream.SendNext(curRot);
            stream.SendNext(curArmRot);
        }
        else
        {
            syncPos = (Vector3)stream.ReceiveNext();
            syncVelocity = (Vector3)stream.ReceiveNext();
            syncRot = (Quaternion)stream.ReceiveNext();
            syncArmRot = (Quaternion)stream.ReceiveNext();

            lag = Mathf.Abs((float)(PhotonNetwork.Time - info.SentServerTime)) * 150;
        }
    }
}
