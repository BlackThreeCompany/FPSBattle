using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using Photon.Realtime;
public class RGame : MonoBehaviourPunCallbacks
{
    public GameObject[] Spawnpos;
    public Player[,] TeamPlayer = new Player[2,12];
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
            SelectTeam();
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

            RandomIdx = Random.Range(1, NowPlayerCnt);
            Debug.Log(RandomIdx);
            if (Cnt%2 == 0)
            {
                if (PlayerFirstCheck[RandomIdx] == false)
                {
                    PlayerFirstCheck[RandomIdx] = true;
                    Debug.Log(PhotonNetwork.CurrentRoom.Players[RandomIdx].NickName);
                    //TeamPlayer[0, TeamABIdx[0]] = PhotonNetwork.PlayerList.;
                    TeamPlayerState[0, TeamABIdx[0]] = 1;
                    TeamABIdx[0]++;
                    Cnt++;
                }
            }
            else
            {
                if (PlayerFirstCheck[RandomIdx] == false)
                {
                    PlayerFirstCheck[RandomIdx] = true;
                    TeamPlayer[1, TeamABIdx[1]] = PhotonNetwork.CurrentRoom.Players[RandomIdx];
                    TeamPlayerState[1, TeamABIdx[1]] = 1;
                    TeamABIdx[1]++;
                    Cnt++;
                }
            }
            
        }

        pv.RPC("TeamSelectFinished",RpcTarget.AllBuffered ,TeamPlayer, TeamPlayerState);


    }

    [PunRPC]
    private void TeamSelectFinished(Player[,] ReceivedPlayer,int[,] ReceivedState)
    {
        for(int i = 0; i < 5; i++)
        {
            Debug.Log(ReceivedPlayer[0, i].UserId);
        }
        for (int i = 0; i < 5; i++)
        {
            Debug.Log(ReceivedPlayer[1, i].UserId);
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
