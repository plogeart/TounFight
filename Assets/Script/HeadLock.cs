using UnityEngine;

public class HeadLook : MonoBehaviour
{
    [Header("Réglages")]
    public float lookDistance = 20f; // A quelle distance il regarde
    [Range(0, 1)] public float headWeight = 1f; // 1 = regarde totalement, 0 = regarde pas
    public float bodyWeight = 0.3f; // Le corps tourne un peu aussi

    private Animator anim;
    private Transform cam;

    void Start()
    {
        anim = GetComponent<Animator>();
        if (Camera.main != null) cam = Camera.main.transform;
    }

    // Cette fonction spéciale est appelée par l'Animator si "IK Pass" est coché
    void OnAnimatorIK()
    {
        if (anim == null || cam == null) return;

        // 1. Définir le point que l'on regarde (droit devant la caméra)
        Vector3 targetPosition = cam.position + cam.forward * lookDistance;

        // 2. Dire à l'animator de regarder ce point
        anim.SetLookAtWeight(headWeight, bodyWeight, 0.5f, 0.5f, 0.7f);
        anim.SetLookAtPosition(targetPosition);
    }
}