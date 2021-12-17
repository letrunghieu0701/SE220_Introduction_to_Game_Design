using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class playerAttack : MonoBehaviour
{
    [SerializeField] private float cutCoolDown;
    [SerializeField] private Transform firePoint;
    [SerializeField] private GameObject[] fireballs;
    private Animator ani;
    private Knight knight;
    private float coolDownTimer = Mathf.Infinity;

    private void Awake() {
        ani = GetComponent<Animator>();
        knight = GetComponent<Knight>();
    }

    private void Update() {
        if(Input.GetKeyDown(KeyCode.C) && coolDownTimer > cutCoolDown && knight.canShoot()) {
            Shoot();
        }

        coolDownTimer += Time.deltaTime;
    }

    private void Shoot() {
        ani.SetTrigger("cast");
        coolDownTimer = 0;

        fireballs[FindFireball()].transform.position = firePoint.position;
        if(knight.GetIsWall() == true) {
            fireballs[FindFireball()].GetComponent<ProjectTile>().SetDirection(Mathf.Sign(-transform.localScale.x));
        } else {
            fireballs[FindFireball()].GetComponent<ProjectTile>().SetDirection(Mathf.Sign(transform.localScale.x));
        }
    }

    private int FindFireball() {
        for (int i = 0; i < fireballs.Length; i++) {
            if(!fireballs[i].activeInHierarchy) {
                return i;
            }
        }
        return 0;
    }
}
