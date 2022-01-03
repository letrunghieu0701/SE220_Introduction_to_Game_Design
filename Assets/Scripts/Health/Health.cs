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
    [Header("Sound")]
    [SerializeField] private AudioSource hurtSound;
    [SerializeField] private AudioSource dieSound;
    private SpriteRenderer spriteRenderer;
    private Animator ani;
    private Knight knight;
    private bool startCoroutine = false;
    public float currentHealth { get; private set;}

    private void Awake() {
        currentHealth = playerHealth;
        ani = GetComponent<Animator>();
        knight = GetComponent<Knight>();
        spriteRenderer = GetComponent<SpriteRenderer>();
        currentHealthBar.fillAmount = currentHealth / 10;
    }

    public void TakeDamage(float damage, Vector3 objectPos) {
        if(knight.GetIsHurting() == false) {
            currentHealth = Mathf.Clamp(currentHealth - damage, 0, playerHealth);
            currentHealthBar.fillAmount = currentHealth / 10;
            if(knight.enabled) {
                if(currentHealth > 0) {
                    hurtSound.Play();
                    ani.SetTrigger("hurt");
                    knight.setAttackToFalse();
                    knight.DoKnockBack(objectPos);
                    StartCoroutine(Invunerability());
                } else {
                    if(knight.GetIsDead() == false) {
                        dieSound.Play();
                        ani.SetTrigger("Die");
                        knight.SetIsDead(true);
                        knight.enabled = false;
                        if(startCoroutine == false) {
                            StartCoroutine(enable());
                        }
                    }
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
            TakeDamage(1, transform.position);
        }
    }

    public float GetCurrentHealth() {
        return currentHealth;
    }

    private IEnumerator enable() {
        if(startCoroutine == false) {
            startCoroutine = true;
            yield return new WaitForSeconds(1f);
        
            LevelManager.instance.lifeCount--;
            if (LevelManager.instance.lifeCount < 0)
            {
                LevelManager.instance.ReLevel();
            }
            else
            {
                LevelManager.instance.Respawn();
            }
        }
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
