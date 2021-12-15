using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ProjectTile : MonoBehaviour
{   
    [SerializeField] private float speed;
    [SerializeField] private int damage;
    private bool hit;
    private bool isWall;
    private float direction;
    private float lifeTime;
    private BoxCollider2D collider;
    private Animator ani;

    private void Awake() {
        ani = GetComponent<Animator>();
        collider = GetComponent<BoxCollider2D>();
    }

    private void Update() {

        if(hit) {
            return;
        }

        float moveSpeed = speed * Time.deltaTime * direction;
        transform.Translate(moveSpeed, 0, 0);

        lifeTime += Time.deltaTime;
        if(lifeTime > 3) gameObject.SetActive(false);
    }

    private void OnTriggerEnter2D(Collider2D col) {
        if(col.tag == "Monster" || col.tag == "Ground") {
            hit = true;
            collider.enabled = false;
            ani.SetTrigger("explote");
            if(col.tag == "Monster") {
                col.GetComponent<Monster>().TakeDamage(damage);
            }
        }
    }

    public void SetDirection(float dir) {
        lifeTime = 0;
        direction = dir;
        gameObject.SetActive(true);
        hit = false;
        collider.enabled = true;

        float localScaleX = transform.localScale.x;
        if(Mathf.Sign(localScaleX) != dir) {
            localScaleX = -localScaleX;
        }

        transform.localScale = new Vector3(localScaleX, transform.localScale.y, transform.localScale.z);
    }

    private void Deactivate() {
        gameObject.SetActive(false);
    }
}
