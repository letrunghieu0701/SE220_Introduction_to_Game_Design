using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SwitchLevel : MonoBehaviour
{
    [SerializeField] private string nextLevelToLoad;
    [SerializeField] private float loadSceneDelay = 2.0f; 
    [SerializeField] private Vector2 nextLevelPlayerPosition;

    private LevelManager levelManager;

    private void Start() 
    {
        levelManager = GameObject.FindGameObjectWithTag("LevelManager").GetComponent<LevelManager>();
    }

    private void OnTriggerEnter2D(Collider2D collider) 
    {
        if (collider.gameObject.CompareTag("Player"))
        {
            Invoke("LoadNextLevel", loadSceneDelay);
        }
    }

    private void LoadNextLevel()
    {
        if (nextLevelToLoad != null && nextLevelToLoad != "")
        {
            levelManager.respawnPoint = nextLevelPlayerPosition;
            SceneManager.LoadScene(nextLevelToLoad);
        }
    }
}
