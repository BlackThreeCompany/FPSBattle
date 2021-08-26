using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeaponManager : MonoBehaviour
{
    public Equipment WeaponSloat;
    public Equipment PistolSloat;

    public bool isCanFireAR = false;
    public bool isCanFirePistol = false;
    public bool isCanThrowGrenade = false;
    public bool isCanThrowSmokeGrenade = false;

    public int GrenadeCnt;
    public int Smoke_GrenadeCnt;

    public float FireSpeed;
    public int damage;

    public static WeaponManager instance;

    private void Awake()
    {
        instance = this;    
    }

    private void Update()
    {
        if(WeaponSloat.item != null)
        {
            isCanFireAR = true;
        }
        else
        {
            isCanFireAR = false;
        }

        if (PistolSloat.item != null)
        {
            isCanFirePistol = true;
        }
        else
        {
            isCanFirePistol = false;
        }

        if(GrenadeCnt > 0)
        {
            isCanThrowGrenade = true;
        }
        else
        {
            isCanThrowGrenade = false;
        }

        if (Smoke_GrenadeCnt > 0)
        {
            isCanThrowSmokeGrenade = true;
        }
        else
        {
            isCanThrowSmokeGrenade = false;
        }

    }
}
