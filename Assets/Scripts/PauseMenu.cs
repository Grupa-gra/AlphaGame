using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class PauseMenuScript : MonoBehaviour
{
    bool gamePaused = false;
    [SerializeField] GameObject pauseMenu;
    [SerializeField] GameObject hud;
    [SerializeField] TileSelector tileSelector;
    [SerializeField] List<Button> buttonsToDisable; // Lista przycisk�w do wy��czenia
    [SerializeField] List<AudioSource> audioSourcesToPause; // Lista �r�de� d�wi�ku do zatrzymania

    void Update()
    {
        // Wy��czona obs�uga klawisza Escape
    }

    public void Pause()
    {
        // Je�li HUD jest przypisany, schowaj go
        if (hud != null)
        {
            hud.SetActive(false);
        }

        // Je�li TileSelector jest przypisany, wy��cz go
        if (tileSelector != null)
        {
            tileSelector.isActive = false;
        }

        // Wy��cz interaktywno�� przycisk�w
        if (buttonsToDisable != null)
        {
            foreach (Button button in buttonsToDisable)
            {
                if (button != null)
                {
                    button.interactable = false;
                }
            }
        }

        // Zatrzymaj wszystkie d�wi�ki
        if (audioSourcesToPause != null)
        {
            foreach (AudioSource audioSource in audioSourcesToPause)
            {
                if (audioSource != null && audioSource.isPlaying)
                {
                    audioSource.Pause();
                }
            }
        }

        Time.timeScale = 0;
        gamePaused = true;
        pauseMenu.SetActive(true);
    }

    public void Home()
    {
        SceneManager.LoadScene("MainMenu");
        Time.timeScale = 1;
        gamePaused = false;
        pauseMenu.SetActive(false);

        EnableButtons(); // Przywr�� interaktywno�� przycisk�w
        ResumeAudio(); // Wznowienie d�wi�k�w
    }

    public void Continue()
    {
        Time.timeScale = 1;
        gamePaused = false;
        pauseMenu.SetActive(false);
        hud.SetActive(true);

        if (tileSelector != null)
        {
            tileSelector.isActive = true;
        }

        EnableButtons(); // Przywr�� interaktywno�� przycisk�w
        ResumeAudio(); // Wznowienie d�wi�k�w
    }

    public void ContinueFight()
    {
        Time.timeScale = 1;
        gamePaused = false;
        pauseMenu.SetActive(false);
        hud.SetActive(true);

        EnableButtons(); // Przywr�� interaktywno�� przycisk�w
        ResumeAudio(); // Wznowienie d�wi�k�w
    }

    public void Surrender()
    {
        SceneManager.LoadScene("SampleScene");
        Time.timeScale = 1;
        gamePaused = false;
        pauseMenu.SetActive(false);

        EnableButtons(); // Przywr�� interaktywno�� przycisk�w
        ResumeAudio(); // Wznowienie d�wi�k�w
    }

    // Funkcja do przywr�cenia interaktywno�ci przycisk�w
    private void EnableButtons()
    {
        if (buttonsToDisable != null)
        {
            foreach (Button button in buttonsToDisable)
            {
                if (button != null)
                {
                    button.interactable = true;
                }
            }
        }
    }

    // Funkcja do wznowienia odtwarzania d�wi�k�w
    private void ResumeAudio()
    {
        if (audioSourcesToPause != null)
        {
            foreach (AudioSource audioSource in audioSourcesToPause)
            {
                if (audioSource != null)
                {
                    audioSource.UnPause();
                }
            }
        }
    }
}