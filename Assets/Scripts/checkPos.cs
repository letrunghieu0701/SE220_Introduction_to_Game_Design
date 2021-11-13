using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class checkPos : MonoBehaviour
{
    float playerPosX;

    void Start()
    {
        
    }

    void Update()
    {
        this.playerPosX = transform.position.x;

        int enermyLocation = 19;
        int spawnLocation = 20;

        if(enermyLocation == spawnLocation ){ this.SpawnPos();}
        else { this.notSpawnPos(); }
    }

    void SpawnPos(){
        Debug.Log("SpawnPos");
    }

    void notSpawnPos(){
        Debug.Log("Not spawn");
    }
}
