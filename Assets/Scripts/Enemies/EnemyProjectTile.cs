using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyProjectTile : EnermyDamage
{
    [SerializeField] private float speed;
    [SerializeField] private float resetTime;
    private float lifeTime;

    public void ActivateProjectTile() {
        lifeTime = 0;
        gameObject.SetActive(true);
    }

    private void Update() {
        float movement = speed * Time.deltaTime;
        transform.Translate(movement, 0, 0);

        lifeTime += Time.deltaTime;
        if(lifeTime > resetTime) {
            gameObject.SetActive(false);
        }
    }

    private void OnTriggerEnter2D(Collider2D col) {
        
    }
}
