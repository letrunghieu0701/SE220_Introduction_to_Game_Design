using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossGateControl : MonoBehaviour
{
    [SerializeField] private GameObject bossBattleCheck;
    [SerializeField] private GameObject bossGates;

    public bool areBossGatesUpdated = false;
    
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (!areBossGatesUpdated)
        {
            if (bossBattleCheck.GetComponent<BossBattleCheck>().bossBattleCheck)
            {
                bossGates.transform.GetChild(0).gameObject.SetActive(true);
                bossGates.transform.GetChild(1).gameObject.SetActive(true);

                areBossGatesUpdated = true;
            }
        }
        
    }
}
