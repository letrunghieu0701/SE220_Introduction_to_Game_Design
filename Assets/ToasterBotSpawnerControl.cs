using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ToasterBotSpawnerControl : MonoBehaviour
{
    [SerializeField] private GameObject toasterBotShortLife;
    [SerializeField] private float spawnDelay = 3.0f;
    [SerializeField] private GameObject spawnPosition;
    [SerializeField] private float destroyBotDelay = 7.0f;
    [SerializeField] private bool facingRight = false;
    [SerializeField] private float spawningHorError = 1.0f;

    private State spawnState;
    private GameObject toasterBotInstance;
    private Animator animator;

    // Start is called before the first frame update
    void Start()
    {
        spawnState = new State("spawn toaster bot delay", spawnDelay);
        spawnState.ResetState();

        animator = GetComponent<Animator>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void FixedUpdate() 
    {
        spawnState.CountDown();
        if (spawnState.timeSup)
        {
            animator.SetTrigger("spawn");
            spawnState.ResetState();
        }
    }

    private void SpawnToasterBot()
    {
        
        if (facingRight)
        {   
            toasterBotInstance = Instantiate(toasterBotShortLife, spawnPosition.transform.position + new Vector3(spawningHorError, 0, 0), Quaternion.identity);
            toasterBotInstance.transform.localScale *= -1;
        }
        else
        {
            toasterBotInstance = Instantiate(toasterBotShortLife, spawnPosition.transform.position - new Vector3(spawningHorError, 0, 0), Quaternion.identity);
        }
        toasterBotInstance.GetComponent<Monster>().destroyObjAfterDieDelay = destroyBotDelay;
    }
}
