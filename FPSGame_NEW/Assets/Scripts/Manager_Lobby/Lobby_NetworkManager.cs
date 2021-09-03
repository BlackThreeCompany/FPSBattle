using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Photon.Pun;
using Photon.Realtime;
public class Lobby_NetworkManager : MonoBehaviour
{
    public Text NickNameTx;

    private void Awake()
    {
        NickNameTx.text = PhotonNetwork.LocalPlayer.NickName;
    }
}
