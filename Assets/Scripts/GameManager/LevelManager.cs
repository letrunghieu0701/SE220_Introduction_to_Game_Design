using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class LevelManager : MonoBehaviour
{
    [SerializeField] public Vector2 beginningPoint;
    [SerializeField] public Vector2 respawnPoint;
    [SerializeField] private Animator sceneTransAnimator;
    [SerializeField] private int beginningLifeCount;
    [SerializeField] public int lifeCount;
    [SerializeField] public int gemAmount;
 

    public static LevelManager instance;
    private bool respawning = true;
    private string levelToLoad;

    private void Awake() {
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(instance);
        }
        else
        {
            Destroy(gameObject);
        }
    }

    public void UpdateCheckPoint(Vector2 _checkPointPos, int _lifeCount, int _gemAmount)
    {
        respawnPoint = _checkPointPos;
        lifeCount = _lifeCount;
        gemAmount = _gemAmount;
    }

    public void Respawn() 
    {
        sceneTransAnimator.SetTrigger("loadNextScene");
        respawning = true;
    }

    public void ReLevel() 
    {
        sceneTransAnimator.SetTrigger("loadNextScene");
        respawnPoint = beginningPoint;
        lifeCount = beginningLifeCount;
        gemAmount = 0;
        respawning = true;
    }

    public void LoadNextLevel(Vector2 nextLevelPlayerPosition, string nextLevelToLoad)
    {
        respawnPoint = nextLevelPlayerPosition;
        beginningPoint = nextLevelPlayerPosition;
        lifeCount = beginningLifeCount;
        gemAmount = 0;
        levelToLoad = nextLevelToLoad;

        sceneTransAnimator.SetTrigger("loadNextScene");
        respawning = false;
        HideIntro.hideText = false;
    }

    private void LoadScene()
    {
        if (respawning)
        {
            SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
        }
        else
        {
            SceneManager.LoadScene(levelToLoad);
        }
    }
}
