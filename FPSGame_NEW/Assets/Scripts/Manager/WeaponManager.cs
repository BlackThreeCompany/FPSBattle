using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

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
    public int CurrentAmmo2;
    public int CurrentAmmo3;
    public int Ammo;
    public int have5mm;
    public int have7mm;
    public int have9mm;


    public float FireSpeed;
    public int damage;

    public static WeaponManager instance;

    //public string WeaponName;
    //public string WeaponName2;



    public Text HaveAmmotxt;
    public Text CurrentAmmotxt;

    private void Awake()
    {
        instance = this;    
    }

    private void Update()
    {

        if (Inventory.instnace.isLift1)
        {
            if (WeaponSloat.GetComponent<Equipment>().item.itemName == "AK-47") //5.56mmźâ
            {
                HaveAmmotxt.text = have5mm.ToString();
                CurrentAmmotxt.text = CurrentAmmo.ToString();
            }
            if (WeaponSloat.GetComponent<Equipment>().item.itemName == "AKM") //7.62mmźâ
            {
                HaveAmmotxt.text = have7mm.ToString();
                CurrentAmmotxt.text = CurrentAmmo.ToString();
            }

        }
        else if (Inventory.instnace.isLift2)
        {
            if (WeaponSloat2.GetComponent<Equipment>().item.itemName == "AK-47") //5.56mmźâ
            {
                HaveAmmotxt.text = have5mm.ToString();
                CurrentAmmotxt.text = CurrentAmmo2.ToString();
            }
            if (WeaponSloat2.GetComponent<Equipment>().item.itemName == "AKM") //7.62mmźâ
            {
                HaveAmmotxt.text = have7mm.ToString();
                CurrentAmmotxt.text = CurrentAmmo2.ToString();
            }
        }
        else if(Inventory.instnace.isLift3)
        {
            //if (WeaponSloat2.GetComponent<Equipment>().item.itemName == "AK-47") //9mm
            {
                HaveAmmotxt.text = have9mm.ToString();
                CurrentAmmotxt.text = CurrentAmmo3.ToString();
            }
        }



        if (WeaponSloat.item != null)
        {
            if (WeaponSloat.GetComponent<Equipment>().item.itemName == "AKM")
            {
                isCanFireAR = true;
            }
            else if (WeaponSloat.GetComponent<Equipment>().item.itemName == "AK-47")
            {
                isCanFireAR = true;
            }
        }
        

        if (WeaponSloat2.item != null)
        {
            if (WeaponSloat2.GetComponent<Equipment>().item.itemName == "AKM")
            {
                isCanFireAR = true;
            }
            else if (WeaponSloat2.GetComponent<Equipment>().item.itemName == "AK-47")
            {
                isCanFireAR = true;
            }
        }
        if(WeaponSloat.item == null && WeaponSloat2.item == null) isCanFireAR = false;

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
