using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VerticalPlatform : MonoBehaviour
{
    [SerializeField] private float speed;
    [SerializeField] private float movementDistance;

    private bool movingDown;

    private float bottomEdge;
    private float aboveEdge;
    void Awake() {
        bottomEdge = transform.position.y - movementDistance;
        aboveEdge = transform.position.y + movementDistance;
    }

    void Update() {
        if(movingDown) {
            if(transform.position.y > bottomEdge) {
                transform.position = new Vector3(transform.position.x, transform.position.y - speed * Time.deltaTime, transform.position.z);
            } else movingDown = false;
        } else {
            if(transform.position.y < aboveEdge) {
                transform.position = new Vector3(transform.position.x, transform.position.y + speed * Time.deltaTime, transform.position.z);
            } else movingDown = true;
        }
    }

    private void OnCollisionEnter2D(Collision2D col) {
        col.transform.SetParent(transform);
    }

    private void OnCollisionExit2D(Collision2D col) {
        col.transform.SetParent(null);
    }
}
