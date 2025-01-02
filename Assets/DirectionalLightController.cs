using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class DirectionalLightController : MonoBehaviour
{
    public float rotationSpeed = 10f; // Pr�dko�� obrotu
    public Light directionalLight;   // Odno�nik do Directional Light

    public Color morningColor = Color.red;    // Kolor poranka
    public Color middayColor = Color.white;   // Kolor po�udnia
    public Color eveningColor = new Color(1f, 0.5f, 0f); // Kolor wieczoru

    public float minIntensity = 0.2f; // Minimalna intensywno�� (noc)
    public float maxIntensity = 2.0f; // Maksymalna intensywno�� (dzie�)

    [Range(0f, 1f)]
    public float dayProportion = 0.9f; // Proporcja dnia: 90% dzie�, 10% noc

    public float speedIncrement = 0.5f; // Warto��, o kt�r� zmieniamy pr�dko��
    public float speedFasterIncrement = 2f; 
    [SerializeField] private Button increaseButton; // Przycisk do zwi�kszania
    [SerializeField] private Button increaseFasterButton;
    [SerializeField] private Button decreaseButton; // Przycisk do zmniejszania
    [SerializeField] private Button pauseButton;
    [SerializeField] private TextMeshProUGUI clockText; // Tekst wy�wietlaj�cy godzin�

    public float timeInMinutes = 0f; // Czas w minutach (od 00:00 do 23:59)

    private void Start()
    { // Sprawd�, czy czas zosta� wczytany z GameDataManager
        if (GameDataManager.Instance?.LoadedSaveData != null)
        {
            timeInMinutes = GameDataManager.Instance.LoadedSaveData.gameTimeInMinutes; // Przypisanie wczytanego czasu
            Debug.Log($"Wczytano czas gry: {timeInMinutes} minut");
        }
        else
        {
            timeInMinutes = 14f * 60f; // Domy�lny czas (14:00)
            Debug.Log("Ustawiono domy�lny czas: 14:00");
        }

        if (increaseButton != null)
            increaseButton.onClick.AddListener(IncreaseSpeed);

        if (increaseFasterButton != null)
            increaseFasterButton.onClick.AddListener(IncreaseFasterSpeed);

        if (decreaseButton != null)
            decreaseButton.onClick.AddListener(DecreaseSpeed);

        if (pauseButton != null)
            pauseButton.onClick.AddListener(PauseSpeed);
    }

    private void Update()
    {
        // Obracaj Directional Light
        transform.Rotate(Vector3.up * rotationSpeed * Time.deltaTime);

        // Aktualizuj czas w minutach w zale�no�ci od pr�dko�ci obrotu
        float anglePerMinute = 360f / (24f * 60f); // 360 stopni w ci�gu 24 godzin
        timeInMinutes += rotationSpeed * Time.deltaTime / anglePerMinute;

        if (timeInMinutes >= 1440f) // Zresetuj po 24 godzinach
            timeInMinutes -= 1440f;

        UpdateClock();
        UpdateLighting();
    }

    private void UpdateClock()
    {
        // Konwertuj czas w minutach na godziny i minuty
        int hours = Mathf.FloorToInt(timeInMinutes / 60f);
        int minutes = Mathf.FloorToInt(timeInMinutes % 60f);

        // Ustaw tekst zegara w formacie HH:mm
        clockText.text = $"{hours:D2}:{minutes:D2}";
    }

    private void UpdateLighting()
    {
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

    // Funkcja zwi�kszaj�ca pr�dko�� obrotu
    private void IncreaseSpeed()
    {
        rotationSpeed += speedIncrement;
    }

    private void IncreaseFasterSpeed()
    {
        rotationSpeed += speedFasterIncrement;
    }

    // Funkcja zmniejszaj�ca pr�dko�� obrotu
    private void DecreaseSpeed()
    {
        rotationSpeed = Mathf.Max(0, rotationSpeed - speedIncrement); // Pr�dko�� nie mo�e by� ujemna
    }

    private void PauseSpeed()
    {
        rotationSpeed = 0;
    }
}