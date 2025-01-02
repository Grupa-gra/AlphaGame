using UnityEngine;

public class Chest : MonoBehaviour
{
    public int coins; // Liczba monet w skrzyni
    private Animator animator; // Animator dla skrzyni
    private bool isOpened = false;

    void Start()
    {
        // Pobieramy Animator z komponentu obiektu skrzyni
        animator = GetComponent<Animator>();
    }

    // Metoda otwierania skrzyni
    public void OpenChest()
    {
        // Sprawdzenie, czy animator jest przypisany
        if (animator != null)
        {
            // Uruchamiamy animacj� otwierania skrzyni (zak�adaj�c, �e masz animacj� o nazwie "Open")
            animator.SetTrigger("Open");
            isOpened = true;
        }
        // Dodajemy monety do gracza
        Debug.Log($"Znaleziono {coins} monet w skrzyni.");

        // Mo�esz tu doda� inne akcje, jak np. przekazanie monet graczowi
    }

    public void CloseChest()
    {
        // Uruchomienie animacji zamykania skrzyni
        if (animator != null)
        {
            animator.SetTrigger("Close"); // Uruchomienie animacji zamykania
        }

        // Zablokowanie interakcji z skrzynk�

        if (isOpened == true)
        {
            // Mo�na r�wnie� wy��czy� mo�liwo�� dalszej interakcji z ni�
            this.GetComponent<Collider>().enabled = false; // Wy��czenie detekcji kolizji
        }
    }
}