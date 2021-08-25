using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeaponManager : MonoBehaviour
{
    public float FireSpeed;
    public int damage;

    public static WeaponManager instance;

    private void Awake()
    {
        instance = this;    
    }
}
