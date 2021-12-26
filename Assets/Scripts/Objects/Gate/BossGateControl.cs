using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossGateControl : MonoBehaviour
{
    [SerializeField] private GameObject bossBattleCheck;
    [SerializeField] private GameObject bossGates;

    public bool areBossGatesUpdated = false;

    public void CloseGate()
    {
        bossGates.transform.GetChild(0).gameObject.SetActive(true);
        areBossGatesUpdated = true;
    }

    public void OpenGate()
    {
        bossGates.transform.GetChild(1).gameObject.GetComponent<Animator>().enabled = true;
    }
}
