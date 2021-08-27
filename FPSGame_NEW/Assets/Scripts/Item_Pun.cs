using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using Photon.Realtime;
public class Item_Pun : MonoBehaviourPunCallbacks
{
    public PhotonView pv;

    public void ToRPC_Destory()
    {
        
        pv.RPC("Destory_RPC", RpcTarget.AllBuffered);
    }
    [PunRPC]
    private void Destory_RPC()
    {
        
        Destroy(this.gameObject);
    }
}
