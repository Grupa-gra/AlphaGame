using UnityEngine;
using TMPro;

public class Radio : MonoBehaviour
{
    public TextMeshProUGUI songNameText; // TextMeshPro do wy�wietlania nazwy utworu
    public AudioClip[] songs;           // Tablica utwor�w
    private AudioSource audioSource;    // AudioSource z MusicManager
    private int currentSongIndex = 0;   // Indeks obecnego utworu
    private bool isPaused = false;      // Czy muzyka jest wstrzymana

    void Start()
    {
        // Znajd� AudioSource na obiekcie MusicManager
        GameObject musicManager = GameObject.Find("MusicManager");
        if (musicManager != null)
        {
            audioSource = musicManager.GetComponent<AudioSource>();
        }
        if (audioSource == null)
        {
            Debug.LogError("Nie znaleziono komponentu AudioSource na MusicManager.");
            return;
        }

        // Zainicjuj pierwszy utw�r
        PlaySong(currentSongIndex);
    }

    // Funkcja odtwarzaj�ca utw�r na podstawie indeksu
    public void PlaySong(int index)
    {
        if (index >= 0 && index < songs.Length)
        {
            audioSource.clip = songs[index];
            audioSource.Play();
            songNameText.text = songs[index].name; // Wy�wietl nazw� utworu
            currentSongIndex = index;
            isPaused = false; // Resetuj status pauzy
        }
    }

    // Funkcja do przej�cia do nast�pnego utworu
    public void NextSong()
    {
        currentSongIndex = (currentSongIndex + 1) % songs.Length;
        PlaySong(currentSongIndex);
    }

    // Funkcja do przej�cia do poprzedniego utworu
    public void PreviousSong()
    {
        currentSongIndex = (currentSongIndex - 1 + songs.Length) % songs.Length;
        PlaySong(currentSongIndex);
    }

    // Funkcja do zatrzymania lub wznowienia muzyki
    public void TogglePauseResume()
    {
        if (isPaused)
        {
            // Wzn�w muzyk�
            audioSource.Play();
            isPaused = false;
        }
        else
        {
            // Wstrzymaj muzyk�
            audioSource.Pause();
            isPaused = true;
        }
    }
}