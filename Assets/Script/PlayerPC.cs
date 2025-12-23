using UnityEngine;

public class PlayerPC : MonoBehaviour
{
    [Header("Paramètres")]
    public float moveSpeed = 5f;
    public float rotationSpeed = 0.1f; // Lissage des virages

    [Header("Combat")]
    public Collider weaponCollider;

    private Animator anim;
    private Transform camTransform;
    private Rigidbody rb;

    void Start()
    {
        anim = GetComponent<Animator>();
        rb = GetComponent<Rigidbody>();
        
        if (Camera.main != null) camTransform = Camera.main.transform;
    }

    void Update()
    {
        // --- 1. DÉPLACEMENTS ---
        float h = Input.GetAxis("Horizontal");
        float v = Input.GetAxis("Vertical");
        
        Vector3 moveDir = new Vector3(h, 0, v).normalized;
        float inputMagnitude = moveDir.magnitude; 

        // On envoie la vitesse au Blend Tree
        anim.SetFloat("Speed", inputMagnitude); 

        if (inputMagnitude >= 0.1f)
        {
            // Calcul de l'angle pour avancer dans la direction de la caméra
            float targetAngle = Mathf.Atan2(moveDir.x, moveDir.z) * Mathf.Rad2Deg + camTransform.eulerAngles.y;
            float angle = Mathf.SmoothDampAngle(transform.eulerAngles.y, targetAngle, ref rotationSpeed, 0.1f);
            
            transform.rotation = Quaternion.Euler(0f, angle, 0f);

            Vector3 moveDirection = Quaternion.Euler(0f, targetAngle, 0f) * Vector3.forward;
            transform.Translate(moveDirection.normalized * moveSpeed * Time.deltaTime, Space.World);
        }

        // --- 2. ATTAQUE ---
        if (Input.GetButtonDown("Fire1"))
        {
            // J'ai supprimé la ligne RotateTowardsCamera() ici.
            // Le personnage restera orienté comme il est.
            
            anim.SetTrigger("Attack");
        }
    }

    // --- EVENTS ANIMATION ---
    public void StartAttack()
    {
        if (weaponCollider != null) weaponCollider.enabled = true;
    }

    public void EndAttack()
    {
        if (weaponCollider != null) weaponCollider.enabled = false;
    }
}