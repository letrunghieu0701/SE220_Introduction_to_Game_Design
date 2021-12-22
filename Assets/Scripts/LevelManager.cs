using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class LevelManager : MonoBehaviour
{
    public static LevelManager instance;
    [SerializeField] private GameObject playerPrefab;
    [SerializeField] private CinemachineVirtualCameraBase cam;
    [SerializeField] public Transform respawnPoint;
    [SerializeField] private Image currentHealthBar;
    static bool initialized { get; set; }

    private void Awake() {
        instance = this;
        // if(initialized) {
        //     Destroy(this.gameObject);
        // }
        // else {
        //     instance = this;
        //     GameObject.DontDestroyOnLoad(this.gameObject);
        //     initialized = true;
        // }
        
    }

    public void Respawn() {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
        GameObject player = Instantiate(playerPrefab, respawnPoint.position, Quaternion.identity);
        cam.Follow = player.transform;
        player.GetComponent<Health>().currentHealthBar = currentHealthBar;
    }
}
