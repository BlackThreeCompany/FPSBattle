using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using Photon.Realtime;
public class HandGun : MonoBehaviourPunCallbacks
{
    public bool isHandGun = false;

    public int damage;

    private void Start()
    {
        isHandGun = true;
    }
}
