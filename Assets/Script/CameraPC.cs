using UnityEngine;

public class CameraPC : MonoBehaviour
{
    public Transform target; // Ton Perso
    public Vector3 offset = new Vector3(0, 2, -4);
    public float sensitivity = 5f;

    float currentX = 0f;
    float currentY = 0f;

    void Start()
    {
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }

    void LateUpdate()
    {
        if (target == null) return;

        // 1. Rotation Libre (Souris)
        currentX += Input.GetAxis("Mouse X") * sensitivity;
        currentY -= Input.GetAxis("Mouse Y") * sensitivity;

        // 2. Limites Verticales (Pour ne pas passer sous le sol)
        currentY = Mathf.Clamp(currentY, -30, 60);

        // 3. Calcul de la Position
        Vector3 dir = new Vector3(0, 0, -offset.magnitude);
        Quaternion rotation = Quaternion.Euler(currentY, currentX, 0);
        
        // La caméra suit la position du joueur + la rotation
        transform.position = target.position + rotation * dir;
        
        // 4. Elle regarde le joueur
        transform.LookAt(target.position + Vector3.up * 1.5f);
    }
}