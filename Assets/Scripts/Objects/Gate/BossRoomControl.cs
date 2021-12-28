using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossRoomControl : MonoBehaviour
{
    [SerializeField] private GameObject bossGate;
    [SerializeField] private GameObject bossBattleCheck;

    private bool bossGateClosed = false;
    private bool bossGateOpened = false;

    private void FixedUpdate() 
    {
        if (!bossGateClosed && bossBattleCheck.GetComponent<BossBattleCheck>().bossBattleCheck)
        {
            bossGate.GetComponent<BossGateControl>().CloseGate();
            bossGateClosed = true;

            GetComponent<Monster>().battleStarted = true;
        }


        if (!bossGateOpened && GetComponent<Monster>().isDead)
        {
            bossGate.GetComponent<BossGateControl>().OpenGate();
            bossGateOpened = true;
        }
    }
}
