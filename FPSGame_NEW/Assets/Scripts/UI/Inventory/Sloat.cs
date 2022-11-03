using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;



public class Sloat : MonoBehaviour , IPointerClickHandler //IPointerClickHandler이거 추가하면 처음에는 오류뜸 오류 해결하려면 우클릭하고 '빠른 작업 및 리팩터링' 클릭해서 인터페이스 구현 클릭하기 
                                   ,IBeginDragHandler,IDragHandler,IEndDragHandler,IDropHandler
{


    public Item item; // 획득한 아이템
    public int itemCnt;
    public Image itemImage;

    public Text text_cnt;
    public GameObject CntImage;

    Vector2 MousePosition;

    //이미지 투명도 조절
    private void SetColor(float _alpha)
    {
        Color color = itemImage.color;
        color.a = _alpha;
        itemImage.color = color;
    }

    //아이템 획득
    public void AddItem(Item _item, int _count = 1)
    {
        item = _item;
        itemCnt = _count;
        itemImage.sprite = item.itemImage;

        if(item.itemType != Item.ItemType.Equipment && !(Item.ItemType.Pistol == item.itemType || Item.ItemType.Weapon == item.itemType || Item.ItemType.Knife == item.itemType || Item.ItemType.Armor == item.itemType || Item.ItemType.Grenade == item.itemType))
        {
            CntImage.SetActive(true);
            text_cnt.text = itemCnt.ToString();
        }
        else
        {
            text_cnt.text = "0";
            CntImage.SetActive(false);
        }

        SetColor(1);
    }

    //아이템 갯수 조정
    public void SetSloatCount(int _count)
    {
        itemCnt += _count;
        text_cnt.text = itemCnt.ToString();

        if(itemCnt <= 0)
        {
            claerSloat();
        }
    }

    //슬롯 초기화
    void claerSloat()
    {
        item = null;
        itemCnt = 0;
        itemImage.sprite = null;
        SetColor(0);

        text_cnt.text = "0";
        CntImage.SetActive(false);
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        if(eventData.button == PointerEventData.InputButton.Right)
        {
            if(item != null)
            {
                MousePosition = Input.mousePosition;
                Inventory.instnace.InputNum.SetActive(true);
                Inventory.instnace.InputNum.transform.position = MousePosition;
                ThrowItems.instance.CopyItem(item);
                //SetSloatCount(-1);
            }
        }
    }

    public void OnBeginDrag(PointerEventData eventData)
    {
        if (item != null)
        {
            if(Item.ItemType.Weapon != item.itemType && Item.ItemType.Pistol != item.itemType && Item.ItemType.Knife != item.itemType && Item.ItemType.Armor != item.itemType)
            {
                DragSlot.instance.dragSlot = this;
                DragSlot.instance.DragSetImage(itemImage);
                DragSlot.instance.SetColor(1);

                DragSlot.instance.transform.position = eventData.position;
            }
            
        }
    }

    public void OnDrag(PointerEventData eventData)
    {
        if (item != null)
        {
            if (Item.ItemType.Weapon != item.itemType && Item.ItemType.Pistol != item.itemType && Item.ItemType.Knife != item.itemType && Item.ItemType.Armor != item.itemType)
            {
                DragSlot.instance.transform.position = eventData.position;
            }
        }
    }

    public void OnEndDrag(PointerEventData eventData) //OnEndDrag 그냥 드래그가 끝날때 실행
    {
        //Debug.Log("OnEndDrag 호출됨");
        DragSlot.instance.SetColor(0);
        DragSlot.instance.dragSlot = null;
    }

    public void OnDrop(PointerEventData eventData) //OnDrop 다른 슬롯 위에서 드래그가 끝났을때 실행
    {
        //Debug.Log("OnDrop 호출됨");
        if (DragSlot.instance.dragSlot != null)
        {
            ChangeSloat();
        }
    }

    void ChangeSloat()
    {
        
        Item _tempItem = item;
        int _tempItemCount = itemCnt;

        AddItem(DragSlot.instance.dragSlot.item, DragSlot.instance.dragSlot.itemCnt);

        if (_tempItem != null)
        {
            DragSlot.instance.dragSlot.AddItem(_tempItem, _tempItemCount);
        }
        else
        {
            DragSlot.instance.dragSlot.claerSloat();
        }

    }

    
}

