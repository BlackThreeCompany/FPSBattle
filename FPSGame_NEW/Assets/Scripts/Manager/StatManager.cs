using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class StatManager : MonoBehaviour
{
    public float PlayerMoveSpeed;
    public float AimSpeed;
    public float GravitySpeed;
    public float JumpSpeed;
    public float BulletSpeed;
    public int MyViewId;
    public int HP = 100000;

    public Text MyViewIdTx;
    public static StatManager instance;

    void Awake()
    {
        instance = this;
    }
    public void GetViewId(int ViewId)
    {
        MyViewId = ViewId;
        MyViewIdTx.text = "ViewID : " + MyViewId;
        
    }
}
