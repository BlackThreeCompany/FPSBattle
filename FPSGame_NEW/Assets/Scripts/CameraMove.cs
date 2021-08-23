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
        }
    }
    public void go()
    {
        ToPlayer();
        Rotate();
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

        
        if (Rot_X > 70f && Rot_X <= 180f)
        {
            Rot_X = 70f;
        }
        else if (Rot_X < 290f && Rot_X > 180f)
        {
            Rot_X = 290f;
        }
        
        transform.rotation = Quaternion.Euler(Rot_X, Rot_y, transform.rotation.eulerAngles.z);
    }
}   
