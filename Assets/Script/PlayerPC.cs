using UnityEngine;

public class PlayerPC : MonoBehaviour
{
    public float moveSpeed = 5f;
    public float rotationSpeed = 10f;
    public Animator anim;
    public Transform cameraTransform; // Glisse la Main Camera ici
    // Variable pour stocker le collider du poing
    public Collider weaponCollider;

    void Update()
    {
        // 1. ATTAQUE (Clic Gauche)
        // 0 = Clic Gauche, 1 = Clic Droit, 2 = Molette
        if (Input.GetMouseButtonDown(0))
        {
            if(anim != null) anim.SetTrigger("Attack");
        }

        // 2. MOUVEMENT
        Move();
    }

    void Move()
    {
        // Récupère les touches (ZQSD ou Flèches) automatiquement
        float h = Input.GetAxis("Horizontal");
        float v = Input.GetAxis("Vertical");

        // Crée un vecteur de direction
        Vector3 direction = new Vector3(h, 0, v);
        float magnitude = direction.magnitude; // La "force" de l'appui (0 ou 1)

        // Animation : On envoie la vitesse (pour le Blend Tree)
        // On utilise Mathf.Clamp01 pour que ça ne dépasse pas 1
        if (anim != null) anim.SetFloat("Speed", Mathf.Clamp01(magnitude));

        // Si on ne bouge pas, on arrête tout
        if (magnitude < 0.1f) return;

        // --- DIRECTION RELATIVE A LA CAMERA ---
        // C'est l'astuce pour que "Haut" soit toujours "Là où regarde la caméra"
        Vector3 camFwd = cameraTransform.forward;
        Vector3 camRight = cameraTransform.right;
        
        camFwd.y = 0; // On ne veut pas voler vers le ciel
        camRight.y = 0;
        
        camFwd.Normalize();
        camRight.Normalize();

        Vector3 moveDir = (camFwd * v + camRight * h).normalized;

        // Rotation fluide du personnage
        Quaternion targetRotation = Quaternion.LookRotation(moveDir);
        transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, rotationSpeed * Time.deltaTime);

        // Déplacement physique (simple)
        transform.Translate(Vector3.forward * moveSpeed * Time.deltaTime);
    }

    // Fonction appelée par l'animation au début du coup
public void StartAttack() {
    if (weaponCollider != null) weaponCollider.enabled = true;
}

// Fonction appelée par l'animation à la fin du coup
public void EndAttack() {
    if (weaponCollider != null) weaponCollider.enabled = false;
}
}