using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ThrowItems : MonoBehaviour
{
    public bool activated = false;

    public Text Txt;
    public InputField inputTxt;

    public GameObject test;

    Item ThrowItem;

    public static ThrowItems instance;

    private void Awake()
    {
        instance = this;
    }

    public void CopyItem(Item item)
    {
        ThrowItem = item;
    }

    public void OK()
    {
        Inventory.instnace.DeleteItem(int.Parse(inputTxt.text), ThrowItem);
        ThorwItem();
        inputTxt.text = 0.ToString();
        Inventory.instnace.InputNum.SetActive(false);

        
    }
    public void Cancel()
    {
        inputTxt.text = 0.ToString();
        Inventory.instnace.InputNum.SetActive(false);
    }

    //�����Ҷ� ���� �� ���̵� �����ؾߵ�.
    //�����Ҷ� ItemCnt�� ���� �ȵ�
    void ThorwItem()
    {
        GameObject Throw = Instantiate(ThrowItem.itemPrefab,this.transform.position,this.transform.rotation);
        Throw.GetComponent<ItemCnt>().itemCnt = int.Parse(inputTxt.text);
        Debug.Log(Throw.GetComponent<ItemCnt>().itemCnt);
    }

}
