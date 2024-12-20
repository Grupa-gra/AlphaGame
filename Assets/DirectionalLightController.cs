using UnityEngine;

public class DirectionalLightController : MonoBehaviour
{
    public float rotationSpeed = 10f; // Pr�dko�� obrotu
    public Light directionalLight;   // Odno�nik do Directional Light
    private Color orange = new Color(1f, 0.5f, 0f); // Definicja pomara�czowego koloru

    public Color morningColor = Color.red;    // Kolor poranka
    public Color middayColor = Color.white;  // Kolor po�udnia
    public Color eveningColor = new Color(1f, 0.5f, 0f); // Kolor wieczoru

    public float minIntensity = 0.2f; // Minimalna intensywno�� (noc)
    public float maxIntensity = 2.0f; // Maksymalna intensywno�� (dzie�)

    [Range(0f, 1f)]
    public float dayProportion = 0.9f; // Proporcja dnia: 90% dzie�, 10% noc

    void Update()
    {
        // Obracaj Directional Light
        transform.Rotate(Vector3.up * rotationSpeed * Time.deltaTime);

        // Oblicz k�t w zakresie od -180 do 180
        float angle = transform.eulerAngles.x;
        if (angle > 180f) angle -= 360f;

        // Podzia� cyklu na dzie� i noc
        float dayBoundary = 90f * dayProportion;
        float normalizedAngle;

        if (angle > -dayBoundary && angle < dayBoundary)
        {
            // Dzie�
            normalizedAngle = Mathf.InverseLerp(-dayBoundary, dayBoundary, angle);
            if (normalizedAngle <= 0.5f)
            {
                // Poranek -> Po�udnie
                directionalLight.color = Color.Lerp(morningColor, middayColor, normalizedAngle * 2f);
            }
            else
            {
                // Po�udnie -> Wiecz�r
                directionalLight.color = Color.Lerp(middayColor, eveningColor, (normalizedAngle - 0.5f) * 2f);
            }

            // Zwi�kszona intensywno�� �wiat�a w ci�gu dnia
            directionalLight.intensity = Mathf.Lerp(minIntensity, maxIntensity, normalizedAngle);
        }
        else
        {
            // Noc
            normalizedAngle = Mathf.InverseLerp(-180f + dayBoundary, 180f - dayBoundary, angle);
            directionalLight.color = Color.black; // Mo�esz zmieni� na np. granatowy dla nocy
            directionalLight.intensity = minIntensity;
        }
    }
}