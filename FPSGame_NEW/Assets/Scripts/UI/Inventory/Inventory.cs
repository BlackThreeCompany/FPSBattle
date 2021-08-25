﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Inventory : MonoBehaviour
{
    public static bool inventoryActivated = false;

    public GameObject InventoryBG;
    public GameObject EquipmentBG;
    public GameObject SlotsParent;

    //슬롯듯
    Sloat[] slots;
    public Equipment WeaponSlots;
    public Equipment PistolSlots;
    public Equipment KnifeSlots;
    public Equipment ArmorSlots;


    // Start is called before the first frame update
    void Start()
    {
        slots = SlotsParent.GetComponentsInChildren<Sloat>();
    }

    // Update is called once per frame
    void Update()
    {
        TryOpenInventory();
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


}
