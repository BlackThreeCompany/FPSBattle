using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeaponManager : MonoBehaviour
{
    public Equipment WeaponSloat;
    public Equipment PistolSloat;

    public bool isCanFireAR = false;
    public bool isCanFirePistol = false;

    public float FireSpeed;
    public int damage;

    public static WeaponManager instance;

    private void Awake()
    {
        instance = this;    
    }

    private void Update()
    {
        if(WeaponSloat.item != null)
        {
            isCanFireAR = true;
        }
        else
        {
            isCanFireAR = false;
        }

        if (PistolSloat.item != null)
        {
            isCanFirePistol = true;
        }
        else
        {
            isCanFirePistol = false;
        }
    }
}
