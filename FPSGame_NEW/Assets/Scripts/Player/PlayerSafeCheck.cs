using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerSafeCheck : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void OnTriggerEnter(Collider other)
    {
        if(other.gameObject.layer == 17)
        {
            StatManager.instance.SafeTx.SetActive(true);
            StatManager.instance.isInSafeArea = true;
        }
    }
    public void OnTriggerExit(Collider other)
    {
        if (other.gameObject.layer == 17)
        {
            StatManager.instance.SafeTx.SetActive(false);
            StatManager.instance.isInSafeArea = false;
        }
    }
}
