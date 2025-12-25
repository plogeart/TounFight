using UnityEngine;

public class PlayerHealth : MonoBehaviour
{
    [Header("Santé")]
    public int maxHealth = 100;
    public int currentHealth;

    [Header("Animations")]
    public Animator anim;

    void Start()
    {
        currentHealth = maxHealth;
        if (anim == null) anim = GetComponent<Animator>();
    }

    public void TakeDamage(int damage)
    {
        currentHealth -= damage;
        Debug.Log("AIE ! J'ai pris " + damage + " dégâts. Reste : " + currentHealth);

        // Animation de coup reçu (Optionnel si tu as l'anim)
        // anim.SetTrigger("Hit");

        if (currentHealth <= 0)
        {
            Die();
        }
    }

    void Die()
    {
        Debug.Log("GAME OVER");
        anim.SetTrigger("Die"); // Assure-toi d'avoir un Trigger "Die" dans l'animator du joueur
        
        // On désactive le mouvement
        GetComponent<PlayerPC>().enabled = false;
        // On désactive les collisions pour ne pas bloquer les ennemis
        GetComponent<Collider>().enabled = false;
    }
}