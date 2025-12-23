using UnityEngine;

public class SpineCorrector : MonoBehaviour
{
    [Header("Réglages")]
    public Transform spineBone; // L'os de la colonne (Spine1 ou Spine2)
    public float angleCorrection = 45f; // L'angle pour redresser (essaie 45 ou -45)
    
    // Le nom EXACT de la boîte grise dans ton Animator (Layer UpperBody)
    // Si tu l'as nommée "Attack", laisse "Attack". Si c'est "Punching", change-le.
    public string attackStateName = "Attack"; 

    private Animator anim;

    void Start()
    {
        anim = GetComponent<Animator>();
    }

    // LateUpdate s'exécute APRES l'animation. C'est là qu'on peut tricher.
    void LateUpdate()
    {
        // On vérifie le Layer 1 (celui du Haut du corps)
        // Est-ce qu'on est en train de jouer l'attaque ?
        if (anim.GetCurrentAnimatorStateInfo(1).IsName(attackStateName))
        {
            // On tourne la colonne vertébrale pour compenser le décalage
            // On utilise Space.Self pour tourner par rapport au corps
            spineBone.Rotate(0, angleCorrection, 0, Space.Self);
        }
    }
}