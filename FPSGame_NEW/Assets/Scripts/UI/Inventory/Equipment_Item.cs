using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Equipment_Item : MonoBehaviour
{
    public GameObject SlotsParent2;

    Sloat[] slots2;

    public static Equipment_Item instance;
    private void Awake()
    {
        instance = this;
    }

    // Start is called before the first frame update
    void Start()
    {
        slots2 = SlotsParent2.GetComponentsInChildren<Sloat>();
        
    }

    
    public void EquipmentWeapon(Item _item)
    {
            if (slots2[0].item == null)
            {
                slots2[0].AddItem(_item, 1);
                return;
            }
    }
    public void EquipmentPistol(Item _item)
    {
            if (slots2[1].item == null)
            {
                slots2[1].AddItem(_item, 1);
                return;
            }
    }
    public void EquipmentArmor(Item _item)
    {
            if (slots2[3].item == null)
            {
                slots2[3].AddItem(_item, 1);
                return;
            }
    }
    public void EquipmentKnife(Item _item)
    {
            if (slots2[4].item == null)
            {
                slots2[4].AddItem(_item, 1);
                return;
            }
    }

}
