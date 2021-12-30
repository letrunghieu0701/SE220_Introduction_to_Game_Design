using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RockHead : MonoBehaviour
{
    [Header("RockHead Attributes")]
    [SerializeField] private float speed;
    [SerializeField] private float range = 5f;
    [SerializeField] private float checkDelay;
    [SerializeField] private int direction;
    [SerializeField] private LayerMask playerMask;
    private Vector3 destination;
    private Vector3 defaultPosition;
    private Vector3[] directions = new Vector3[4];

    private float checkTimer;
    
    private bool attacking;
    private bool backToDefaultPos;

    private void Awake() {
        defaultPosition = transform.position;
    }

    private void Update() {
        if(attacking == true && backToDefaultPos == false) {
            transform.Translate(destination * Time.deltaTime * speed);
        } else {
            checkTimer += Time.deltaTime;
            if(checkTimer > checkDelay) {
                CheckForPlayer();
            }
        }

        CheckDirectionCase();

        if(backToDefaultPos == true) {
            attacking = false;
            transform.Translate(-destination * Time.deltaTime * (speed / 5));
        }
    }

    private void CheckForPlayer() {
        CalculateDirection();

        Debug.DrawRay(transform.position, directions[direction - 1], Color.red, 2f);
        RaycastHit2D hit = Physics2D.Raycast(transform.position, directions[direction - 1], range, playerMask);
        if(hit.collider != null && !attacking) {
            attacking = true;
            destination = directions[direction - 1];
            checkTimer = 0;
        }
    }

    private void CheckDirectionCase() {
        if(direction == 1 || direction == 3) {
            if(transform.position.x <= defaultPosition.x && transform.position.y <= defaultPosition.y) {
                backToDefaultPos = false;
            }
        } else if(direction == 2 || direction == 4) {
            if(transform.position.x >= defaultPosition.x && transform.position.y >= defaultPosition.y) {
                backToDefaultPos = false;
            }
        }
    }

    private void CalculateDirection() {
        directions[0] = transform.right * range;
        directions[1] = -transform.right * range;
        directions[2] = transform.up * range;
        directions[3] = -transform.up * range;
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
