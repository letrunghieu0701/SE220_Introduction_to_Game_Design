using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FallRock : MonoBehaviour
{
    [Header("FallRock Attributes")]
    [SerializeField] private float speed;
    [SerializeField] private float range = 5f;
    [SerializeField] private float delayFallingTime = 2f;
    [SerializeField] private LayerMask playerMask;
    private Vector3 defaultPosition;
    private Vector3 direction = new Vector3();

    private float checkTimer;
    
    private bool backToDefaultPos;

    private void Awake() {
        defaultPosition = transform.position;
        direction = -transform.up * range;
    }

    private void Update() {
        checkTimer += Time.deltaTime;
        if(backToDefaultPos == false && checkTimer > delayFallingTime) {
            transform.Translate(direction * Time.deltaTime * speed);
        }

        if(transform.position == defaultPosition) {
            backToDefaultPos = false;
        }

        if(backToDefaultPos == true) {
            transform.position = defaultPosition;
            checkTimer = 0;
        }
    }

    private void CheckForPlayer() {
        
    }

    private void OnTriggerEnter2D(Collider2D col) {
        Stop();
    }

    private void Stop() {
        backToDefaultPos = true;
    }

    private void OnEnable() {
        Stop();
    }
}
