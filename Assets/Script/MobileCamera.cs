using UnityEngine;
using UnityEngine.EventSystems;

public class MobileCamera : MonoBehaviour, IDragHandler
{
    public Transform player;       // Le joueur à suivre
    public Transform cameraTarget; // La caméra elle-même
    
    public float sensitivity = 0.5f;
    public float distance = 5.0f;
    public float height = 2.0f;

    private float rotX;
    private float rotY;

    void Start()
    {
        // Initialisation des angles
        Vector3 angles = cameraTarget.eulerAngles;
        rotX = angles.y;
        rotY = angles.x;
    }

    public void OnDrag(PointerEventData eventData)
    {
        // On récupère le mouvement du doigt
        rotX += eventData.delta.x * sensitivity;
        rotY -= eventData.delta.y * sensitivity;

        // Limiter la rotation haut/bas
        rotY = Mathf.Clamp(rotY, -10f, 60f);
    }

    void LateUpdate()
    {
        if (player == null || cameraTarget == null) return;

        // Calcul de la rotation
        Quaternion localRotation = Quaternion.Euler(rotY, rotX, 0);
        
        // Positionner la caméra derrière le joueur
        cameraTarget.position = player.position - (localRotation * Vector3.forward * distance) + (Vector3.up * height);
        cameraTarget.LookAt(player.position + Vector3.up * 1.5f); // Regarde un peu au dessus des pieds
    }
}