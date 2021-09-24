using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

public class PlayerSafeCheck : MonoBehaviourPunCallbacks
{
    public PhotonView pv;
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
        if (!pv.IsMine) return;

        if(other.gameObject.layer == 18)
        {
            StatManager.instance.SafeTx.SetActive(true);
            StatManager.instance.isInSafeArea = true;
        }
    }
    public void OnTriggerExit(Collider other)
    {
        if (!pv.IsMine) return;

        if (other.gameObject.layer == 18)
        {
            StatManager.instance.SafeTx.SetActive(false);
            StatManager.instance.isInSafeArea = false;
        }
    }
}
