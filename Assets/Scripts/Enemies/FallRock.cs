using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FallRock : MonoBehaviour
{
    [Header("FallRock Attributes")]
    [SerializeField] private float speed = 5f;
    [SerializeField] private float delayFallingTime = 5f;
    private Vector3 defaultPosition;
    private float checkTimer;
    private bool backToDefaultPos;

    private void Awake() {
        defaultPosition = transform.position;
    }

    private void Update() {
        checkTimer += Time.deltaTime;
        if(backToDefaultPos == false && checkTimer > delayFallingTime) {
            transform.Translate(-transform.up * Time.deltaTime * speed);
        }

        if(transform.position == defaultPosition) {
            backToDefaultPos = false;
        }

        if(backToDefaultPos == true) {
            transform.position = defaultPosition;
            checkTimer = 0;
        }
    }

    private void OnTriggerEnter2D(Collider2D col) {
        if(col.tag == "Ground") {
            Stop();
        }
    }

    private void Stop() {
        backToDefaultPos = true;
    }

    private void OnEnable() {
        Stop();
    }
}
