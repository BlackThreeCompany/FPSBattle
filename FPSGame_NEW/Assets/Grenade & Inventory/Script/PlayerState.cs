using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerState : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void HitByGrenade(Vector3 explosionPos)
    {
        float distance = Vector3.Distance(explosionPos, transform.position);
        //Debug.Log(distance);
        if(distance >= 0 && distance <= 3.7)
        {
            Debug.Log("������ 100");
        }
        else
        {
            float damage = (1 / distance) * 300;
            Debug.Log("������ " + (int)damage);
        }
    }
}
