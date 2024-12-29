using UnityEngine;

public class PlayerInfo : MonoBehaviour
{
    public static PlayerInfo Instance; // Singleton

    public int maxHealth = 5; // Bazowe zdrowie

    private void Awake()
    {
        // Je�li instancja ju� istnieje, usu� ten obiekt (zapewnia jedn� instancj�)
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
        }
        else
        {
            Instance = this;
            DontDestroyOnLoad(gameObject); // Zapewnia, �e ten obiekt przetrwa prze�adowanie sceny
        }
    }

    // Funkcja zmieniaj�ca maksymalne zdrowie
    public void ChangeMaxHealth(int amountToIncrease)
    {
        maxHealth += amountToIncrease;
        Debug.Log("Nowe maksymalne zdrowie: " + maxHealth);
    }
}