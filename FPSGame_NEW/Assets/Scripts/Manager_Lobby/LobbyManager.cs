using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using Photon.Realtime;
using UnityEngine.UI;

public class LobbyManager : MonoBehaviourPunCallbacks
{
    public Dropdown GameModeSelect;
    public Button GameJoinBt;
    public Text GameJoinBt_Tx;

    public Button GameCancelBt;
    public Text GameCancelBt_Tx;

    public Button QuickStartBt;

    public GameObject MatchedPn;
    public int NowGameMode = 0; // 0 : ���� 1 : ������


    public PhotonView pv;
    public void GameModeChange()
    {
        NowGameMode = GameModeSelect.value;
        if(NowGameMode == 0)
        {
            GameJoinBt_Tx.text = "�÷���\n����";
        }
        else if(NowGameMode == 1)
        {
            GameJoinBt_Tx.text = "�÷���\n������";
        }
    }

    public void GameJoin()
    {
        if (NowGameMode == 0)
        {
            GameModeSelect.interactable = false;
            GameJoinBt.gameObject.SetActive(false);
            GameCancelBt.gameObject.SetActive(true);
            GameCancelBt.interactable = false;

            GameCancelBt_Tx.text = "���� ã����\n(���)";

            PhotonNetwork.JoinRandomRoom();
            
        }
    }
    public void GameCancel()
    {
        PhotonNetwork.LeaveRoom();
        GameCancelBt.interactable = false;
        QuickStartBt.gameObject.SetActive(false);
    }
    public override void OnJoinRandomFailed(short returnCode, string message)
    {
        PhotonNetwork.CreateRoom("������" + Random.Range(0,1000), new RoomOptions { MaxPlayers = 10 ,PublishUserId = true});
    }
    public override void OnCreateRoomFailed(short returnCode, string message)
    {
        PhotonNetwork.CreateRoom("������" + Random.Range(0, 1000), new RoomOptions { MaxPlayers = 10 , PublishUserId = true });
    }
    public override void OnJoinedRoom()
    {
        GameCancelBt.interactable = true;
        if (PhotonNetwork.IsMasterClient)
        {
            QuickStartBt.gameObject.SetActive(true);
        }
        
        GameCancelBt_Tx.text = "���� ã���� " + PhotonNetwork.CurrentRoom.PlayerCount +" / 10"+"\n(���)";
    }
    public override void OnPlayerLeftRoom(Player otherPlayer)
    {
        if (PhotonNetwork.IsMasterClient)
        {
            QuickStartBt.gameObject.SetActive(true);
        }

        GameCancelBt_Tx.text = "���� ã���� " + PhotonNetwork.CurrentRoom.PlayerCount + " / 10" + "\n(���)";
    }
    public override void OnPlayerEnteredRoom(Player newPlayer)
    {
        GameCancelBt_Tx.text = "���� ã���� " + PhotonNetwork.CurrentRoom.PlayerCount + " / 10" + "\n(���)";
    }
    public override void OnLeftRoom()
    {
        GameModeSelect.interactable = true;
        GameJoinBt.gameObject.SetActive(true);
        GameCancelBt.gameObject.SetActive(false);
        
    }

    public void QuickStart()
    {
        if (PhotonNetwork.IsMasterClient)
        {
            pv.RPC("OnMatched", RpcTarget.AllBuffered);
            PhotonNetwork.CurrentRoom.IsVisible = false;
            Invoke("ToMap", 3);
        }
    }

    [PunRPC]
    private void OnMatched()
    {
        MatchedPn.SetActive(true);
    }


    [PunRPC]
    private void ToMap_RPC()
    {
        PhotonNetwork.LoadLevel("TestMap1");
    }
    public void ToMap()
    {
        pv.RPC("ToMap_RPC", RpcTarget.AllBuffered);
    }
}
