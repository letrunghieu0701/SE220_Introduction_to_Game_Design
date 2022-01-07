using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SwitchLevel : MonoBehaviour
{
    [SerializeField] private string nextLevelToLoad;
    [SerializeField] private Vector2 nextLevelPlayerPosition;

    private LevelManager levelManager;

    private void Start() 
    {
        levelManager = GameObject.FindGameObjectWithTag("LevelManager").GetComponent<LevelManager>();
    }

    private void OnTriggerEnter2D(Collider2D collider) 
    {
        if (collider.CompareTag("Player"))
        {
            levelManager.LoadNextLevel(nextLevelPlayerPosition, nextLevelToLoad);
        }
    }

    private void Update() {
        if(Input.GetKeyDown(KeyCode.L)) {
            levelManager.LoadNextLevel(nextLevelPlayerPosition, nextLevelToLoad);
        }
    }
}
