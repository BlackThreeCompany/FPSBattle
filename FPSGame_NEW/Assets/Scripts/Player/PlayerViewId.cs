using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using Photon.Realtime;

public class PlayerViewId : MonoBehaviourPunCallbacks
{
    public PhotonView pv;
    public int ViewId;
    // Start is called before the first frame update
    private void Start()
    {
        ViewId = pv.ViewID;
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
