using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Inventory : MonoBehaviour
{
    public static bool inventoryActivated = false;

    public int CurrentHand;
    int cnt;

    public GameObject InventoryBG;
    public GameObject EquipmentBG;
    public GameObject SlotsParent;


    
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
    }

    // Update is called once per frame
    void Update()
    {
        
        if(MyGunController == null)
        {
            if(Player != null)
            {
                MyGunController = Player.GetComponent<GunController>();
            }
        }
        TryOpenInventory();
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
        InventoryBG.SetActive(true);
        EquipmentBG.SetActive(true);
    }

    void CloseInventory()
    {
        InventoryBG.SetActive(false);
        EquipmentBG.SetActive(false);
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
                if(CurrentHand == 1)
                {
                    WeaponSlots.AddItem(_item, _count);
                    MyGunController.ChangeWeapon2();

                    return;
                }
                else if(CurrentHand == 2)
                {
                    WeaponSlots2.AddItem(_item, _count);
                    MyGunController.WeaponChnage();

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
            if (ArmorSlots.item == null)
            {
                ArmorSlots.AddItem(_item, _count);
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
                    return;
                }
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
                slots[i].SetSloatCount(-1);
                return;
            }
        }
    }

}
