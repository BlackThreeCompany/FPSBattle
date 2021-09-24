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
    public Text ArmorTx;

    public GameObject Aim;
    public GameObject ScopeAim;

    bool isAim;

    public GameObject WeaponCamera;
    public PhotonView pv;
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
            if(WeaponManager.instance.isHasArmor)
            {
                ArmorTx.text = "AP : " + StatManager.instance.AP;
            }
            else
            {
                ArmorTx.text = "";
            }
        }
    }
    public void KillLog(int From_id,int To_id, int Damage, int Killtype) // Killtype 1-shoot 2-damage 3-else
    {                                                                   //Grenade 4-to 5-damage 6-else
        LogCount += 1;
        if (Killtype == 1)
        {
            KillLogTx.color = Color.blue;
            KillLogTx.text = "[GUN] "+ "[ Damage : " + Damage + " ] " + From_id + " -> " + To_id + " ( " + LogCount + " )  ";
        }
        else if (Killtype == 2)
        {
            KillLogTx.color = Color.red;

            if (WeaponManager.instance.isHasArmor)
            {
                StatManager.instance.HP -= (int)(Damage * 0.4);
                StatManager.instance.AP -= (int)(Damage * 0.6);
            }
            else StatManager.instance.HP -= Damage;

            KillLogTx.text = "[GUN] " + "[ Damage : " + Damage + " ] " + From_id + " -> " + To_id + " ( " + LogCount + " )  ";

            if (StatManager.instance.HP <= 0)
            {
                StatManager.instance.HP = 0;

                pv.RPC("R_PLAYERKILLED", RpcTarget.AllBuffered, From_id, To_id,1,RGame.instance.MYTEAM,RGame.instance.MYTEAM_IDX);
            }
        }
        else if (Killtype == 3)
        {
            KillLogTx.color = Color.gray;
            KillLogTx.text = "[GUN] " + "[ Damage : " + Damage + " ] " + From_id + " -> " + To_id + " ( " + LogCount + " )  ";
        }
        else if (Killtype == 4)
        {
            KillLogTx.color = Color.blue;
            KillLogTx.text = "[Grenade] " + "[ Damage : " + Damage + " ] " + From_id + " -> " + To_id + " ( " + LogCount + " )  ";
        }
        else if (Killtype == 5)
        {
            KillLogTx.color = Color.red;

            if (WeaponManager.instance.isHasArmor)
            {
                StatManager.instance.HP -= (int)(Damage * 0.4);
                StatManager.instance.AP -= (int)(Damage * 0.6);
            }
            else StatManager.instance.HP -= Damage;

            KillLogTx.text = "[Grenade] " + "[ Damage : " + Damage + " ] " + From_id + " -> " + To_id + " ( " + LogCount + " )  ";

            if (StatManager.instance.HP <= 0)
            {
                StatManager.instance.HP = 0;

                pv.RPC("R_PLAYERKILLED", RpcTarget.AllBuffered, From_id, To_id, 2, RGame.instance.MYTEAM, RGame.instance.MYTEAM_IDX);
            }


        }
        else if (Killtype == 6)
        {
            KillLogTx.color = Color.gray;
            KillLogTx.text = "[Grenade] " + "[ Damage : " + Damage + " ] " + From_id + " -> " + To_id + " ( " + LogCount + " )  ";
        }

    }
    
    public void R_KilledBY(int KillMeID,int KillType) 
    {
       
    }

    [PunRPC]
    private void R_PLAYERKILLED(int TOKILLID, int KilledID, int KillType, int KilledPL_Team, int KilledPL_T_Idx)// 1-shoot 2-Grenade
    {
        Player KilledPlayer = PhotonView.Find(KilledID).Owner;
        Player ToKillPlayer = PhotonView.Find(TOKILLID).Owner;

        if(KilledPL_Team == 0)
        {
            RGame.instance.TeamPlayerState_A[KilledPL_T_Idx] = 2;
        }
        else
        {
            RGame.instance.TeamPlayerState_B[KilledPL_T_Idx] = 2;
        }
        
    }

    public void AimDisable()
    {
        Aim.SetActive(false);
    }

    public void AimEnable()
    {
        Aim.SetActive(true);
    }

    IEnumerator OnScope()
    {
        yield return new WaitForSecondsRealtime(0.15f);

        AimDisable();
        WeaponCamera.SetActive(false);
        ScopeAim.SetActive(true);
    }

    public void ScopeAimEnable()
    {
        StartCoroutine(OnScope());
    }

    public void ScopeAimDisable()
    {
        AimEnable();
        WeaponCamera.SetActive(true);
        ScopeAim.SetActive(false);
    }
}
