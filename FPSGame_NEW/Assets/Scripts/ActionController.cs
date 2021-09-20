using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ActionController : MonoBehaviour
{
    public float range; // 습득 가능한 최대 거리

    bool pickupActivated = false;

    RaycastHit hitInfo; //충돌체 정보 저장

    public LayerMask layerMask; //땅을 보고있는데 아이템을 획득하면 안되기 때문에 아이템 레이어에만 반응 하게 할려고 마스크 설정

    public Text actionText;
    public Text CantPickUpTxt;
    public float CantPickUpTime = 10;
    //Inventory 스크립트 함수 가져올수 있음
    public Inventory theInventory;

    //수류탄 프리펩
    public GameObject grenade;
    public GameObject Smokegrenade;
    public bool throwGrenade = false;


    Transform tr;
    public static ActionController instance;

    private void Awake()
    {
        instance = this;
        tr = GetComponent<Transform>();
    }

    // Update is called once per frame
    void Update()
    {
        CheckItem();
        TryAction();
        CantPickUpTime += Time.deltaTime;
        if(CantPickUpTime >=3) CantPickUpTxt.gameObject.SetActive(false);
    }


    void TryAction()
    {
        if(Input.GetButtonDown("PickUp"))
        {
            CheckItem();
            CanPickUp();
        }
        if(Input.GetButtonDown("Reload"))
        {
            Reload();
        }
    }

    void CanPickUp()
    {
       
        if (pickupActivated)
        {
            if(hitInfo.transform != null)
            {
                //Debug.Log(hitInfo.transform.GetComponent<ItemPickUp>().item.itemName + " 획득했습니다");
                if(hitInfo.transform.gameObject.GetComponent<ItemPickUp>().item.itemName == "Grenade")
                {
                    WeaponManager.instance.GrenadeCnt++;
                }
                if (hitInfo.transform.gameObject.GetComponent<ItemPickUp>().item.itemName == "Smoke Grenade")
                {
                    WeaponManager.instance.Smoke_GrenadeCnt++;
                }
                if ((Inventory.instnace.CurrentHand == 4 || Inventory.instnace.CurrentHand == 5))
                {
                    if(hitInfo.transform.gameObject.GetComponent<ItemPickUp>().item.itemType != Item.ItemType.Grenade)
                    {
                        CantPickUpTxt.gameObject.SetActive(true);
                        InfoDisappear();
                        CantPickUpTime = 0;
                        return;
                    }
                    theInventory.AcquireItem(hitInfo.transform.GetComponent<ItemPickUp>().item);
                    Destroy(hitInfo.transform.gameObject);
                    InfoDisappear(); 
                }
                if (hitInfo.transform.gameObject.GetComponent<ItemPickUp>().item.itemType == Item.ItemType.Ammo)
                {
                    if(hitInfo.transform.gameObject.GetComponent<ItemPickUp>().item.itemName == "ShotGunAmmo")
                    {
                        theInventory.AcquireItem(hitInfo.transform.GetComponent<ItemPickUp>().item, 10);
                    }
                    else if (hitInfo.transform.gameObject.GetComponent<ItemPickUp>().item.itemName == "SniperAmmo")
                    {
                        theInventory.AcquireItem(hitInfo.transform.GetComponent<ItemPickUp>().item, 5);
                    }
                    else theInventory.AcquireItem(hitInfo.transform.GetComponent<ItemPickUp>().item, 30);
                }
                else
                {
                    theInventory.AcquireItem(hitInfo.transform.GetComponent<ItemPickUp>().item);
                }
                
                hitInfo.transform.gameObject.GetComponent<Item_Pun>().ToRPC_Destory();
                

                //Destroy(hitInfo.transform.gameObject);
                InfoDisappear();
                    
                
            }
        }
    }

    void CheckItem()
    {
        if (Physics.Raycast(tr.position, tr.TransformDirection(Vector3.forward), out hitInfo, range, layerMask)) //tr.TransformDirection(Vector3.forward) == tr.forward
        {
            if (hitInfo.transform.tag == "Item" || hitInfo.transform.tag == "Equipment")
            {
                if(CantPickUpTxt.gameObject.activeSelf == false)
                ItemInfoAppear();
            }
        }
        else InfoDisappear();
    }

    void ItemInfoAppear()
    {
            pickupActivated = true;
            actionText.gameObject.SetActive(true);
            actionText.text = hitInfo.transform.GetComponent<ItemPickUp>().item.itemName + " 획득 " + "<color=yellow> " + "(F)" + "</color>";
    }

    void InfoDisappear()
    {
        pickupActivated = false;
        actionText.gameObject.SetActive(false);
    }

    void Reload()
    {
        int temp;

        if(Inventory.instnace.isLift1) //주무기 첫번째
        {
            if (WeaponManager.instance.WeaponSloat.GetComponent<Equipment>().item.itemName == "AK-47") //5.56mm 탄창
            {
                if (WeaponManager.instance.have5mm >= WeaponManager.instance.Ammo)
                {
                    Inventory.instnace.DeleteAmmo5(WeaponManager.instance.Ammo - WeaponManager.instance.CurrentAmmo);
                    temp = WeaponManager.instance.CurrentAmmo;
                    WeaponManager.instance.CurrentAmmo = WeaponManager.instance.Ammo;
                    WeaponManager.instance.have5mm = WeaponManager.instance.have5mm - (WeaponManager.instance.Ammo - temp);
                }
                else if (WeaponManager.instance.have5mm < WeaponManager.instance.Ammo)
                {
                    if (WeaponManager.instance.CurrentAmmo + WeaponManager.instance.have5mm > WeaponManager.instance.Ammo)
                    {
                        Inventory.instnace.DeleteAmmo5(WeaponManager.instance.Ammo - WeaponManager.instance.CurrentAmmo);
                        temp = WeaponManager.instance.CurrentAmmo;
                        WeaponManager.instance.CurrentAmmo = WeaponManager.instance.Ammo;
                        WeaponManager.instance.have5mm = WeaponManager.instance.have5mm - (WeaponManager.instance.Ammo - temp);
                    }
                    else
                    {
                        Inventory.instnace.DeleteAmmo5(WeaponManager.instance.Ammo - WeaponManager.instance.CurrentAmmo);
                        WeaponManager.instance.CurrentAmmo = WeaponManager.instance.CurrentAmmo + WeaponManager.instance.have5mm;
                        WeaponManager.instance.have5mm = 0;
                    }
                }
            }
            else if (WeaponManager.instance.WeaponSloat.GetComponent<Equipment>().item.itemName == "AKM") //7.62mm 탄창
            {
                if (WeaponManager.instance.have7mm >= WeaponManager.instance.Ammo)
                {
                    Inventory.instnace.DeleteAmmo7(WeaponManager.instance.Ammo - WeaponManager.instance.CurrentAmmo);
                    temp = WeaponManager.instance.CurrentAmmo;
                    WeaponManager.instance.CurrentAmmo = WeaponManager.instance.Ammo;
                    WeaponManager.instance.have7mm = WeaponManager.instance.have7mm - (WeaponManager.instance.Ammo - temp);
                }
                else if (WeaponManager.instance.have7mm < WeaponManager.instance.Ammo)
                {
                    if (WeaponManager.instance.CurrentAmmo + WeaponManager.instance.have7mm > WeaponManager.instance.Ammo)
                    {
                        Inventory.instnace.DeleteAmmo7(WeaponManager.instance.Ammo - WeaponManager.instance.CurrentAmmo);
                        temp = WeaponManager.instance.CurrentAmmo;
                        WeaponManager.instance.CurrentAmmo = WeaponManager.instance.Ammo;
                        WeaponManager.instance.have7mm = WeaponManager.instance.have7mm - (WeaponManager.instance.Ammo - temp);
                    }
                    else
                    {
                        Inventory.instnace.DeleteAmmo7(WeaponManager.instance.Ammo - WeaponManager.instance.CurrentAmmo);
                        WeaponManager.instance.CurrentAmmo = WeaponManager.instance.CurrentAmmo + WeaponManager.instance.have7mm;
                        WeaponManager.instance.have7mm = 0;
                    }
                }

            }
            else if(WeaponManager.instance.WeaponSloat.GetComponent<Equipment>().item.itemName == "ShotGun") //샷건 탄창
            {
                if (WeaponManager.instance.ShotGunAmmo >= WeaponManager.instance.Ammo)
                {
                    Inventory.instnace.DeleteShotGunAmmo(WeaponManager.instance.Ammo - WeaponManager.instance.CurrentAmmo);
                    temp = WeaponManager.instance.CurrentAmmo;
                    WeaponManager.instance.CurrentAmmo = WeaponManager.instance.Ammo;
                    WeaponManager.instance.ShotGunAmmo = WeaponManager.instance.ShotGunAmmo - (WeaponManager.instance.Ammo - temp);
                }
                else if (WeaponManager.instance.ShotGunAmmo < WeaponManager.instance.Ammo)
                {
                    if (WeaponManager.instance.CurrentAmmo + WeaponManager.instance.ShotGunAmmo > WeaponManager.instance.Ammo)
                    {
                        Inventory.instnace.DeleteShotGunAmmo(WeaponManager.instance.Ammo - WeaponManager.instance.CurrentAmmo);
                        temp = WeaponManager.instance.CurrentAmmo;
                        WeaponManager.instance.CurrentAmmo = WeaponManager.instance.Ammo;
                        WeaponManager.instance.ShotGunAmmo = WeaponManager.instance.ShotGunAmmo - (WeaponManager.instance.Ammo - temp);
                    }
                    else
                    {
                        Inventory.instnace.DeleteShotGunAmmo(WeaponManager.instance.Ammo - WeaponManager.instance.CurrentAmmo);
                        WeaponManager.instance.CurrentAmmo = WeaponManager.instance.CurrentAmmo + WeaponManager.instance.ShotGunAmmo;
                        WeaponManager.instance.ShotGunAmmo = 0;
                    }
                }
            }
            else if (WeaponManager.instance.WeaponSloat.GetComponent<Equipment>().item.itemName == "Sniper") //스나이퍼 탄창
            {
                if (WeaponManager.instance.sniperAmmo >= WeaponManager.instance.Ammo)
                {
                    Inventory.instnace.DeleteSniperAmmo(WeaponManager.instance.Ammo - WeaponManager.instance.CurrentAmmo);
                    temp = WeaponManager.instance.CurrentAmmo;
                    WeaponManager.instance.CurrentAmmo = WeaponManager.instance.Ammo;
                    WeaponManager.instance.sniperAmmo = WeaponManager.instance.sniperAmmo - (WeaponManager.instance.Ammo - temp);
                }
                else if (WeaponManager.instance.sniperAmmo < WeaponManager.instance.Ammo)
                {
                    if (WeaponManager.instance.CurrentAmmo + WeaponManager.instance.sniperAmmo > WeaponManager.instance.Ammo)
                    {
                        Inventory.instnace.DeleteSniperAmmo(WeaponManager.instance.Ammo - WeaponManager.instance.CurrentAmmo);
                        temp = WeaponManager.instance.CurrentAmmo;
                        WeaponManager.instance.CurrentAmmo = WeaponManager.instance.Ammo;
                        WeaponManager.instance.sniperAmmo = WeaponManager.instance.sniperAmmo - (WeaponManager.instance.Ammo - temp);
                    }
                    else
                    {
                        Inventory.instnace.DeleteSniperAmmo(WeaponManager.instance.Ammo - WeaponManager.instance.CurrentAmmo);
                        WeaponManager.instance.CurrentAmmo = WeaponManager.instance.CurrentAmmo + WeaponManager.instance.sniperAmmo;
                        WeaponManager.instance.sniperAmmo = 0;
                    }
                }
            }
        }
        else if(Inventory.instnace.isLift2) //주무기 2번째
        {
            if (WeaponManager.instance.WeaponSloat2.GetComponent<Equipment>().item.itemName == "AK-47") //5.56mm 탄창
            {
                if (WeaponManager.instance.have5mm >= WeaponManager.instance.Ammo)
                {
                    Inventory.instnace.DeleteAmmo5(WeaponManager.instance.Ammo - WeaponManager.instance.CurrentAmmo2
                        );
                    temp = WeaponManager.instance.CurrentAmmo2;
                    WeaponManager.instance.CurrentAmmo2 = WeaponManager.instance.Ammo;
                    WeaponManager.instance.have5mm = WeaponManager.instance.have5mm - (WeaponManager.instance.Ammo - temp);
                }
                else if (WeaponManager.instance.have5mm < WeaponManager.instance.Ammo)
                {
                    if (WeaponManager.instance.CurrentAmmo2 + WeaponManager.instance.have5mm > WeaponManager.instance.Ammo)
                    {
                        Inventory.instnace.DeleteAmmo5(WeaponManager.instance.Ammo - WeaponManager.instance.CurrentAmmo2);
                        temp = WeaponManager.instance.CurrentAmmo2;
                        WeaponManager.instance.CurrentAmmo2 = WeaponManager.instance.Ammo;
                        WeaponManager.instance.have5mm = WeaponManager.instance.have5mm - (WeaponManager.instance.Ammo - temp);
                    }
                    else
                    {
                        Inventory.instnace.DeleteAmmo5(WeaponManager.instance.Ammo - WeaponManager.instance.CurrentAmmo2);
                        WeaponManager.instance.CurrentAmmo2 = WeaponManager.instance.CurrentAmmo2 + WeaponManager.instance.have5mm;
                        WeaponManager.instance.have5mm = 0;
                    }
                }
            }
            else if (WeaponManager.instance.WeaponSloat2.GetComponent<Equipment>().item.itemName == "AKM") //7.62mm 탄창
            {
                if (WeaponManager.instance.have7mm >= WeaponManager.instance.Ammo)
                {
                    Inventory.instnace.DeleteAmmo7(WeaponManager.instance.Ammo - WeaponManager.instance.CurrentAmmo2);
                    temp = WeaponManager.instance.CurrentAmmo2;
                    WeaponManager.instance.CurrentAmmo2 = WeaponManager.instance.Ammo;
                    WeaponManager.instance.have7mm = WeaponManager.instance.have7mm - (WeaponManager.instance.Ammo - temp);
                }
                else if (WeaponManager.instance.have7mm < WeaponManager.instance.Ammo)
                {
                    if (WeaponManager.instance.CurrentAmmo2 + WeaponManager.instance.have7mm > WeaponManager.instance.Ammo)
                    {
                        Inventory.instnace.DeleteAmmo7(WeaponManager.instance.Ammo - WeaponManager.instance.CurrentAmmo2);
                        temp = WeaponManager.instance.CurrentAmmo2;
                        WeaponManager.instance.CurrentAmmo2 = WeaponManager.instance.Ammo;
                        WeaponManager.instance.have7mm = WeaponManager.instance.have7mm - (WeaponManager.instance.Ammo - temp);
                    }
                    else
                    {
                        Inventory.instnace.DeleteAmmo7(WeaponManager.instance.Ammo - WeaponManager.instance.CurrentAmmo2);
                        WeaponManager.instance.CurrentAmmo2 = WeaponManager.instance.CurrentAmmo2 + WeaponManager.instance.have7mm;
                        WeaponManager.instance.have7mm = 0;
                    }
                }

            }
            else if (WeaponManager.instance.WeaponSloat2.GetComponent<Equipment>().item.itemName == "ShotGun") //샷건 탄창
            {
                if (WeaponManager.instance.ShotGunAmmo >= WeaponManager.instance.Ammo)
                {
                    Inventory.instnace.DeleteShotGunAmmo(WeaponManager.instance.Ammo - WeaponManager.instance.CurrentAmmo2);
                    temp = WeaponManager.instance.CurrentAmmo2;
                    WeaponManager.instance.CurrentAmmo2 = WeaponManager.instance.Ammo;
                    WeaponManager.instance.ShotGunAmmo = WeaponManager.instance.ShotGunAmmo - (WeaponManager.instance.Ammo - temp);
                }
                else if (WeaponManager.instance.ShotGunAmmo < WeaponManager.instance.Ammo)
                {
                    if (WeaponManager.instance.CurrentAmmo2 + WeaponManager.instance.ShotGunAmmo > WeaponManager.instance.Ammo)
                    {
                        Inventory.instnace.DeleteShotGunAmmo(WeaponManager.instance.Ammo - WeaponManager.instance.CurrentAmmo2);
                        temp = WeaponManager.instance.CurrentAmmo2;
                        WeaponManager.instance.CurrentAmmo2 = WeaponManager.instance.Ammo;
                        WeaponManager.instance.ShotGunAmmo = WeaponManager.instance.ShotGunAmmo - (WeaponManager.instance.Ammo - temp);
                    }
                    else
                    {
                        Inventory.instnace.DeleteShotGunAmmo(WeaponManager.instance.Ammo - WeaponManager.instance.CurrentAmmo2);
                        WeaponManager.instance.CurrentAmmo2 = WeaponManager.instance.CurrentAmmo2 + WeaponManager.instance.ShotGunAmmo;
                        WeaponManager.instance.ShotGunAmmo = 0;
                    }
                }

            }
            else if (WeaponManager.instance.WeaponSloat2.GetComponent<Equipment>().item.itemName == "Sniper") //샷건 탄창
            {
                if (WeaponManager.instance.sniperAmmo >= WeaponManager.instance.Ammo)
                {
                    Inventory.instnace.DeleteSniperAmmo(WeaponManager.instance.Ammo - WeaponManager.instance.CurrentAmmo2);
                    temp = WeaponManager.instance.CurrentAmmo2;
                    WeaponManager.instance.CurrentAmmo2 = WeaponManager.instance.Ammo;
                    WeaponManager.instance.sniperAmmo = WeaponManager.instance.sniperAmmo - (WeaponManager.instance.Ammo - temp);
                }
                else if (WeaponManager.instance.sniperAmmo < WeaponManager.instance.Ammo)
                {
                    if (WeaponManager.instance.CurrentAmmo2 + WeaponManager.instance.sniperAmmo > WeaponManager.instance.Ammo)
                    {
                        Inventory.instnace.DeleteSniperAmmo(WeaponManager.instance.Ammo - WeaponManager.instance.CurrentAmmo2);
                        temp = WeaponManager.instance.CurrentAmmo2;
                        WeaponManager.instance.CurrentAmmo2 = WeaponManager.instance.Ammo;
                        WeaponManager.instance.sniperAmmo = WeaponManager.instance.sniperAmmo - (WeaponManager.instance.Ammo - temp);
                    }
                    else
                    {
                        Inventory.instnace.DeleteSniperAmmo(WeaponManager.instance.Ammo - WeaponManager.instance.CurrentAmmo2);
                        WeaponManager.instance.CurrentAmmo2 = WeaponManager.instance.CurrentAmmo2 + WeaponManager.instance.sniperAmmo;
                        WeaponManager.instance.sniperAmmo = 0;
                    }
                }

            }
        }
        else if(Inventory.instnace.isLift3) //권총
        {
            //if (WeaponManager.instance.PistolSloat.GetComponent<Equipment>().item.itemName == "Pistol") //9mm 탄창
            {
                if (WeaponManager.instance.have9mm >= WeaponManager.instance.Ammo)
                {
                    Inventory.instnace.DeleteAmmo9(WeaponManager.instance.Ammo - WeaponManager.instance.CurrentAmmo3);
                    temp = WeaponManager.instance.CurrentAmmo3;
                    WeaponManager.instance.CurrentAmmo3 = WeaponManager.instance.Ammo;
                    WeaponManager.instance.have9mm = WeaponManager.instance.have9mm - (WeaponManager.instance.Ammo - temp);
                }
                else if (WeaponManager.instance.have9mm < WeaponManager.instance.Ammo)
                {
                    if (WeaponManager.instance.CurrentAmmo3 + WeaponManager.instance.have9mm > WeaponManager.instance.Ammo)
                    {
                        Inventory.instnace.DeleteAmmo9(WeaponManager.instance.Ammo - WeaponManager.instance.CurrentAmmo3);
                        temp = WeaponManager.instance.CurrentAmmo3;
                        WeaponManager.instance.CurrentAmmo3 = WeaponManager.instance.Ammo;
                        WeaponManager.instance.have9mm = WeaponManager.instance.have9mm - (WeaponManager.instance.Ammo - temp);
                    }
                    else
                    {
                        Inventory.instnace.DeleteAmmo9(WeaponManager.instance.Ammo - WeaponManager.instance.CurrentAmmo3);
                        WeaponManager.instance.CurrentAmmo3 = WeaponManager.instance.CurrentAmmo3 + WeaponManager.instance.have9mm;
                        WeaponManager.instance.have9mm = 0;
                    }
                }

            }
        }
    }
}
