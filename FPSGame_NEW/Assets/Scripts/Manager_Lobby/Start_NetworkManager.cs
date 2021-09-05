using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using Photon.Realtime;
using UnityEngine.UI;

public class Start_NetworkManager : MonoBehaviourPunCallbacks
{
    // Start is called before the first frame update

    public Button ServerJoinBt;
    public InputField NickNameIp;

    public void ServerJoin()
    {
        PhotonNetwork.ConnectUsingSettings();
        ServerJoinBt.interactable = false;
        NickNameIp.interactable = false;
    }

    public override void OnConnectedToMaster()
    {
        if(NickNameIp.text == "")
        {
            NickNameIp.text = "Player" + Random.Range(0, 100);
        }

        PhotonNetwork.LocalPlayer.NickName = NickNameIp.text;
        PhotonNetwork.JoinLobby();
       

    }
    public override void OnDisconnected(DisconnectCause cause)
    {
        ServerJoinBt.interactable = true;
        NickNameIp.interactable = true;
    }
    public override void OnJoinedLobby()
    {
        PhotonNetwork.LoadLevel("Lobby");

    }
}
