using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using Photon.Realtime;
using UnityEngine.UI;

public class RGame : MonoBehaviourPunCallbacks
{
    public GameObject[] Spawnpos;
    public Player[] TeamPlayer_A = new Player[12];
    public Player[] TeamPlayer_B = new Player[12];

    public string[] TeamPlayerToID_A = new string[12];
    public string[] TeamPlayerToID_B = new string[12];

    public int[] TeamPlayerState_A = new int[12]; // 0 : 처음부터 없음(삭제예정) 1:생존 2:죽음 3:나감
    public int[] TeamPlayerState_B = new int[12];

    public bool[] PlayerFirstCheck;
    public int[] TeamABIdx;
    public int NowPlayerCnt;
    public bool isPunReceived = false;

    public Text TeamLogTx;
    public int MYTEAM; // 0:A 1:B

    public PhotonView pv;

    // Start is called before the first frame update
    void Start()
    {
        if (pv.IsMine)
        {
            SelectTeam();
            Debug.Log(")))))");
        }
        else
        {
            Debug.Log("아님");
        }
    }
    private void Update()
    {
        if (!isPunReceived)
        {
            return;
        }

        PlayerUpdate();
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
            Debug.Log("랜덤:"+RandomIdx);
            if (Cnt%2 == 0)
            {
                
                if (PlayerFirstCheck[RandomIdx] == false)
                {
                    
                    PlayerFirstCheck[RandomIdx] = true;
                   
                    TeamPlayer_A[TeamABIdx[0]] = PhotonNetwork.PlayerList[RandomIdx];
                    TeamPlayerToID_A[TeamABIdx[0]] = PhotonNetwork.PlayerList[RandomIdx].UserId;
                    TeamPlayerState_A[TeamABIdx[0]] = 1;
                    Debug.Log("MASDTERID1 : " + TeamPlayerToID_A[TeamABIdx[0]]);

                    pv.RPC("TeamSelectSend", RpcTarget.AllBuffered, 0, TeamPlayerToID_A[TeamABIdx[0]], TeamABIdx[0]);
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

                    pv.RPC("TeamSelectSend", RpcTarget.AllBuffered, 1, TeamPlayerToID_B[TeamABIdx[1]], TeamABIdx[1]);
                    Debug.Log("MASDTERID : " + TeamPlayerToID_B[TeamABIdx[1]]);
                    TeamABIdx[1]++;
                    Debug.Log("B:" + TeamABIdx[1].ToString());
                    Cnt++;
                }
            }

            
            

        }

        pv.RPC("TeamSelectFinished",RpcTarget.AllBuffered);


    }

    [PunRPC]
    private void TeamSelectFinished()
    {
        
        Debug.Log("!!");
        for (int i = 0; i < PhotonNetwork.CurrentRoom.PlayerCount; i++)
        {
            if (i % 2 == 0)
            {
                if (TeamPlayer_A[i/2] == null) continue;
                Debug.Log("A : " + TeamPlayer_A[i/2].NickName);

            }
            else
            {
                if (TeamPlayer_B[ i / 2] == null) continue;
                Debug.Log("B : " + TeamPlayer_B[i / 2].NickName);


            }
        }

        isPunReceived = true;
    }

    [PunRPC]
    private void TeamSelectSend(int Team_RPC,string Team_ID_RPC,int Idx_RPC)
    {
        Debug.Log("정보받음" + Team_RPC + Team_ID_RPC + Idx_RPC);



        if(Team_RPC == 0)
        {
            TeamPlayerToID_A[Idx_RPC] = Team_ID_RPC;
            for (int i = 0; i < PhotonNetwork.CurrentRoom.PlayerCount; i++)
            {
                if (Team_ID_RPC == PhotonNetwork.PlayerList[i].UserId)
                {
                    TeamPlayer_A[Idx_RPC] = PhotonNetwork.PlayerList[i];
                    TeamPlayerState_A[Idx_RPC] = 1;
                    TeamABIdx[Team_RPC] = Idx_RPC;
                    if (Team_ID_RPC == PhotonNetwork.LocalPlayer.UserId)
                    {
                        MYTEAM = Team_RPC;
                        if (Team_RPC == 0) TeamLogTx.text = "Team : A";
                        else TeamLogTx.text = "Team : B";
                    }
                }
            }

            Debug.Log("끝");
            TeamPlayerState_A[Idx_RPC] = 1;
        }
        else
        {
            TeamPlayerToID_B[Idx_RPC] = Team_ID_RPC;
            for (int i = 0; i < PhotonNetwork.CurrentRoom.PlayerCount; i++)
            {
                if (Team_ID_RPC == PhotonNetwork.PlayerList[i].UserId)
                {
                    TeamPlayer_B[Idx_RPC] = PhotonNetwork.PlayerList[i];
                    TeamPlayerState_B[Idx_RPC] = 1;
                    TeamABIdx[Team_RPC] = Idx_RPC;
                    if (Team_ID_RPC == PhotonNetwork.LocalPlayer.UserId)
                    {
                        MYTEAM = Team_RPC;
                        if (Team_RPC == 0) TeamLogTx.text = "Team : A";
                        else TeamLogTx.text = "Team : B";
                    }
                }
            }

            Debug.Log("끝");
            TeamPlayerState_B[Idx_RPC] = 1;
        }
    }
    public void PlayerUpdate()
    {
        NowPlayerCnt = PhotonNetwork.CurrentRoom.PlayerCount;
        
    }

    public override void OnPlayerLeftRoom(Player otherPlayer)
    {
        
    }
}
