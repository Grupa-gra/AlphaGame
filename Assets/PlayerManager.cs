using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System.Collections;


public class PlayerManager : MonoBehaviour
{
    public static PlayerManager Instance; // Singleton
    public TMP_Text coinsText; // Pole do wy�wietlania liczby monet
    public TMP_Text coinsinfo; // Pole do wy�wietlania informacji o zdobytych monetach
    public int coins; // Liczba monet gracza
    private Coroutine infoCoroutine; // Przechowuje referencj� do aktywnej korutyny

    void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            //Destroy(gameObject);
        }
    }

    // Dodawanie monet
    public void AddCoins(int amount)
    {
        coins += amount;
        UpdateUI();

        // Wy�wietl informacj� o zdobytych monetach
        ShowCoinsInfo(amount);
    }

    // Aktualizacja UI
    private void UpdateUI()
    {
        if (coinsText != null)
        {
            coinsText.text = "" + coins;
        }
    }

    // Wy�wietlanie informacji o zdobytych monetach
    private void ShowCoinsInfo(int amount)
    {
        if (infoCoroutine != null)
        {
            StopCoroutine(infoCoroutine); // Zatrzymaj poprzedni� korutyn�, je�li istnieje
        }

        infoCoroutine = StartCoroutine(DisplayCoinsInfo(amount));
    }

    private IEnumerator DisplayCoinsInfo(int amount)
    {
        if (coinsinfo != null)
        {
            coinsinfo.text = "You found " + amount + " coins in chest!";
            coinsinfo.gameObject.SetActive(true); // Upewnij si�, �e tekst jest widoczny
        }

        yield return new WaitForSeconds(10f); // Wy�wietlaj przez 10 sekund

        if (coinsinfo != null)
        {
            coinsinfo.gameObject.SetActive(false); // Ukryj tekst po up�ywie czasu
        }
    }
}