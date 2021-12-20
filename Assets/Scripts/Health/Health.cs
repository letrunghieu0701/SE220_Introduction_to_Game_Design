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
                Debug.Log("hurt");
            } else {
                if(!knight.GetIsDead()) {
                    ani.SetTrigger("Die");
                    knight.SetIsDead(true);
                    knight.enabled = false;
                    StartCoroutine(enable());
                }
                // currentHealth = playerHealth;
                // Destroy(gameObject);
                // LevelManager.instance.Respawn();
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

    private IEnumerator enable() {
        yield return new WaitForSeconds(0.5f);
        gameObject.SetActive(false);
        currentHealth = playerHealth;
        gameObject.SetActive(true);
        knight.SetIsDead(false);
        knight.enabled = true;
        Destroy(gameObject);
        LevelManager.instance.Respawn();
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
