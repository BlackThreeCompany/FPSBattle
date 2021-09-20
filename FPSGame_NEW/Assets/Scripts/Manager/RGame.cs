using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using Photon.Realtime;
using UnityEngine.UI;
using Hashtable = ExitGames.Client.Photon.Hashtable;
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
    

    public int MYTEAM_IDX;
    public int MYTEAM; // 0:A 1:B
    public int spawnA, spawnB;

    public GameObject[] SpawnPos_CHD = new GameObject[6];
    public Transform[] SpawnPos_Trans = new Transform[6];

    public PhotonView pv;

    public Text GameTimeTx;
    public bool IsTimesend = false;
    public double NowGameTime;
    public double StartGameTime;
    public double GameTime;
    public double NowGameTimer;
    public int sec;
    public float secelse;

    public int RoundEnd = 0;

    public static RGame instance;


    public int GameOver_SendCnt = 0;
    public int GameOver_A_Safe = 0;
    public int GameOver_B_Safe = 0;

    public bool ISGameOver_OKSend = false;
    public int OK_Cnt = 0;
    // Start is called before the first frame update


    // -----------------------------------------------
    //시작, 모든 플레이어 STATE Manager-------------------------------------------
    //-----------------------------------------------


    private void Awake()
    {
        instance = this;
    }
    void Start()
    {
        Random.seed = System.DateTime.Now.Millisecond;

        if (pv.IsMine)
        {
            SelectTeam();
            SelectSpawn();

            Invoke("EndTimeSet_M",Random.Range(5.0f,10.0f));
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

        if (RoundEnd == 1)
        {
            if(GameOver_SendCnt == PhotonNetwork.CurrentRoom.PlayerCount && !ISGameOver_OKSend)
            {
                pv.RPC("RoundOver_OK", RpcTarget.AllBuffered);
                ISGameOver_OKSend = true;
            }
            if (PhotonNetwork.IsMasterClient)
            {
                if(OK_Cnt == PhotonNetwork.CurrentRoom.PlayerCount)
                {
                    Hashtable CP = PhotonNetwork.CurrentRoom.CustomProperties;
                    CP["RedSafe"] = GameOver_A_Safe;
                    CP["BlueSafe"] = GameOver_B_Safe;

                    CP["RedScore"] = int.Parse(CP["RedScore"].ToString()) + GameOver_A_Safe;
                    CP["BlueScore"] = int.Parse(CP["BlueScore"].ToString()) + GameOver_B_Safe;
                    Invoke("TOSCORESCENE", 5f);
                }
            }
        }

        if (IsTimesend)
        {
            if (RoundEnd == 1) return;
            TimeUpdate();
        }

        
        //PlayerUpdate();
    }

    public void TOSCORESCENE()
    {
        pv.RPC("TOSCORESCENE_RPC", RpcTarget.AllBuffered);
    }
    [PunRPC]
    private void TOSCORESCENE_RPC()
    {
        PhotonNetwork.LoadLevel("ScoreScene");
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
                        MYTEAM_IDX = Idx_RPC;
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
                        MYTEAM_IDX = Idx_RPC;
                        if (Team_RPC == 0) TeamLogTx.text = "Team : A";
                        else TeamLogTx.text = "Team : B";
                    }
                }
            }

            Debug.Log("끝");
            TeamPlayerState_B[Idx_RPC] = 1;
        }
    }




    public void SelectSpawn()
    {
        

        spawnA = Random.Range(0, Spawnpos.Length);
        while (true)
        {
            spawnB = Random.Range(0, Spawnpos.Length);
            Debug.Log("spawnB : " + spawnB);
            if (spawnA != spawnB) break;
        }

        pv.RPC("TeamSpawnPosSend", RpcTarget.AllBuffered, spawnA, spawnB);
    }


    [PunRPC]
    private void TeamSpawnPosSend(int spawnA_RPC, int spawnB_RPC)
    {
        


        if(MYTEAM == 0)
        {
            SpawnPos_Trans = Spawnpos[spawnA_RPC].GetComponentsInChildren<Transform>();
            for (int i = 0; i < 5; i++)
            {
                SpawnPos_CHD[i] = SpawnPos_Trans[i+1].gameObject;
            }

            NetWorkManager.instance.Finished_PlayerSpawn(SpawnPos_CHD[MYTEAM_IDX].transform.position, Spawnpos[spawnA_RPC].transform.rotation);
        }
        else
        {
            SpawnPos_Trans = Spawnpos[spawnB_RPC].GetComponentsInChildren<Transform>();
            for (int i = 0; i < 5; i++)
            {
                SpawnPos_CHD[i] = SpawnPos_Trans[i + 1].gameObject;
            }

            NetWorkManager.instance.Finished_PlayerSpawn(SpawnPos_CHD[MYTEAM_IDX].transform.position, Spawnpos[spawnB_RPC].transform.rotation);
        }


        isPunReceived = true;
    }







    public void PlayerUpdate()
    {
        NowPlayerCnt = PhotonNetwork.CurrentRoom.PlayerCount;
        
    }

    public override void OnPlayerLeftRoom(Player otherPlayer)
    {
        for(int i = 0; i < 5; i++)
        {
            if(TeamPlayer_A[i] == otherPlayer)
            {
                TeamPlayerState_A[i] = 3;
            }

            if (TeamPlayer_B[i] == otherPlayer)
            {
                TeamPlayerState_B[i] = 3;
            }
        }
    }

    public void EndTimeSet_M()
    {
        pv.RPC("SendGameTime", RpcTarget.AllBuffered, PhotonNetwork.Time, Random.Range(30.0f, 120.0f));
    }
    [PunRPC]
    private void SendGameTime(double StartTime_rpc,float GameTime_rpc)
    {
        StartGameTime = StartTime_rpc;
        GameTime = GameTime_rpc;
        
        IsTimesend = true;
    }

    public void TimeUpdate()
    {
        NowGameTime = PhotonNetwork.Time;
        NowGameTimer = StartGameTime + GameTime - NowGameTime;

        sec = Mathf.FloorToInt((float)NowGameTimer);
        secelse = (float)(NowGameTimer - sec);
        if(NowGameTimer < 0)
        {
            GameTimeTx.text = "0 . 000";
            RoundEnd = 1;
            PlayerSafeAreacheck();
            return;
        }

        if(sec < 10)
        {
            GameTimeTx.text = sec + " . " + Mathf.FloorToInt(secelse * 1000);
        }
        else
        {
            GameTimeTx.text = sec + " . " + Mathf.FloorToInt(secelse * 10);
        }
        
    }

    public void PlayerSafeAreacheck()
    {
        if(StatManager.instance.isInSafeArea == false)
        {
            StatManager.instance.HP = 0;
            pv.RPC("PlayerKilledORSafe_TimesUP",RpcTarget.AllBuffered, MYTEAM, MYTEAM_IDX,1); // 죽음
        }
        else
        {
            pv.RPC("PlayerKilledORSafe_TimesUP", RpcTarget.AllBuffered, MYTEAM, MYTEAM_IDX, 0); //생존
        }
    }

    [PunRPC]
    private void PlayerKilled_TimesUP(int PL_Team,int PL_T_IDX, int SafeOrKilled)
    {
        if(SafeOrKilled == 1)
        {
            if (PL_Team == 0)
            {
                TeamPlayerState_A[PL_T_IDX] = 2;
            }
            else
            {
                TeamPlayerState_B[PL_T_IDX] = 2;
            }
        }
        else
        {
            if (PL_Team == 0)
            {
                GameOver_A_Safe++;
            }
            else
            {
                GameOver_B_Safe++;
            }
        }

        Debug.Log("!! " + PL_Team + "  " + PL_T_IDX + " 받음 ");

        GameOver_SendCnt++;

    }

    [PunRPC]
    private void RoundOver_OK()
    {
        OK_Cnt++;
    }
}
