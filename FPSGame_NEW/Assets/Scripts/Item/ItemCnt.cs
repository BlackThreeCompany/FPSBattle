using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemCnt : MonoBehaviour
{
    public int itemCnt;


    // Start is called before the first frame update
    void Start()
    {
        itemCnt = this.GetComponent<ItemPickUp>().item.itemCnt;
    }
}
