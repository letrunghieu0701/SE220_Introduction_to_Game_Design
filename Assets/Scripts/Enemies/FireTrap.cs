using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FireTrap : MonoBehaviour
{
    [SerializeField] private float damage;

    [Header ("FireTrap Timers")]
    [SerializeField] private float activationDelay;
    [SerializeField] private float activeTime;

    private Animator ani;
    private SpriteRenderer spiteRen;
    private GameObject flameAni;

    private bool triggered;
    private bool active;

    private float activeTimeCoolDown;

    private void Awake() {
        ani = GetComponent<Animator>();
        spiteRen = GetComponent<SpriteRenderer>();
        flameAni = gameObject.transform.GetChild(0).gameObject;
    }

    private void FixedUpdate() {
        if(ani) {
            ani.SetBool("activated", active);
            flameAni.SetActive(active);
        }
    }

    private void Update() {
        activeTimeCoolDown += Time.deltaTime;
        if(active == true && activeTimeCoolDown > activeTime) {
            activeTimeCoolDown = 0;
            active = false;
        }
        if(active == false && activeTimeCoolDown > activeTime) {
            activeTimeCoolDown = 0;
            active = true;
        }
    }

    private void OnTriggerEnter2D(Collider2D col) {
        if(col.tag == "Player") {
            // if(!triggered) {
            //     StartCoroutine(ActivateFireTrap());
            // }
            if(active) {
                col.GetComponent<Health>().TakeDamage(damage, transform.position);
            }
        }
    }

    private IEnumerator ActivateFireTrap() {
        triggered = true;
        yield return new WaitForSeconds(activationDelay);
        active = true;
        yield return new WaitForSeconds(activeTime);
        active = false;
        triggered = false;
    }
}
