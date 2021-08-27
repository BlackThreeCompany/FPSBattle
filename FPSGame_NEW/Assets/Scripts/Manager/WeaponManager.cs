using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeaponManager : MonoBehaviour
{
    public Equipment WeaponSloat;
    public Equipment WeaponSloat2;
    public Equipment PistolSloat;

    public bool isCanFireAR = false;
    public bool isCanFirePistol = false;
    public bool isCanThrowGrenade = false;
    public bool isCanThrowSmokeGrenade = false;

    public int GrenadeCnt;
    public int Smoke_GrenadeCnt;

    public int CurrentAmmo;
    public int Ammo;

    public float FireSpeed;
    public int damage;

    public static WeaponManager instance;

    public string WeaponName;
    public string WeaponName2;

    public string PistolName;


    private void Awake()
    {
        instance = this;    
    }

    private void Update()
    {
        if (WeaponSloat.item != null)
        {
            if (WeaponSloat.GetComponent<Equipment>().item.itemName == "AKM")
            {
                WeaponName = "AKM";
                isCanFireAR = true;
            }
            else if (WeaponSloat.GetComponent<Equipment>().item.itemName == "AK-47")
            {
                WeaponName = "AK-47";
                isCanFireAR = true;
            }
        }
        

        if (WeaponSloat2.item != null)
        {
            if (WeaponSloat2.GetComponent<Equipment>().item.itemName == "AKM")
            {
                WeaponName2 = "AKM";
                isCanFireAR = true;
            }
            else if (WeaponSloat2.GetComponent<Equipment>().item.itemName == "AK-47")
            {
                WeaponName2 = "AK-47";
                isCanFireAR = true;
            }
        }
        if (WeaponSloat.item == null && WeaponSloat2.item == null) isCanFireAR = false;

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
