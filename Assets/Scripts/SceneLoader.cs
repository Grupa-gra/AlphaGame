using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using TMPro;

public class SceneLoader : MonoBehaviour
{
    public GameObject loadingScreen; // Obiekt ekranu �adowania
    public Slider loadingBar;        // Pasek �adowania
    public TextMeshProUGUI hintText; // Pole tekstowe na wskaz�wki
    public List<string> hints;       // Lista wskaz�wek
    public float hintChangeInterval = 3f; // Czas mi�dzy zmian� wskaz�wek

    private Coroutine hintCoroutine; // Referencja do uruchomionej corutyny dla wskaz�wek

    private void Start()
    {
        // Sprawdzamy, czy ekrany �adowania i wskaz�wki s� prawid�owo przypisane
        if (loadingScreen == null || loadingBar == null || hintText == null)
        {
            Debug.LogError("Nie przypisano wszystkich element�w UI!");
        }
    }

    // Wywo�ywana, gdy chcesz za�adowa� scen�
    public void LoadScene(int Levelindex)
    {
        GameDataManager.Instance.ClearData();
        StartCoroutine(LoadSceneAsynchronously(Levelindex));
    }

    // Metoda do asynchronicznego �adowania sceny
    IEnumerator LoadSceneAsynchronously(int Levelindex)
    {
        AsyncOperation operation = SceneManager.LoadSceneAsync(Levelindex);
        loadingScreen.SetActive(true);

        // Rozpocz�cie zmiany wskaz�wek
        hintCoroutine = StartCoroutine(ChangeHints());

        while (!operation.isDone)
        {
            // Aktualizacja paska post�pu
            loadingBar.value = Mathf.Clamp01(operation.progress / 0.9f); // U�ywamy `0.9f`, bo `operation.progress` ko�czy si� na 0.9
            yield return null;
        }

        // Po za�adowaniu sceny zatrzymujemy zmienianie wskaz�wek
        StopCoroutine(hintCoroutine);
    }

    // P�tla do zmiany wskaz�wek
    IEnumerator ChangeHints()
    {
        while (true) // P�tla niesko�czona dop�ki ekran �adowania jest aktywny
        {
            if (hints.Count > 0)
            {
                int randomIndex = Random.Range(0, hints.Count);
                hintText.text = hints[randomIndex];
            }
            yield return new WaitForSeconds(hintChangeInterval);
        }
    }
}