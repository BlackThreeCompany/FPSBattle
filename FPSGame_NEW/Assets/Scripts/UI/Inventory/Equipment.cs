using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;



public class Equipment : MonoBehaviour

{ 
    public Item item; // ȹ���� ������
    public int itemCnt;
    public Image itemImage;

    public Text text_cnt;
    public GameObject CntImage;

    //�̹��� ���� ����
    private void SetColor(float _alpha)
    {
        Color color = itemImage.color;
        color.a = _alpha;
        itemImage.color = color;
    }

    //������ ȹ��
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

    //������ ���� ����
    public void SetSloatCount(int _count)
    {
        itemCnt += _count;
        text_cnt.text = itemCnt.ToString();

        if (itemCnt <= 0)
        {
            claerSloat();
        }
    }

    //���� �ʱ�ȭ
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

