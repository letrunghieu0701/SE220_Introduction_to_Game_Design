using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HorizontalPlatform : MonoBehaviour
{
    [SerializeField] private float speed;
    [SerializeField] private float movementDistance;

    private bool movingLeft;

    private float leftEdge;
    private float rightEdge;
    void Awake() {
        leftEdge = transform.position.x - movementDistance;
        rightEdge = transform.position.x + movementDistance;
    }

    void Update() {
        if(movingLeft) {
            if(transform.position.x > leftEdge) {
                transform.position = new Vector3(transform.position.x - speed * Time.deltaTime, transform.position.y, transform.position.z);
            } else movingLeft = false;
        } else {
            if(transform.position.x < rightEdge) {
                transform.position = new Vector3(transform.position.x + speed * Time.deltaTime, transform.position.y, transform.position.z);
            } else movingLeft = true;
        }
    }

    private void OnCollisionEnter2D(Collision2D col) {
        col.transform.SetParent(transform);
    }

    private void OnCollisionExit2D(Collision2D col) {
        col.transform.SetParent(null);
    }
}
