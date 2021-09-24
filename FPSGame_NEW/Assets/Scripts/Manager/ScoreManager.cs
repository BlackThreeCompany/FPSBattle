using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Photon.Pun;
using Photon.Realtime;
using Hashtable = ExitGames.Client.Photon.Hashtable;

public class ScoreManager : MonoBehaviourPunCallbacks
{
    public Text RED_SAFE;
    public Text BLUE_SAFE;

    public Text RED_SCORE;
    public Text BLUE_SCORE;

    public int GameWin = 0; //1:RED 2:BLUE
    Hashtable CP = new Hashtable();
    private void Awake()
    {
        CP = PhotonNetwork.CurrentRoom.CustomProperties;
        RED_SAFE.text = CP["RedSafe"].ToString();
        BLUE_SAFE.text = CP["BlueSafe"].ToString();

        RED_SCORE.text = CP["RedScore"].ToString();
        BLUE_SCORE.text = CP["BlueScore"].ToString();

        if (int.Parse( CP["RedScore"].ToString() )>= 5)
        {
            GameWin = 1;
            PhotonNetwork.AutomaticallySyncScene = false;

            Invoke("GOLOBBYSCENE", 5);
        }
        else if (int.Parse(CP["BlueScore"].ToString()) >= 5)
        {
            GameWin = 2;
            PhotonNetwork.AutomaticallySyncScene = false;

            Invoke("GOLOBBYSCENE", 5);
        }
        else
        {
            Invoke("GOGAMESCENE", 5);
        }

        

        
        
    }

    public void GOGAMESCENE()
    { 
        if(PhotonNetwork.IsMasterClient)
            PhotonNetwork.LoadLevel("TestMap1");
        else
        {
            Invoke("GOGAMESCENE",1);
        }
    }

    public void GOLOBBYSCENE()
    {
        PhotonNetwork.LoadLevel("Lobby");
    }

}
