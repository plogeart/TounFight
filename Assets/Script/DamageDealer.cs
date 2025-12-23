using UnityEngine;

public class DamageDealer : MonoBehaviour
{
    public int damageAmount = 25; // Dégâts du coup de poing

    private void OnTriggerEnter(Collider other)
    {
        // On vérifie si c'est un Ennemi
        if (other.CompareTag("Enemy"))
        {
            // On cherche si l'ennemi a un script de statistiques (PV)
            EnemyStats enemy = other.GetComponent<EnemyStats>();

            if (enemy != null)
            {
                // BIM ! On lui fait mal
                enemy.TakeDamage(damageAmount);
            }
        }
    }
}