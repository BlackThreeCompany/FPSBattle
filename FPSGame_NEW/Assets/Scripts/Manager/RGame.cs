using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using Photon.Realtime;
public class RGame : MonoBehaviourPunCallbacks
{
    public GameObject[] Spawnpos;
    public Player[,] TeamPlayer = new Player[2,12];
    public string[,] TeamPlayerToID = new string[2, 12];

    public int[,] TeamPlayerState = new int[2,12]; // 0 : 처음부터 없음(삭제예정) 1:생존 2:죽음 3:나감
    
    public bool[] PlayerFirstCheck;
    public int[] TeamABIdx;
    public int NowPlayerCnt;
    public bool isPunReceived = false;


    public PhotonView pv;

    // Start is called before the first frame update
    void Start()
    {
        if (pv.IsMine)
        {
            //SelectTeam();
            Debug.Log(")))))");
        }
    }
    private void Update()
    {
        if (!isPunReceived) return;

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

            RandomIdx = Random.Range(0, NowPlayerCnt-1);
            Debug.Log(RandomIdx);
            if (Cnt%2 == 0)
            {
                if (PlayerFirstCheck[RandomIdx] == false)
                {
                    PlayerFirstCheck[RandomIdx] = true;
                   
                    TeamPlayer[0, TeamABIdx[0]] = PhotonNetwork.PlayerList[RandomIdx];
                    TeamPlayerToID[0,TeamABIdx[0]] = PhotonNetwork.PlayerList[RandomIdx].UserId;
                    TeamPlayerState[0, TeamABIdx[0]] = 1;


                    //pv.RPC("TeamSelectSend", RpcTarget.AllBuffered, 0, TeamPlayerToID[0, TeamABIdx[0]], TeamABIdx[0]);

                    TeamABIdx[0]++;
                    Cnt++;




                }
            }
            else
            {
                if (PlayerFirstCheck[RandomIdx] == false)
                {
                    PlayerFirstCheck[RandomIdx] = true;
                    TeamPlayer[1, TeamABIdx[1]] = PhotonNetwork.PlayerList[RandomIdx];
                    TeamPlayerToID[1, TeamABIdx[1]] = PhotonNetwork.PlayerList[RandomIdx].UserId;
                    TeamPlayerState[1, TeamABIdx[1]] = 1;


                    //pv.RPC("TeamSelectSend", RpcTarget.AllBuffered, 1, TeamPlayerToID[1, TeamABIdx[1]], TeamABIdx[1]);

                    TeamABIdx[1]++;
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
                Debug.Log("0 : " + TeamPlayer[0, i].NickName);
            }
            else
            {
                Debug.Log("1 : " + TeamPlayer[1, i].NickName);
            }
        }
    }

    [PunRPC]
    private void TeamSelectSend(int Team_RPC,string Team_ID_RPC,int Idx_RPC)
    {
        TeamPlayerToID[Team_RPC, Idx_RPC] = Team_ID_RPC;
        for(int i = 0; i < PhotonNetwork.CurrentRoom.PlayerCount; i++)
        {
            if(Team_ID_RPC == PhotonNetwork.PlayerList[i].UserId)
            {
                TeamPlayer[Team_RPC, Idx_RPC] = PhotonNetwork.PlayerList[i];
            }
        }
        
        
        TeamPlayerState[0, Idx_RPC] = 1;
    }
    public void PlayerUpdate()
    {
        NowPlayerCnt = PhotonNetwork.CurrentRoom.PlayerCount;
        
    }

    public override void OnPlayerLeftRoom(Player otherPlayer)
    {
        
    }
}
