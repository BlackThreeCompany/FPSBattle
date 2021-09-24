using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using Photon.Realtime;
using UnityEngine.UI;

using Hashtable = ExitGames.Client.Photon.Hashtable;
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

    RoomOptions roomOptions = new RoomOptions();




    public GameObject[] Spawnpos;
    public Player[] TeamPlayer_A = new Player[12];
    public Player[] TeamPlayer_B = new Player[12];

    public string[] TeamPlayerToID_A = new string[12];
    public string[] TeamPlayerToID_B = new string[12];

    public int[] TeamPlayerState_A = new int[12]; // 0 : ó������ ����(��������) 1:���� 2:���� 3:����
    public int[] TeamPlayerState_B = new int[12];
    
    
    public bool[] PlayerFirstCheck;
    public int[] TeamABIdx;
    public int NowPlayerCnt;


    private void Awake()
    {
        if (PhotonNetwork.InRoom)
        {
            PhotonNetwork.LeaveRoom();
        }
    }










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
        roomOptions.MaxPlayers = 10;
        roomOptions.PublishUserId = true;
        roomOptions.CustomRoomProperties = new Hashtable()
        {
            {"RedScore", 0 },
            {"BlueScore", 0 },
            {"RedSafe", 0 },
            {"BlueSafe", 0 }
        };


        PhotonNetwork.CreateRoom("������" + Random.Range(0,1000), roomOptions);
    }
    public override void OnCreateRoomFailed(short returnCode, string message)
    {
        roomOptions.MaxPlayers = 10;
        roomOptions.PublishUserId = true;
        roomOptions.CustomRoomProperties = new Hashtable()
        {
            {"RedScore", 0 },
            {"BlueScore", 0 },
            {"RedSafe", 0 },
            {"BlueSafe", 0 }
        };


        PhotonNetwork.CreateRoom("������" + Random.Range(0, 1000), roomOptions);
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
        QuickStartBt.gameObject.SetActive(false);
    }

    public void QuickStart()
    {
        if (PhotonNetwork.IsMasterClient)
        {
            pv.RPC("OnMatched", RpcTarget.AllBuffered);
            PhotonNetwork.CurrentRoom.IsVisible = false;
            
        }
    }

    [PunRPC]
    private void OnMatched()
    {
        PhotonNetwork.AutomaticallySyncScene = true;
        MatchedPn.SetActive(true);

        Invoke("ToMap", 3);
    }


    [PunRPC]
    private void ToMap_RPC()
    {
        PhotonNetwork.LoadLevel("TestMap1");
    }
    public void ToMap()
    {
        if (PhotonNetwork.IsMasterClient)
        {

            // �� ���ϱ� �ؾ���
            SelectTeam();
            
            PhotonNetwork.LoadLevel("TestMap1");
        }
        else
        {
            Invoke("ToMap", 1);
        }
    }












    public void SelectTeam()
    {
        NowPlayerCnt = PhotonNetwork.CurrentRoom.PlayerCount;
        int Cnt = 0;
        int RandomIdx = 0;
        while (true)
        {
            if (Cnt == NowPlayerCnt)
            {
                break;
            }

            RandomIdx = Random.Range(0, NowPlayerCnt);
            Debug.Log("����:" + RandomIdx);
            if (Cnt % 2 == 0)
            {

                if (PlayerFirstCheck[RandomIdx] == false)
                {

                    PlayerFirstCheck[RandomIdx] = true;

                    TeamPlayer_A[TeamABIdx[0]] = PhotonNetwork.PlayerList[RandomIdx];
                    TeamPlayerToID_A[TeamABIdx[0]] = PhotonNetwork.PlayerList[RandomIdx].UserId;
                    TeamPlayerState_A[TeamABIdx[0]] = 1;
                    Debug.Log("MASDTERID1 : " + TeamPlayerToID_A[TeamABIdx[0]]);

                    
                    Debug.Log("MASDTERID : " + TeamPlayerToID_A[TeamABIdx[0]]);
                    TeamABIdx[0]++;
                    Debug.Log("A:" + TeamABIdx[0].ToString());
                    Cnt++;




                }
            }
            else
            {

                if (PlayerFirstCheck[RandomIdx] == false)
                {
                    PlayerFirstCheck[RandomIdx] = true;
                    TeamPlayer_B[TeamABIdx[1]] = PhotonNetwork.PlayerList[RandomIdx];
                    TeamPlayerToID_B[TeamABIdx[1]] = PhotonNetwork.PlayerList[RandomIdx].UserId;
                    TeamPlayerState_B[TeamABIdx[1]] = 1;
                    Debug.Log("MASDTERID1 : " + TeamPlayerToID_B[TeamABIdx[1]]);

                    
                    Debug.Log("MASDTERID : " + TeamPlayerToID_B[TeamABIdx[1]]);
                    TeamABIdx[1]++;
                    Debug.Log("B:" + TeamABIdx[1].ToString());
                    Cnt++;
                }
            }




        }

    }

}
