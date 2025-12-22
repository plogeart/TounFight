using UnityEngine;

public class CameraPC : MonoBehaviour
{
    public Transform target; // Glisse ton Perso ici
    public Vector3 offset = new Vector3(0, 2, -4); // Position par rapport au perso
    public float sensitivity = 5f; // Vitesse de la souris

    float currentX = 0f;
    float currentY = 0f;

    void Start()
    {
        // Vérouille la souris au centre de l'écran et la cache
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }

    void LateUpdate()
    {
        // Récupère le mouvement de la souris
        currentX += Input.GetAxis("Mouse X") * sensitivity;
        currentY -= Input.GetAxis("Mouse Y") * sensitivity;

        // Empêche la caméra de faire des tonneaux (bloque le haut/bas)
        currentY = Mathf.Clamp(currentY, -30, 60);

        // Calcul de la rotation et position
        Vector3 dir = new Vector3(0, 0, -offset.magnitude);
        Quaternion rotation = Quaternion.Euler(currentY, currentX, 0);
        
        // Applique la position : Position du joueur + Rotation + Recul
        transform.position = target.position + rotation * dir;
        
        // La caméra regarde toujours le joueur (un peu au dessus de ses pieds)
        transform.LookAt(target.position + Vector3.up * 1.5f);
    }
}