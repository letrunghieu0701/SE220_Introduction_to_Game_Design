using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Health : MonoBehaviour
{
    [Header("Health")]
    [SerializeField] private float playerHealth;
    [SerializeField] public Image totalHealthBar;
    [SerializeField] public Image currentHealthBar;
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
        currentHealthBar.fillAmount = currentHealth / 10;
    }

    public void TakeDamage(float damage, float direction) {
        if(knight.GetIsHurting() == false) {
            currentHealth = Mathf.Clamp(currentHealth - damage, 0, playerHealth);
            currentHealthBar.fillAmount = currentHealth / 10;
            
            if(currentHealth > 0) {
                ani.SetTrigger("hurt");
                knight.setAttackToFalse();
                knight.setJumpAttackToFalse();
                knight.DoKnockBack(direction);
                StartCoroutine(Invunerability());
            } else {
                if(!knight.GetIsDead()) {
                    ani.SetTrigger("Die");
                    knight.SetIsDead(true);
                    knight.enabled = false;
                    StartCoroutine(enable());
                }
            }
        }
    }

    public void Heal(float health) {
        currentHealth = Mathf.Clamp(currentHealth + health, 0, playerHealth);
        currentHealthBar.fillAmount = currentHealth / 10;
    }

    private void Update() {
        if(Input.GetKeyDown(KeyCode.E)) {
            TakeDamage(1, 0);
        }
    }

    public float GetCurrentHealth() {
        return currentHealth;
    }

    private IEnumerator enable() {
        yield return new WaitForSeconds(1f);
        knight.SetIsDead(false);
        knight.enabled = true;
        Destroy(gameObject);
        LevelManager.instance.Respawn();
        Life.lifesCount -= 1;
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
