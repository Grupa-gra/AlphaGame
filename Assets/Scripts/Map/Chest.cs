using UnityEngine;

public class Chest : MonoBehaviour
{
    public int coins; // Liczba monet w skrzyni
    private Animator animator; // Animator dla skrzyni
    private bool isOpened = false;
    public GameObject marker; // Znacznik nad skrzynk�

    void Start()
    {
        // Losowanie liczby monet w przedziale od 1 do 50
        coins = Random.Range(1, 51); // Random.Range(1, 51) generuje liczb� ca�kowit� od 1 do 50

        // Pobieramy Animator z komponentu obiektu skrzyni
        animator = GetComponent<Animator>();

        // Sprawd�, czy znacznik jest przypisany
        if (marker == null)
        {
            Debug.LogWarning("Znacznik nie zosta� przypisany do skrzyni!");
        }
    }

    // Metoda otwierania skrzyni
    public void OpenChest()
    {
        if (!isOpened)
        {
            // Uruchamiamy animacj� otwierania skrzyni
            if (animator != null)
            {
                animator.SetTrigger("Open");
            }

            // Ustawiamy flag�, �e skrzynka zosta�a otwarta
            isOpened = true;

            // Ukrywamy znacznik
            if (marker != null)
            {
                marker.SetActive(false); // Dezaktywacja znacznika
            }

            // Dodajemy monety do gracza (lub inna akcja)
            Debug.Log($"Znaleziono {coins} monet w skrzyni.");
        }
    }

    public void CloseChest()
    {
        // Uruchomienie animacji zamykania skrzyni
        if (animator != null)
        {
            animator.SetTrigger("Close");
        }

        // Opcjonalne: blokowanie dalszej interakcji
        if (isOpened)
        {
            this.GetComponent<Collider>().enabled = false; // Wy��czenie kolizji
        }
    }
}