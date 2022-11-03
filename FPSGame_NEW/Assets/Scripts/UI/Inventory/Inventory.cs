using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Inventory : MonoBehaviour
{
    public static bool inventoryActivated = false;

    public int CurrentHand;
    public bool isLift1;
    public bool isLift2;
    public bool isLift3;

    //무게
    int Weight5mm = 3;
    int Weight7mm = 5;
    int Weight9mm = 2;
    int WeightSniperAmmo = 25;
    int WeightShotgunAmmo = 10;
    int WeightAK47 = 200;
    int WeightAKM = 250;
    int WeightSniper = 300;
    int WeightShotgun = 200;
    int WeightPistol = 50;

    int cnt;

    public float InventoryMaxVolume;
    public float InventoryCurrentVolume;

    public GameObject InventoryBG;
    public GameObject EquipmentBG;
    public GameObject ThrowBG;
    public GameObject InputNum;
    public GameObject SlotsParent;

    public Text InventoryCurrentVolumeTxt;
    
    //슬롯듯
    Sloat[] slots;
    public Equipment WeaponSlots;
    public Equipment WeaponSlots2;
    public Equipment PistolSlots;
    public Equipment KnifeSlots;
    public Equipment ArmorSlots;

    public GameObject Player;
    public GunController MyGunController;

    public static Inventory instnace;

    private void Awake()
    {
        instnace = this;
    }

    // Start is called before the first frame update
    void Start()
    {
        slots = SlotsParent.GetComponentsInChildren<Sloat>();
        InventoryCurrentVolume = 0;
    }

    // Update is called once per frame
    void Update()
    {

        if (MyGunController == null)
        {
            if(Player != null)
            {
                MyGunController = Player.GetComponent<GunController>();
            }
        }
        TryOpenInventory();
        InventoryCurrentVolumeTxt.text = "현재용량 : " + InventoryCurrentVolume.ToString();
        //CheckGrenade();
    }

    void TryOpenInventory()
    {
        if (Input.GetButtonDown("Inventory"))
        {
            inventoryActivated = !inventoryActivated;

            if (inventoryActivated)
            {
                OpenInventory();
            }
            else
            {
                CloseInventory();
            }


        }
    }


    void OpenInventory()
    {
        ThrowBG.SetActive(true);
        InventoryBG.SetActive(true);
        EquipmentBG.SetActive(true);

        Cursor.visible = true;
        Cursor.lockState = CursorLockMode.None;
    }

    void CloseInventory()
    {
        ThrowBG.SetActive(false);
        InventoryBG.SetActive(false);
        EquipmentBG.SetActive(false);

        Cursor.visible = false;
        Cursor.lockState = CursorLockMode.Locked;
    }

    public void AcquireItem(Item _item,int _count = 1)
    {

        if(Item.ItemType.Weapon == _item.itemType)
        {
                     
            if (WeaponSlots.item == null)
            {
                WeaponSlots.AddItem(_item, _count);
                return;
            }
            else if(WeaponSlots2.item == null)
            {
                WeaponSlots2.AddItem(_item, _count);
                return;
            }
            else
            {
                if(isLift1)
                {
                    if (WeaponManager.instance.WeaponSloat.item.itemName == "AK-47") //5.56mm 탄창
                    {
                        WeaponManager.instance.have5mm += WeaponManager.instance.CurrentAmmo;
                        DeleteAmmo5(-WeaponManager.instance.CurrentAmmo);
                        WeaponManager.instance.CurrentAmmo = 0;
                        if (Inventory.instnace.InventoryCurrentVolume - WeightAK47 >= 0)
                        {
                            Inventory.instnace.InventoryCurrentVolume -= WeightAK47;
                        }
                        else Inventory.instnace.InventoryCurrentVolume = 0;
                        
                    }
                    else if (WeaponManager.instance.WeaponSloat.item.itemName == "AKM") //7.62mm 탄창
                    {
                        WeaponManager.instance.have7mm += WeaponManager.instance.CurrentAmmo;
                        DeleteAmmo7(-WeaponManager.instance.CurrentAmmo);
                        WeaponManager.instance.CurrentAmmo = 0;
                        if (Inventory.instnace.InventoryCurrentVolume - WeightAKM >= 0)
                        {
                            Inventory.instnace.InventoryCurrentVolume -= WeightAKM;
                        }
                        else Inventory.instnace.InventoryCurrentVolume = 0;
                    }
                    else if (WeaponManager.instance.WeaponSloat.item.itemName == "ShotGun") //샷건탄창
                    {
                        WeaponManager.instance.ShotGunAmmo += WeaponManager.instance.CurrentAmmo;
                        DeleteShotGunAmmo(-WeaponManager.instance.CurrentAmmo);
                        WeaponManager.instance.CurrentAmmo = 0;
                        if (Inventory.instnace.InventoryCurrentVolume - WeightShotgun >= 0)
                        {
                            Inventory.instnace.InventoryCurrentVolume -= WeightShotgun;
                        }
                        else Inventory.instnace.InventoryCurrentVolume = 0;
                    }
                    else if (WeaponManager.instance.WeaponSloat.item.itemName == "Sniper") //스나이퍼탄창
                    {
                        WeaponManager.instance.ShotGunAmmo += WeaponManager.instance.CurrentAmmo;
                        DeleteSniperAmmo(-WeaponManager.instance.CurrentAmmo);
                        WeaponManager.instance.CurrentAmmo = 0;
                        if (Inventory.instnace.InventoryCurrentVolume - WeightSniper >= 0)
                        {
                            Inventory.instnace.InventoryCurrentVolume -= WeightSniper;
                        }
                        else Inventory.instnace.InventoryCurrentVolume = 0;
                    }
                    WeaponSlots.AddItem(_item, _count);
                    MyGunController.WeaponChnage();


                    return;
                }
                else if(isLift2)
                {
                    if (WeaponManager.instance.WeaponSloat2.item.itemName == "AK-47") //5.56mm 탄창
                    {
                        WeaponManager.instance.have5mm += WeaponManager.instance.CurrentAmmo2;
                        DeleteAmmo5(-WeaponManager.instance.CurrentAmmo2);
                        WeaponManager.instance.CurrentAmmo2 = 0;
                        if (Inventory.instnace.InventoryCurrentVolume - WeightAK47 >= 0)
                        {
                            Inventory.instnace.InventoryCurrentVolume -= WeightAK47;
                        }
                        else Inventory.instnace.InventoryCurrentVolume = 0;
                    }
                    else if (WeaponManager.instance.WeaponSloat2.item.itemName == "AKM") //7.62mm 탄창
                    {
                        WeaponManager.instance.have7mm += WeaponManager.instance.CurrentAmmo2;
                        DeleteAmmo7(-WeaponManager.instance.CurrentAmmo2);
                        WeaponManager.instance.CurrentAmmo2 = 0;
                        if (Inventory.instnace.InventoryCurrentVolume - WeightAKM >= 0)
                        {
                            Inventory.instnace.InventoryCurrentVolume -= WeightAKM;
                        }
                        else Inventory.instnace.InventoryCurrentVolume = 0;
                    }
                    else if (WeaponManager.instance.WeaponSloat2.item.itemName == "ShotGun") //샷건탄창
                    {
                        WeaponManager.instance.ShotGunAmmo += WeaponManager.instance.CurrentAmmo2;
                        DeleteShotGunAmmo(-WeaponManager.instance.CurrentAmmo2);
                        WeaponManager.instance.CurrentAmmo2 = 0;
                        if (Inventory.instnace.InventoryCurrentVolume - WeightShotgun >= 0)
                        {
                            Inventory.instnace.InventoryCurrentVolume -= WeightShotgun;
                        }
                        else Inventory.instnace.InventoryCurrentVolume = 0;
                    }
                    else if (WeaponManager.instance.WeaponSloat2.item.itemName == "Sniper") //스나이퍼탄창
                    {
                        WeaponManager.instance.ShotGunAmmo += WeaponManager.instance.CurrentAmmo2;
                        DeleteSniperAmmo(-WeaponManager.instance.CurrentAmmo2);
                        WeaponManager.instance.CurrentAmmo2 = 0;
                        if (Inventory.instnace.InventoryCurrentVolume - WeightSniper >= 0)
                        {
                            Inventory.instnace.InventoryCurrentVolume -= WeightSniper;
                        }
                        else Inventory.instnace.InventoryCurrentVolume = 0;
                    }
                    WeaponSlots2.AddItem(_item, _count);
                    MyGunController.ChangeWeapon2();


                    return;
                }
            }
        }
        else if (Item.ItemType.Pistol == _item.itemType)
        {
            if (PistolSlots.item == null)
            {
                PistolSlots.AddItem(_item, _count);
                return;
            }
        }
        else if (Item.ItemType.Knife == _item.itemType)
        {
            if (KnifeSlots.item == null)
            {
                KnifeSlots.AddItem(_item, _count);
                return;
            }
        }
        else if (Item.ItemType.Armor == _item.itemType)
        {
            //if (ArmorSlots.item == null)
            {
                ArmorSlots.AddItem(_item, _count);
                if(ArmorSlots.item.itemName == "Armor1")
                {
                    StatManager.instance.AP = 25;
                }
                else if (ArmorSlots.item.itemName == "Armor2")
                {
                    StatManager.instance.AP = 50;
                }
                else if (ArmorSlots.item.itemName == "Armor3")
                {
                    StatManager.instance.AP = 100;
                }
                return;
            }
        }
        else
        {
            if (Item.ItemType.Grenade != _item.itemType)
            {
                for (int i = 0; i < slots.Length; i++)
                {
                    if (slots[i].item != null)
                    {
                        if (slots[i].item.itemName == _item.itemName)
                        {
                            slots[i].SetSloatCount(_count);
                            if(_item.itemName == "5.56mm")
                            {
                                WeaponManager.instance.have5mm += _count;
                            }
                            if(_item.itemName == "7.62mm")
                            {
                                WeaponManager.instance.have7mm += _count;
                            }
                            if (_item.itemName == "9mm")
                            {
                                WeaponManager.instance.have9mm += _count;
                            }
                            if (_item.itemName == "ShotGunAmmo")
                            {
                                WeaponManager.instance.ShotGunAmmo += _count;
                            }
                            if (_item.itemName == "SniperAmmo")
                            {
                                WeaponManager.instance.sniperAmmo += _count;
                            }
                            return;
                        }
                    }

                }
            }

            for (int i = 0; i < slots.Length; i++)
            {
                if (slots[i].item == null)
                {
                    slots[i].AddItem(_item, _count);
                    if (_item.itemName == "5.56mm")
                    {
                        WeaponManager.instance.have5mm += _count;
                    }
                    if (_item.itemName == "7.62mm")
                    {
                        WeaponManager.instance.have7mm += _count;
                    }
                    if (_item.itemName == "9mm")
                    {
                        WeaponManager.instance.have9mm += _count;
                    }
                    if (_item.itemName == "ShotGunAmmo")
                    {
                        WeaponManager.instance.ShotGunAmmo += _count;
                    }
                    if (_item.itemName == "SniperAmmo")
                    {
                        WeaponManager.instance.sniperAmmo += _count;
                    }
                    return;
                }
            }
        }
    }

    public void DeleteItem(int cnt,Item item)
    {
        for (int i = 0; i < slots.Length; i++)
        {
            if (slots[i].item == null) continue;
            if (slots[i].item.itemName == item.itemName)
            {
                slots[i].SetSloatCount(-cnt);
                if (Inventory.instnace.InventoryCurrentVolume - (cnt) * item.weight >= 0)
                {
                    Inventory.instnace.InventoryCurrentVolume -= (cnt) * item.weight;
                }
                else Inventory.instnace.InventoryCurrentVolume = 0;


                return;
            }
        }
    }

    public void DeleteAmmo5(int cnt)
    {
        for (int i = 0; i < slots.Length; i++)
        {
            if (slots[i].item == null) continue;
            if (slots[i].item.itemName == "5.56mm")
            {
                slots[i].SetSloatCount(-cnt);
                if (Inventory.instnace.InventoryCurrentVolume - (cnt) * Weight5mm >= 0)
                {
                    Inventory.instnace.InventoryCurrentVolume -= (cnt) * Weight5mm;
                }
                else Inventory.instnace.InventoryCurrentVolume = 0;


                return;
            }
        }
    }

    public void DeleteAmmo7(int cnt)
    {
        for (int i = 0; i < slots.Length; i++)
        {
            if (slots[i].item == null) continue;
            if (slots[i].item.itemName == "7.62mm")
            {
                slots[i].SetSloatCount(-cnt);
                if (Inventory.instnace.InventoryCurrentVolume - (cnt) * Weight7mm >= 0)
                {
                    Inventory.instnace.InventoryCurrentVolume -= (cnt) * Weight7mm;
                }
                else Inventory.instnace.InventoryCurrentVolume = 0;
                return;
            }
        }
    }

    public void DeleteShotGunAmmo(int cnt)
    {
        for (int i = 0; i < slots.Length; i++)
        {
            if (slots[i].item == null) continue;
            if (slots[i].item.itemName == "ShotGunAmmo")
            {
                slots[i].SetSloatCount(-cnt);
                if (Inventory.instnace.InventoryCurrentVolume - (cnt) * WeightShotgunAmmo >= 0)
                {
                    Inventory.instnace.InventoryCurrentVolume -= (cnt) * WeightShotgunAmmo;
                }
                else Inventory.instnace.InventoryCurrentVolume = 0;
                return;
            }
        }
    }

    public void DeleteSniperAmmo(int cnt)
    {
        for (int i = 0; i < slots.Length; i++)
        {
            if (slots[i].item == null) continue;
            if (slots[i].item.itemName == "SniperAmmo")
            {
                slots[i].SetSloatCount(-cnt);
                if (Inventory.instnace.InventoryCurrentVolume - (cnt) * WeightSniperAmmo >= 0)
                {
                    Inventory.instnace.InventoryCurrentVolume -= (cnt) * WeightSniperAmmo;
                }
                else Inventory.instnace.InventoryCurrentVolume = 0;
                return;
            }
        }
    }

    public void DeleteAmmo9(int cnt)
    {
        for (int i = 0; i < slots.Length; i++)
        {
            if (slots[i].item == null) continue;
            if (slots[i].item.itemName == "9mm")
            {
                slots[i].SetSloatCount(-cnt);
                if (Inventory.instnace.InventoryCurrentVolume - (cnt) * Weight9mm >= 0)
                {
                    Inventory.instnace.InventoryCurrentVolume -= (cnt) * Weight9mm;
                }
                else Inventory.instnace.InventoryCurrentVolume = 0;
                return;
            }
        }
    }

    public void DeleteGrenade()
    {
        for (int i = 0; i < slots.Length; i++)
        {
            if (slots[i].item == null) continue;
            if (slots[i].item.itemName == "Grenade")
            {
                if (Inventory.instnace.InventoryCurrentVolume - 10 >= 0)
                {
                    Inventory.instnace.InventoryCurrentVolume -= 10;
                }
                else Inventory.instnace.InventoryCurrentVolume = 0;
                slots[i].SetSloatCount(-1);
                return;
            }
        }
        
    }
    public void DeleteSmokeGrenade()
    {
        for (int i = 0; i < slots.Length; i++)
        {
            if (slots[i].item == null) continue;
            if (slots[i].item.itemName == "Smoke Grenade")
            {
                Inventory.instnace.InventoryCurrentVolume -= 10;
                slots[i].SetSloatCount(-1);
                return;
            }
        }
    }

}
