using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using TMPro;

public class SceneLoader : MonoBehaviour
{
    public GameObject loadingScreen; // Obiekt ekranu �adowania
    public Slider LoadingBar;        // Pasek �adowania
    public TextMeshProUGUI hintText;           // Pole tekstowe na wskaz�wki
    public List<string> hints;       // Lista wskaz�wek
    public float hintChangeInterval = 3f; // Czas mi�dzy zmian� wskaz�wek

    public void LoadScene(int Levelindex)
    {
        StartCoroutine(LoadSceneAsynchronously(Levelindex));
    }

    IEnumerator LoadSceneAsynchronously(int Levelindex)
    {
        AsyncOperation operation = SceneManager.LoadSceneAsync(Levelindex);
        loadingScreen.SetActive(true);

        // Rozpocz�cie zmiany wskaz�wek
        StartCoroutine(ChangeHints());

        while (!operation.isDone)
        {
            // Aktualizacja paska post�pu
            LoadingBar.value = Mathf.Clamp01(operation.progress / 0.9f);
            yield return null;
        }
    }

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