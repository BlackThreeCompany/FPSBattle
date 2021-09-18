using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using Photon.Realtime;
public class ShotGunBulletMove : MonoBehaviourPunCallbacks
{
    private void Awake()
    {
        GetComponent<Rigidbody>().AddForce(transform.forward * StatManager.instance.BulletSpeed);
    }
}
