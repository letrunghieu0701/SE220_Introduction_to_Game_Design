using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bringer_death : MonoBehaviour
{
    [Header("Info")]
    [SerializeField] private int startHealth;
    [SerializeField] private int damage;
    [SerializeField] private float range;
    [SerializeField] private Transform playerCheck;
    [SerializeField] private LayerMask playerLayer;
    [SerializeField] private float checkRadius = 0.5f;
    [SerializeField] private float speed = 3f;
    [SerializeField] private float movementDistance = 3f;
    [Header("Timer")]
    [SerializeField] private float attackCoolDown = 2f;
    [SerializeField] private float destroyObjAfterDieDelay = 1f;

    private Animator ani;

    private float coolDownTimer;
    private float leftEdge;
    private float rightEdge;

    private int currentHealth;

    private bool isPlayer;
    private bool movingLeft;
    private bool isDead;
    private bool canAttack;

    private void Awake() {
        currentHealth = startHealth;
        leftEdge = transform.position.x - movementDistance;
        rightEdge = transform.position.x + movementDistance;
        ani = GetComponent<Animator>();
    }

    private void FixedUpdate() {
        ani.SetBool("walk", !canAttack);
    }

    private void Update() {
        isPlayer = Physics2D.OverlapCircle(playerCheck.position, checkRadius, playerLayer);
        coolDownTimer += Time.deltaTime;

        if(canAttack == false) {
            Movement();
        }

        if(isPlayer == true) {
            if(coolDownTimer >= attackCoolDown) {
                canAttack = true;
                coolDownTimer = 0;
                ani.SetTrigger("attack");
            }
        } else {
            canAttack = false;
        }
    }

    private void Movement() {
        if(movingLeft) {
            if(transform.position.x > leftEdge) {
                transform.position = new Vector3(transform.position.x - speed * Time.deltaTime, transform.position.y, transform.position.z);
            } else Flip();
        } else {
            if(transform.position.x < rightEdge) {
                transform.position = new Vector3(transform.position.x + speed * Time.deltaTime, transform.position.y, transform.position.z);
            } else Flip();
        }
    }

    private void Flip() {
        movingLeft = !movingLeft;
        Vector3 scaler = transform.localScale;
        scaler.x *= -1;
        transform.localScale = scaler;
    }

    public void TakeDamage(int damage) {
        if(currentHealth > 0) {
            ani.SetTrigger("hurt");
            currentHealth -= damage;
            if(currentHealth <= 0) {
                isDead = true;
                ani.SetTrigger("die");
                StartCoroutine(enable());
            }
        }
    }

    public void OnTriggerEnter2D(Collider2D col) {
        if(col.tag == "Player") {
            col.GetComponent<Health>().TakeDamage(damage, transform.position);
        }
    }

    private IEnumerator enable() {
        yield return new WaitForSeconds(destroyObjAfterDieDelay);
        gameObject.SetActive(false);
        Destroy(gameObject);
    }
}
