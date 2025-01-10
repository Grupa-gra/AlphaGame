using UnityEngine;

public class MarkerBounce : MonoBehaviour
{
    public float amplitude = 0.2f; // Maksymalna wysoko�� ruchu
    public float frequency = 2f;  // Pr�dko�� ruchu g�ra-d�

    private Vector3 startPosition;

    void Start()
    {
        startPosition = transform.localPosition; // Pozycja pocz�tkowa znacznika
    }

    void Update()
    {
        // Ruch g�ra-d� w osi Y
        float yOffset = Mathf.Sin(Time.time * frequency) * amplitude;
        transform.localPosition = startPosition + new Vector3(0, yOffset, 0);
    }
}