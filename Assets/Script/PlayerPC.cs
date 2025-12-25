using UnityEngine;

public class PlayerPC : MonoBehaviour
{
    [Header("Mouvement")]
    public float maxSpeed = 5f;
    public float rotationSpeed = 20f;
    public float acceleration = 10f; // Vitesse de démarrage
    public float deceleration = 10f; // Vitesse de freinage (Plus c'est haut, plus l'arrêt est sec)

    [Header("Combat (Auto-Aim)")]
    public Collider weaponCollider;
    public float detectRadius = 5f;
    public float detectAngle = 90f;

    private Animator anim;
    private Transform camTransform;
    private float currentSpeed = 0f; // Pour stocker la vitesse réelle

    void Start()
    {
        anim = GetComponent<Animator>();
        if (Camera.main != null) camTransform = Camera.main.transform;
    }

    void Update()
    {
        // 1. INPUTS BRUTS (On/Off immédiat)
        float h = Input.GetAxisRaw("Horizontal");
        float v = Input.GetAxisRaw("Vertical");
        
        Vector3 inputDir = new Vector3(h, 0, v).normalized; // Normalized pour éviter d'aller plus vite en diagonale

        // 2. GESTION DE LA VITESSE (Accélération / Freinage)
        float targetSpeed = inputDir.magnitude * maxSpeed;

        if (inputDir.magnitude > 0.1f)
        {
            // On accélère vers la vitesse max
            currentSpeed = Mathf.MoveTowards(currentSpeed, targetSpeed, acceleration * Time.deltaTime);
        }
        else
        {
            // On freine vers 0 (C'est ici que tu règles l'arrêt "naturel")
            currentSpeed = Mathf.MoveTowards(currentSpeed, 0f, deceleration * Time.deltaTime);
        }

        // 3. ANIMATION
        // On envoie le pourcentage de vitesse (0 à 1) à l'Animator
        anim.SetFloat("InputY", currentSpeed / maxSpeed);
        anim.SetFloat("InputX", 0);

        // 4. DÉPLACEMENT & ROTATION
        // On bouge tant qu'il y a de la vitesse (même si on a lâché les touches)
        if (currentSpeed > 0.1f && camTransform != null)
        {
            // A. Si on appuie sur des touches, on met à jour la direction
            // Sinon, on garde la dernière direction connue pour le freinage
            Vector3 moveDirection = Vector3.zero;

            if (inputDir.magnitude > 0.1f)
            {
                moveDirection = camTransform.right * h + camTransform.forward * v;
            }
            else
            {
                // Si on freine, on continue tout droit dans la direction actuelle du perso
                moveDirection = transform.forward;
            }

            moveDirection.y = 0;
            moveDirection.Normalize();

            // B. DÉPLACEMENT
            transform.Translate(moveDirection * currentSpeed * Time.deltaTime, Space.World);

            // C. ROTATION (Seulement si on appuie sur une touche)
            if (inputDir.magnitude > 0.1f && moveDirection != Vector3.zero)
            {
                Quaternion targetRot = Quaternion.LookRotation(moveDirection);
                transform.rotation = Quaternion.Slerp(transform.rotation, targetRot, rotationSpeed * Time.deltaTime);
            }
        }

        // --- ATTAQUE ---
        if (Input.GetButtonDown("Fire1"))
        {
            AutoTargetEnemy();
            anim.SetTrigger("Attack");
        }
    }

    void AutoTargetEnemy()
    {
        Collider[] hits = Physics.OverlapSphere(transform.position, detectRadius);
        Transform bestTarget = null;
        float closestDistance = Mathf.Infinity;

        foreach (Collider hit in hits)
        {
            if (hit.CompareTag("Enemy"))
            {
                Vector3 directionToEnemy = (hit.transform.position - transform.position).normalized;
                if (Vector3.Angle(transform.forward, directionToEnemy) < detectAngle)
                {
                    float dst = Vector3.Distance(transform.position, hit.transform.position);
                    if (dst < closestDistance)
                    {
                        closestDistance = dst;
                        bestTarget = hit.transform;
                    }
                }
            }
        }

        if (bestTarget != null)
        {
            Vector3 targetPostion = new Vector3(bestTarget.position.x, transform.position.y, bestTarget.position.z);
            transform.LookAt(targetPostion);
        }
    }

    public void StartAttack() { if (weaponCollider != null) weaponCollider.enabled = true; }
    public void EndAttack() { if (weaponCollider != null) weaponCollider.enabled = false; }
}