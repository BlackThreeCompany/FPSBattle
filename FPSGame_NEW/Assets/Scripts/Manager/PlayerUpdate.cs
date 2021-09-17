using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerUpdate : MonoBehaviour
{
    public GameObject[] TeamA_UIState = new GameObject[6];
    public GameObject[] TeamB_UIState = new GameObject[6];


    // Update is called once per frame
    void Update()
    {
        if (RGame.instance.isPunReceived == false) return;

        //PlayerListUpdate();
        UISTATEUpdate();
    }

    void PlayerListUpdate()
    {
        for (int i = 0; i < 5; i++)
        {
            if (RGame.instance.TeamPlayer_A[i] == null)
            {
                if(RGame.instance.TeamPlayerState_A[i] == 1)
                {
                    RGame.instance.TeamPlayerState_A[i] = 3;
                }
            }

            if (RGame.instance.TeamPlayer_B[i] == null)
            {
                if (RGame.instance.TeamPlayerState_B[i] == 1)
                {
                    RGame.instance.TeamPlayerState_B[i] = 3;
                }
            }

        }
           
    }
    void UISTATEUpdate()
    {
        for (int i = 0; i < 5; i++)
        {
            if (RGame.instance.TeamPlayerState_A[i] == 1)
            {
                TeamA_UIState[i].SetActive(true);
            }
            else
            {
                TeamA_UIState[i].SetActive(false);
            }
        }



        for (int i = 0; i < 5; i++)
        {
            if (RGame.instance.TeamPlayerState_B[i] == 1)
            {
                TeamB_UIState[i].SetActive(true);
            }
            else
            {
                TeamB_UIState[i].SetActive(false);
            }
        }
    }
}
