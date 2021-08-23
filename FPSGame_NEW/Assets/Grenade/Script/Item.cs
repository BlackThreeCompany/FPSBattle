using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[CreateAssetMenu(fileName = "New Item", menuName = "New Item/item")]
public class Item : ScriptableObject //MonoBehaviour은 GameObject에 스크립트를 붙여야지만 실행
                                     //ScriptableObject는 안붙여도됨
{
    public string itemName;
    public Sprite itemImage; //Image는 캔버스가 필요, Sprite 캔버스 필요X
    public GameObject itemPrefab;


    public enum ItemType
    { 
        Equipment,
        Used,
        Weapon,
        Ammo,
        Grenade
    }

    public ItemType itemType;

}
