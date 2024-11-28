using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class GraphicsSettings : MonoBehaviour
{
    public TMP_Dropdown resolutionDropdown;
    public Toggle fullscreenToggle; // Toggle do obs�ugi trybu pe�noekranowego
    public Toggle vSyncToggle; // Toggle do obs�ugi V-Sync
    private Resolution[] resolutions;
    private List<Resolution> uniqueResolutions = new List<Resolution>();

    private void Start()
    {
        // Pobierz dost�pne rozdzielczo�ci ekranu
        resolutions = Screen.resolutions;

        // Wyczyszczenie opcji w dropdownie
        resolutionDropdown.ClearOptions();

        // Lista opcji do wy�wietlenia
        List<string> options = new List<string>();
        int currentResolutionIndex = 0;

        // Filtruj unikalne rozdzielczo�ci (usuwamy duplikaty rozdzielczo�ci o tej samej szeroko�ci i wysoko�ci)
        foreach (var resolution in resolutions)
        {
            bool isDuplicate = false;
            foreach (var uniqueResolution in uniqueResolutions)
            {
                if (uniqueResolution.width == resolution.width && uniqueResolution.height == resolution.height)
                {
                    isDuplicate = true;
                    break;
                }
            }

            if (!isDuplicate)
            {
                uniqueResolutions.Add(resolution);
                string option = resolution.width + " x " + resolution.height;
                options.Add(option);

                // Je�li to jest aktualna rozdzielczo�� ekranu, zapisz indeks
                if (resolution.width == 1920 && resolution.height == 1080)
                {
                    currentResolutionIndex = options.Count - 1;
                }
            }
        }

        // Dodaj unikalne opcje do dropdowna
        resolutionDropdown.AddOptions(options);

        // Wymu� ustawienie pocz�tkowej rozdzielczo�ci na 1920x1080
        SetResolution(1920, 1080);

        // Ustaw aktualnie wybran� opcj�
        resolutionDropdown.value = currentResolutionIndex;
        resolutionDropdown.RefreshShownValue();

        // Ustawienie stanu Toggle Fullscreen
        fullscreenToggle.isOn = Screen.fullScreen;
        fullscreenToggle.onValueChanged.AddListener(OnFullscreenToggleChanged);

        // Listener do zmiany rozdzielczo�ci w dropdownie
        resolutionDropdown.onValueChanged.AddListener(OnResolutionChanged);

        // Ustawienie stanu Toggle V-Sync
        vSyncToggle.isOn = QualitySettings.vSyncCount > 0;
        vSyncToggle.onValueChanged.AddListener(OnVSyncToggleChanged);
    }

    // Funkcja do ustawienia rozdzielczo�ci
    public void SetResolution(int width, int height)
    {
        // Ustaw tryb pe�noekranowy lub okienkowy w zale�no�ci od aktualnego stanu
        Screen.SetResolution(width, height, Screen.fullScreenMode);

        // Debugowanie proporcji ekranu
        float targetAspect = (float)width / height;
        float currentAspect = (float)Screen.width / Screen.height;
        if (Mathf.Abs(targetAspect - currentAspect) > 0.01f)
        {
            Debug.LogWarning("Uwaga: Proporcje mog� by� niedopasowane. Mog� pojawi� si� paski.");
        }
    }

    // Funkcja wywo�ywana po zmianie rozdzielczo�ci w dropdownie
    private void OnResolutionChanged(int resolutionIndex)
    {
        // Pobierz wybran� rozdzielczo�� z listy unikalnych rozdzielczo�ci
        Resolution selectedResolution = uniqueResolutions[resolutionIndex];
        SetResolution(selectedResolution.width, selectedResolution.height);
    }

    // Funkcja wywo�ywana, gdy zmienia si� stan Toggle Fullscreen
    private void OnFullscreenToggleChanged(bool isFullscreen)
    {
        SetFullscreen(isFullscreen);
    }

    // Funkcja ustawiaj�ca tryb pe�noekranowy
    public void SetFullscreen(bool isFullscreen)
    {
        Screen.fullScreenMode = isFullscreen ? FullScreenMode.FullScreenWindow : FullScreenMode.Windowed;
        Screen.fullScreen = isFullscreen;
    }

    // Funkcja do zmiany poziomu jako�ci grafiki
    public void SetQuality(int qualityIndex)
    {
        QualitySettings.SetQualityLevel(qualityIndex);
    }

    // Funkcja wywo�ywana po zmianie stanu Toggle V-Sync
    private void OnVSyncToggleChanged(bool isVSyncOn)
    {
        QualitySettings.vSyncCount = isVSyncOn ? 1 : 0;
    }
}