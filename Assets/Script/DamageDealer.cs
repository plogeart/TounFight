using UnityEngine;
public class DamageDealer : MonoBehaviour {
    private void OnTriggerEnter(Collider other) {
        if (other.CompareTag("Enemy")) {
            Debug.Log("PIF ! Touché : " + other.name);
        }
    }
}