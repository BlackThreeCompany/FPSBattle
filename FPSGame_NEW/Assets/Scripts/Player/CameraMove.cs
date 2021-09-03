using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraMove : MonoBehaviour
{
    public GameObject PlayerCamPos;
    public GameObject Player;
    public float MouseX;
    public float MouseY;
    public float Rot_X;
    public float Rot_y;
    public float AimSpeed;

    
    public float DownTime;
    public float DownSpeed = 5;
    Vector3 MainCameraVector;

    public static CameraMove instance;

    // Start is called before the first frame update

    void Awake()
    {
        instance = this;
    }
    void Start()
    {
        AimSpeed = StatManager.instance.AimSpeed;
    }

    // Update is called once per frame
    void Update()
    {

        if (NetWorkManager.instance.isJoined)
        {
            if(Player == null)
            {
                Player = GameObject.FindGameObjectWithTag("MyPlayer");
                PlayerCamPos = Player.transform.Find("CamPos").gameObject;
                
            }
            else
            {
                CamRecoil();
            }
        }
    }
    public void go()
    {
        if(!Inventory.inventoryActivated)
        {
            Rotate();
        }
        ToPlayer();

    }
    void ToPlayer()
    {
        transform.position = PlayerCamPos.transform.position;
       
        
    }
    void Rotate()
    {
        MouseX = Input.GetAxis("Mouse X");
        MouseY = Input.GetAxis("Mouse Y");




        Rot_X = transform.rotation.eulerAngles.x + MouseY * -AimSpeed;
        Rot_y = transform.rotation.eulerAngles.y + MouseX * AimSpeed;

        
        if (Rot_X > 54f && Rot_X <= 180f)
        {
            Rot_X = 54f;
        }
        else if (Rot_X < 290f && Rot_X > 180f)
        {
            Rot_X = 290f;
        }
        
        transform.rotation = Quaternion.Euler(Rot_X, Rot_y, transform.rotation.eulerAngles.z);
    }

    public void AddRotation(float x, float y, float z)
    {
        float RotX = transform.rotation.eulerAngles.x - x; 
        float RotY = transform.rotation.eulerAngles.y + y;
        float RotZ = transform.rotation.eulerAngles.z + z;

        //MainCameraVector = new Vector3(RotX, RotY, RotZ);
        

        transform.rotation = Quaternion.Euler(RotX, RotY, RotZ);
    }

    public void CamRecoil()
    {

        
            if (DownTime > 0)
        {
            transform.rotation = Quaternion.Euler(transform.rotation.eulerAngles.x + (DownSpeed * Time.deltaTime), transform.rotation.eulerAngles.y, transform.rotation.eulerAngles.z);
            DownTime -= Time.deltaTime;
        }
        
    }
}   
