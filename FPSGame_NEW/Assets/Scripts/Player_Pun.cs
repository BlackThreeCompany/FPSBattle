using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using Photon.Realtime;
public class Player_Pun : MonoBehaviourPunCallbacks
{
    public GunController gunController;
    public int CurrentHand;
    public int LastCurrentHand;
    public PhotonView pv;

    
    // Start is called before the first frame update
    void Start()
    {
        //GameManager.instance.MYPv = pv;
    }

    // Update is called once per frame
    void Update()
    {
        if (!pv.IsMine)
        {

            return;
        }

        ChangeHand();




    }

    public void ChangeHand()
    {



        CurrentHand = gunController.CurrentHand;
        if (CurrentHand != LastCurrentHand)
        {
            LastCurrentHand = CurrentHand;
            pv.RPC("ChangeHand_RPC", RpcTarget.OthersBuffered, CurrentHand);
        }

    }
    [PunRPC]
    private void ChangeHand_RPC(int hand)
    {
        CurrentHand = hand;
        if (CurrentHand == 0)
        {
            gunController.AR1.SetActive(false);
            gunController.AR2.SetActive(false);
            gunController.ShotGun1.SetActive(false);
            gunController.HandGun.SetActive(false);
            gunController.Grenade.SetActive(false);
            gunController.SmokeGrenade.SetActive(false);
        }
        else if (CurrentHand == 1)
        {
            gunController.AR1.SetActive(true);
            gunController.AR2.SetActive(false);
            gunController.ShotGun1.SetActive(false);
            gunController.HandGun.SetActive(false);
            gunController.Grenade.SetActive(false);
            gunController.SmokeGrenade.SetActive(false);
        }
        else if (CurrentHand == 2)
        {
            gunController.AR1.SetActive(false);
            gunController.AR2.SetActive(true);
            gunController.ShotGun1.SetActive(false);
            gunController.HandGun.SetActive(false);
            gunController.Grenade.SetActive(false);
            gunController.SmokeGrenade.SetActive(false);
        }
        if (CurrentHand == 3)
        {
            gunController.AR1.SetActive(false);
            gunController.AR2.SetActive(false);
            gunController.ShotGun1.SetActive(false);
            gunController.HandGun.SetActive(true);
            gunController.Grenade.SetActive(false);
            gunController.SmokeGrenade.SetActive(false);
        }
        if (CurrentHand == 4)
        {
            gunController.AR1.SetActive(false);
            gunController.AR2.SetActive(false);
            gunController.ShotGun1.SetActive(false);
            gunController.HandGun.SetActive(false);
            gunController.Grenade.SetActive(true);
            gunController.SmokeGrenade.SetActive(false);
        }
        if (CurrentHand == 5)
        {
            gunController.AR1.SetActive(false);
            gunController.AR2.SetActive(false);
            gunController.ShotGun1.SetActive(false);
            gunController.HandGun.SetActive(false);
            gunController.Grenade.SetActive(false);
            gunController.SmokeGrenade.SetActive(true);
        }
        if (CurrentHand == 6)
        {
            gunController.AR1.SetActive(false);
            gunController.AR2.SetActive(false);
            gunController.ShotGun1.SetActive(true);
            gunController.HandGun.SetActive(false);
            gunController.Grenade.SetActive(false);
            gunController.SmokeGrenade.SetActive(true);
        }
    }
}
