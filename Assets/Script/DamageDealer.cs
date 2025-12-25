using UnityEngine;

public class DamageDealer : MonoBehaviour
{
    public int damageAmount = 10;
    public float knockbackForce = 300f; // La puissance du recul

    private void OnTriggerEnter(Collider other)
    {
        // 1. On vérifie si c'est un ennemi
        if (other.CompareTag("Enemy"))
        {
            // --- GESTION DES DÉGÂTS ---
            // On cherche le script de vie (soit sur l'objet touché, soit sur son parent)
            EnemyScript enemy = other.GetComponent<EnemyScript>();
            if (enemy == null) enemy = other.GetComponentInParent<EnemyScript>();

            if (enemy != null)
            {
                enemy.TakeDamage(damageAmount);
            }

            // --- GESTION DU RECUL (Physique) ---
            // On cherche le Rigidbody pour le pousser
            Rigidbody enemyRB = other.GetComponent<Rigidbody>();
            if (enemyRB == null) enemyRB = other.GetComponentInParent<Rigidbody>();

            if (enemyRB != null)
            {
                // Calcul de la direction : De NOUS vers l'ENNEMI
                // On met y à 0 pour ne pas l'envoyer voler dans le ciel
                Vector3 direction = (other.transform.position - transform.position).normalized;
                direction.y = 0.2f; // On soulève un tout petit peu pour réduire la friction au sol

                // BOUM ! On applique une force instantanée (Impulse)
                enemyRB.AddForce(direction * knockbackForce, ForceMode.Impulse);
            }
        }
    }
}