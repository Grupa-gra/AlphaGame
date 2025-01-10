using UnityEngine;

public class MarkerRotate : MonoBehaviour
{
    public float rotationSpeed = 50f; // Pr�dko�� obrotu w stopniach na sekund�

    void Update()
    {
        // Obracaj wok� osi Z (p�askiej w 2D)
        transform.Rotate(0, rotationSpeed * Time.deltaTime, 0);
    }
}