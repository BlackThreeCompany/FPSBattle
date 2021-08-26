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
    }

    void TryAction()
    {
        if(Input.GetButton("PickUp"))
        {
            CheckItem();
            CanPickUp();
        }
        //if (Input.GetButtonDown("ThrowGrenade") && !throwGrenade && WeaponManager.instance.isCanThrowGrenade)
        //{
        //    throwGrenade = true;
        //    SoundManager.instance.PlaySfx(tr.position, SoundManager.instance.ThrowGrenade, 0, SoundManager.instance.sfxVolum);
        //    GameObject CloneGrenade = grenade;
        //    //CloneGrenade = Instantiate(Smokegrenade, tr.position, tr.rotation);
        //    CloneGrenade = Instantiate(grenade, tr.position, tr.rotation);
        //    CloneGrenade.GetComponent<Grenade>().isBoom = true;
        //    WeaponManager.instance.GrenadeCnt--;
        //    CloneGrenade.layer = 7;
        //}
        //if (Input.GetButtonDown("ThrowSmokeGrenade") && !throwGrenade && WeaponManager.instance.isCanThrowSmokeGrenade)
        //{
        //    throwGrenade = true;
        //    SoundManager.instance.PlaySfx(tr.position, SoundManager.instance.ThrowGrenade, 0, SoundManager.instance.sfxVolum);
        //    GameObject CloneGrenade = Smokegrenade;
        //    //CloneGrenade = Instantiate(grenade, tr.position, tr.rotation);
        //    CloneGrenade = Instantiate(Smokegrenade, tr.position, tr.rotation);
        //    CloneGrenade.GetComponent<SmokeGrenade>().isBoom = true;
        //    WeaponManager.instance.Smoke_GrenadeCnt--;
        //    CloneGrenade.layer = 7;
        //}
    }

    void CanPickUp()
    {
        if(pickupActivated)
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

                theInventory.AcquireItem(hitInfo.transform.GetComponent<ItemPickUp>().item);
                Destroy(hitInfo.transform.gameObject);
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
}
