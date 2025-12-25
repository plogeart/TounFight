using UnityEngine;

public class EnemyScript : MonoBehaviour
{
    [Header("Stats")]
    public int maxHealth = 100;
    private int currentHealth;

    [Header("Animations")]
    public Animator anim;

    // Pour éviter de taper un cadavre
    private bool isDead = false;

    void Start()
    {
        currentHealth = maxHealth;
        
        // Si l'animator n'est pas assigné manuellement, on le cherche sur l'objet
        if (anim == null) anim = GetComponent<Animator>();
    }

    public void TakeDamage(int damageAmount)
    {
        if (isDead) return; // On ne frappe pas un mort

        currentHealth -= damageAmount;

        // 1. REACTION AU COUP (Hit Stun)
        if (currentHealth > 0)
        {
            // Joue l'animation "Hit" si elle existe
            if(anim != null) anim.SetTrigger("Hit");
        }
        // 2. MORT
        else
        {
            Die();
        }
    }

    void Die()
    {
        isDead = true;
        
        // Désactive le collider pour ne plus bloquer le joueur
        GetComponent<Collider>().enabled = false;

        if (anim != null)
        {
            // Joue l'animation de mort
            anim.SetTrigger("Die");
        }

        // On détruit le corps après 5 secondes (le temps de voir l'animation)
        Destroy(gameObject, 5f);
    }
}