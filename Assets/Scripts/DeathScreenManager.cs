using UnityEngine;
using UnityEngine.UI;

public class MainMenuManager : MonoBehaviour
{
    [SerializeField] private GameObject deathScreen;
    [SerializeField] private Button returnButton;

    [SerializeField] private AudioSource mainMenuMusic;    // �r�d�o muzyki dla menu g��wnego
    [SerializeField] private AudioSource deathScreenMusic; // �r�d�o muzyki dla ekranu �mierci

    private void Start()
    {
        // Sprawd�, czy gracz przegra�
        if (PlayerInfo.Instance != null && PlayerInfo.Instance.hasPlayerLost)
        {
            // Wy�wietl ekran �mierci
            if (deathScreen != null) deathScreen.SetActive(true);

            // Odtw�rz muzyk� ekranu �mierci
            if (mainMenuMusic != null && mainMenuMusic.isPlaying)
                mainMenuMusic.Stop();

            if (deathScreenMusic != null)
                deathScreenMusic.Play();
        }
        else
        {
            // Gracz nie przegra� � odtwarzaj muzyk� menu g��wnego
            if (mainMenuMusic != null && !mainMenuMusic.isPlaying)
                mainMenuMusic.Play();
        }

        // Przypisz dzia�anie do przycisku powrotu
        if (returnButton != null)
            returnButton.onClick.AddListener(HideDeathScreen);
    }

    private void HideDeathScreen()
    {
        // Ukryj ekran �mierci
        if (deathScreen != null) deathScreen.SetActive(false);

        // Zatrzymaj muzyk� ekranu �mierci i w��cz muzyk� menu g��wnego (opcjonalnie)
        if (deathScreenMusic != null && deathScreenMusic.isPlaying)
            deathScreenMusic.Stop();

        if (mainMenuMusic != null && !mainMenuMusic.isPlaying)
            mainMenuMusic.Play();
    }
}