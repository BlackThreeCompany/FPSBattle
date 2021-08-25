using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;



public class Equipment : MonoBehaviour

{ 
    public Item item; // 획득한 아이템
    public int itemCnt;
    public Image itemImage;

    public Text text_cnt;
    public GameObject CntImage;

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

        if (item.itemType != Item.ItemType.Equipment && !(Item.ItemType.Pistol == item.itemType || Item.ItemType.Weapon == item.itemType || Item.ItemType.Knife == item.itemType || Item.ItemType.Armor == item.itemType))
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

        if (itemCnt <= 0)
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

}

