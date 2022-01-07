using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PauseGame : MonoBehaviour
{
    public static bool isGamePause = false;
    [SerializeField] GameObject pauseMenu;

    void Update()
    {
        if(Input.GetKeyDown(KeyCode.Escape)) {
            if(isGamePause) {
                Resume();
            } else {
                Pause();
            }
        }
    }

    public void Pause() {
        pauseMenu.SetActive(true);
        Time.timeScale = 0f;
        isGamePause = true;
    }

    public void Resume() {
        pauseMenu.SetActive(false);
        Time.timeScale = 1f;
        isGamePause = false;
    }

    public void BackToMenu() {
        SceneManager.LoadScene(0);
    }

    public void ExitGame() {
        Application.Quit();
    }
}
