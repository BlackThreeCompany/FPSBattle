using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class WeaponManager : MonoBehaviour
{
    public Equipment WeaponSloat;
    public Equipment WeaponSloat2;
    public Equipment PistolSloat;
    public Equipment ArmorSloat;

    public bool isCanFireAR = false;
    public bool isCanFirePistol = false;
    public bool isCanThrowGrenade = false;
    public bool isCanThrowSmokeGrenade = false;
    public bool isHasArmor = false;

    public int GrenadeCnt;
    public int Smoke_GrenadeCnt;

    public int CurrentAmmo;
    public int CurrentAmmo2;
    public int CurrentAmmo3;
    public int Ammo;
    public int have5mm;
    public int have7mm;
    public int have9mm;
    public int ShotGunAmmo;
    public int sniperAmmo;

    public bool aim;

    public float FireSpeed;
    public int damage;
    public float ReboundPower;

    public Vector3 SpreadBullet;

    public static WeaponManager instance;

    //public string WeaponName;
    //public string WeaponName2;

    

    public Text HaveAmmotxt;
    public Text CurrentAmmotxt;

    public Text AmmoTx;
    private void Awake()
    {
        instance = this;    
    }

    private void Start()
    {
        
    }
    private void Update()
    {
        if (Inventory.instnace.isLift1)
        {
            if (WeaponSloat.GetComponent<Equipment>().item.itemName == "AK-47") //5.56mmźâ
            {
                //HaveAmmotxt.text = have5mm.ToString();
                //CurrentAmmotxt.text = CurrentAmmo.ToString();
                AmmoTx.text = CurrentAmmo.ToString() + " / " + have5mm.ToString();
            }
            else if (WeaponSloat.GetComponent<Equipment>().item.itemName == "AKM") //7.62mmźâ
            {
                //HaveAmmotxt.text = have7mm.ToString();
                //
                //txt.text = CurrentAmmo.ToString();
                AmmoTx.text = CurrentAmmo.ToString() + " / " + have7mm.ToString();
            }
            else if(WeaponSloat.GetComponent<Equipment>().item.itemName == "ShotGun") //샷건탄창
            {
                //HaveAmmotxt.text = have7mm.ToString();
                //CurrentAmmotxt.text = CurrentAmmo.ToString();
                AmmoTx.text = CurrentAmmo.ToString() + " / " + ShotGunAmmo.ToString();
            }
            else if (WeaponSloat.GetComponent<Equipment>().item.itemName == "Sniper") //스나이퍼탄창
            {
                //HaveAmmotxt.text = have7mm.ToString();
                //CurrentAmmotxt.text = CurrentAmmo.ToString();
                AmmoTx.text = CurrentAmmo.ToString() + " / " + sniperAmmo.ToString();
            }

        }
        else if (Inventory.instnace.isLift2)
        {
            if (WeaponSloat2.GetComponent<Equipment>().item.itemName == "AK-47") //5.56mmźâ
            {
                //HaveAmmotxt.text = have5mm.ToString();
                //CurrentAmmotxt.text = CurrentAmmo2.ToString();
                AmmoTx.text = CurrentAmmo2.ToString() + " / " + have5mm.ToString();
            }
            else if(WeaponSloat2.GetComponent<Equipment>().item.itemName == "AKM") //7.62mmźâ
            {
                //HaveAmmotxt.text = have7mm.ToString();
                //CurrentAmmotxt.text = CurrentAmmo2.ToString();
                AmmoTx.text = CurrentAmmo2.ToString() + " / " + have7mm.ToString();
            }
            else if (WeaponSloat2.GetComponent<Equipment>().item.itemName == "ShotGun") //샷건탄창
            {
                //HaveAmmotxt.text = have7mm.ToString();
                //CurrentAmmotxt.text = CurrentAmmo.ToString();
                AmmoTx.text = CurrentAmmo2.ToString() + " / " + ShotGunAmmo.ToString();
            }
            else if (WeaponSloat2.GetComponent<Equipment>().item.itemName == "Sniper") //샷건탄창
            {
                //HaveAmmotxt.text = have7mm.ToString();
                //CurrentAmmotxt.text = CurrentAmmo.ToString();
                AmmoTx.text = CurrentAmmo2.ToString() + " / " + sniperAmmo.ToString();
            }
        }
        else if(Inventory.instnace.isLift3)
        {
            //if (WeaponSloat2.GetComponent<Equipment>().item.itemName == "AK-47") //9mm
            {
                //HaveAmmotxt.text = have9mm.ToString();
                //CurrentAmmotxt.text = CurrentAmmo3.ToString();
                AmmoTx.text = CurrentAmmo3.ToString() + " / " + have9mm.ToString();
            }
        }
        
        if(StatManager.instance.AP < 0)
        {
            ArmorSloat.claerSloat();
            StatManager.instance.AP = 0;
        }

        if(ArmorSloat.item != null)
        {
            isHasArmor = true;
        }
        else
        {
            isHasArmor = false;
        }

        if(!Inventory.inventoryActivated)
        {
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
                else if (WeaponSloat.GetComponent<Equipment>().item.itemName == "ShotGun")
                {
                    isCanFireAR = true;
                }
                else if (WeaponSloat.GetComponent<Equipment>().item.itemName == "Sniper")
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
                else if (WeaponSloat2.GetComponent<Equipment>().item.itemName == "ShotGun")
                {
                    isCanFireAR = true;
                }
                else if (WeaponSloat2.GetComponent<Equipment>().item.itemName == "Sniper")
                {
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

            if (GrenadeCnt > 0)
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
        else
        {
            isCanFireAR = false;
            isCanFirePistol = false;
            isCanThrowGrenade = false;
            isCanThrowSmokeGrenade = false;
        }

        


    }
}
