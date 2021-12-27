using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Transporter : MonoBehaviour
{
    [Header("Attributes")]
    [SerializeField] private float speed;
    [SerializeField] private int direction;
    private Animator ani;

    private void Awake() {
        ani = GetComponent<Animator>();
    }

    private void FixedUpdate() {
        ani.SetBool("goleft", direction == 1);
        ani.SetBool("goright", direction == 2);
    }
}
