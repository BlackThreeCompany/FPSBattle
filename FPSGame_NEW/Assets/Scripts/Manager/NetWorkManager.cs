using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using Photon.Realtime;
using UnityEngine.UI;

public class NetWorkManager : MonoBehaviourPunCallbacks
{
    public GameObject JoinPn;
    public GameObject InGamePn;
    public Button JoinBt;
    public Text StatTx;
    public Text PingTx;
    public GameObject Player;
    public bool isJoined = false;

    public static NetWorkManager instance;
    void Start()
    {
        
    }
    void Awake()
    {
        instance = this;

        PhotonNetwork.SendRate = 40;
        PhotonNetwork.SerializationRate = 40;
    }
    // Update is called once per frame
    void Update()
    {
        StatTx.text = PhotonNetwork.NetworkClientState.ToString();
        if (isJoined)
        {
            PingTx.text = "Ping : " + PhotonNetwork.GetPing().ToString();
        }
    }

    public void JoinServer()
    {
        JoinBt.interactable = false;
        PhotonNetwork.ConnectUsingSettings();
    }

    public override void OnConnectedToMaster()
    {
        PhotonNetwork.JoinRandomRoom();
    }
    public override void OnJoinedRoom()
    {
        JoinPn.SetActive(false);
        InGamePn.SetActive(true);

        isJoined = true;

        Player = PhotonNetwork.Instantiate("Player", new Vector3(0, 10, 0),Quaternion.Euler(0,0,0));
        Inventory.instnace.Player = Player;
    }
    public override void OnJoinRandomFailed(short returnCode, string message)
    {
        PhotonNetwork.CreateRoom("Room", new RoomOptions { MaxPlayers = 20 });
    }
    

    public override void OnDisconnected(DisconnectCause cause)
    {
        isJoined = false;

        JoinPn.SetActive(true);
        InGamePn.SetActive(false);
        JoinBt.interactable = true;
    }
}
