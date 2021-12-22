using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class LevelManager : MonoBehaviour
{
    [SerializeField] public Vector2 respawnPoint;
    public static LevelManager instance;

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

    public void Respawn() {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }
}
