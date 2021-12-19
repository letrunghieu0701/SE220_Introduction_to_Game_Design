using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Health : MonoBehaviour
{
    [Header("Health")]
    [SerializeField] private float playerHealth;
    [Header("IFrames")]
    [SerializeField] private float iFrameDuration;
    [SerializeField] private int numOfFlashes;
    private SpriteRenderer spriteRenderer;
    private Animator ani;
    private Knight knight;
    public float currentHealth { get; private set;}

    private void Awake() {
        currentHealth = playerHealth;
        ani = GetComponent<Animator>();
        knight = GetComponent<Knight>();
        spriteRenderer = GetComponent<SpriteRenderer>();
    }

    public void TakeDamage(float damage) {
        if(knight.GetIsHurting() == false) {
            currentHealth = Mathf.Clamp(currentHealth - damage, 0, playerHealth);
            if(currentHealth > 0) {
                ani.SetTrigger("hurt");
                StartCoroutine(Invunerability());
                knight.SetIsHurting(true);
            } else {
                if(!knight.GetIsDead()) {
                    ani.SetBool("Dead", true);
                    knight.SetIsDead(true);
                    knight.enabled = false;
                    currentHealth = playerHealth;
                }
                
                
            }
        }
    }

    public void Heal(float health) {
        currentHealth = Mathf.Clamp(currentHealth + health, 0, playerHealth);
    }

    private void Update() {
        if(Input.GetKeyDown(KeyCode.E)) {
            TakeDamage(1);
        }
    }

    private void enable() {
        Destroy(gameObject);
        LevelManager.instance.Respawn();
        gameObject.SetActive(false);
    }

    private IEnumerator Invunerability() {
        Physics2D.IgnoreLayerCollision(10, 11, true);
        for (int i = 0; i < numOfFlashes; i++) {
            spriteRenderer.color = new Color(1, 0, 0, 0.5f);
            yield return new WaitForSeconds(iFrameDuration / (numOfFlashes * 2));
            spriteRenderer.color = Color.white;
            yield return new WaitForSeconds(iFrameDuration / (numOfFlashes * 2));
        }
        Physics2D.IgnoreLayerCollision(10, 11, false);
    }
}
