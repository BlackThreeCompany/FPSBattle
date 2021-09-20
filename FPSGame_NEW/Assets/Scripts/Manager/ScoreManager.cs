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

    Hashtable CP = new Hashtable();
    private void Awake()
    {
        CP = PhotonNetwork.CurrentRoom.CustomProperties;
        RED_SAFE.text = CP["RedSafe"].ToString();
        BLUE_SAFE.text = CP["BlueSafe"].ToString();

        RED_SCORE.text = CP["RedScore"].ToString();
        BLUE_SCORE.text = CP["BlueScore"].ToString();

        Invoke("GOGAMESCENE",5);
    }

    public void GOGAMESCENE()
    {
        PhotonNetwork.LoadLevel("TestMap1");
    }


}
