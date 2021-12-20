using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;
using UnityEngine.SceneManagement;

public class LevelManager : MonoBehaviour
{
    public static LevelManager instance;
    [SerializeField] private GameObject playerPrefab;
    [SerializeField] private CinemachineVirtualCameraBase cam;
    [SerializeField] private Transform respawnPoint;

    private void Awake() {
        instance = this;
    }

    public void Respawn() {
        GameObject player = Instantiate(playerPrefab, respawnPoint.position, Quaternion.identity);
        cam.Follow = player.transform;
    }

    // public void Restart() {
    //     //reset scene
    //     SceneManager.LoadScene("SampleSceneWithBoss");
    // }
}
