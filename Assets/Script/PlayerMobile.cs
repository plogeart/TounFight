using UnityEngine;
using UnityEngine.UI;

public class PlayerMobile : MonoBehaviour
{
    public float speed = 5f;
    public VirtualJoystick joystick; // Lien vers le joystick
    public Transform cameraTransform; // Lien vers la caméra pour la direction
    public Button attackBtn;          // Lien vers le bouton

    private Animator anim;
    private Rigidbody rb;

    void Start()
    {
        anim = GetComponent<Animator>();
        rb = GetComponent<Rigidbody>();

        // Dire au bouton d'attaque d'appeler la fonction Attack() quand on clique
        if (attackBtn != null)
            attackBtn.onClick.AddListener(Attack);
    }

    void FixedUpdate()
    {
        Move();
    }

    void Move()
    {
        // 1. Récupérer l'entrée du joystick
        Vector2 input = joystick.inputVector;

        if (anim != null)
        {
            anim.SetFloat("Speed", input.magnitude);
        }

        // Si on ne touche pas le joystick, on s'arrête
        if (input.magnitude < 0.1f) return;

        // 2. Calculer la direction par rapport à la caméra
        Vector3 camFwd = cameraTransform.forward;
        Vector3 camRight = cameraTransform.right;

        // On aplatit la direction (on ne veut pas voler vers le ciel)
        camFwd.y = 0;
        camRight.y = 0;
        camFwd.Normalize();
        camRight.Normalize();

        // Direction finale
        Vector3 moveDir = (camFwd * input.y + camRight * input.x).normalized;

        // 3. Déplacer le joueur
        Vector3 newPos = transform.position + moveDir * speed * Time.fixedDeltaTime;
        rb.MovePosition(newPos);

        // 4. Tourner le joueur vers la direction
        Quaternion targetRotation = Quaternion.LookRotation(moveDir);
        transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, 10f * Time.fixedDeltaTime);
        
        // (Optionnel) Lancer l'anim de marche
        // anim.SetBool("IsWalking", true);
    }

    void Attack()
    {
        // Jouer l'animation d'attaque
        anim.SetTrigger("Attack");
        
        // Ici tu mettras plus tard le code pour faire des dégâts
        Debug.Log("Pan ! Attaque !");
    }
}