using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using Photon.Realtime;
using UnityEngine.UI;

public class GameManager : MonoBehaviourPunCallbacks
{
    public static GameManager instance;
    public int LogCount = 0;
    public Text KillLogTx;
    public Text HPTx;
    void Awake()
    {
        instance = this;
    }

    // Update is called once per frame
    void Update()
    {
        if (NetWorkManager.instance.isJoined)
        {
            HPTx.text = "HP : " + StatManager.instance.HP;
        }
    }
    public void KillLog(int From_id,int To_id, int Damage, int Killtype) // Killtype 1-shoot 2-damage 3-else
    {
        LogCount += 1;
        if (Killtype == 1)
        {
            KillLogTx.color = Color.blue;
        }
        else if (Killtype == 2)
        {
            KillLogTx.color = Color.red;

            StatManager.instance.HP -= Damage;
        }
        else if (Killtype == 3)
        {
            KillLogTx.color = Color.gray;
        }

        KillLogTx.text = "[ " + Damage + " ] " + From_id + " -> " + To_id + " ( " + LogCount + " )  ";
    }
    
}
