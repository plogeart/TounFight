using UnityEngine;

public class EnemyStats : MonoBehaviour
{
    [Header("Santé")]
    public int maxHealth = 100;
    public int currentHealth;

    void Start()
    {
        // Au début du jeu, le monstre a toute sa vie
        currentHealth = maxHealth;
    }

    // Cette fonction sera appelée par ton Poing
    public void TakeDamage(int damage)
    {
        currentHealth -= damage; // On enlève les PV
        Debug.Log(transform.name + " a pris " + damage + " dégâts ! PV restants : " + currentHealth);

        // Si la vie tombe à 0, il meurt
        if (currentHealth <= 0)
        {
            Die();
        }
    }

    void Die()
    {
        Debug.Log(transform.name + " est MORT !");
        
        // Pour l'instant on détruit l'objet. 
        // Plus tard, on mettra une animation de mort ou un effet de particules.
        Destroy(gameObject); 
    }
}