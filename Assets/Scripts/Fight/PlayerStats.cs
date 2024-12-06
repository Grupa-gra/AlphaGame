using UnityEngine;

public class PlayerStats : MonoBehaviour
{
    public static PlayerStats Instance;

    [Header("Statystyki Gracza")]
    public int MaxHealth = 10; // Maksymalne zdrowie gracza
    public int CurrentHealth;   // Aktualne zdrowie gracza


    private void Awake()
    {
        // Singleton - zapewnia jedn� instancj�
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject); // Utrzymanie obiektu mi�dzy scenami
        }
        else
        {
            Destroy(gameObject);
        }
    }

    private void Start()
    {
        // Ustawienie pocz�tkowego zdrowia na maksymalne
        CurrentHealth = MaxHealth;
    }

    public void TakeDamage(int damage)
    {
        
        CurrentHealth -= damage;
        Debug.Log($"Gracz otrzyma� {damage} obra�e�. Pozosta�e HP: {CurrentHealth}");

        if (CurrentHealth <= 0)
        {
            CurrentHealth = 0;
            Debug.Log("Gracz zosta� pokonany!");
            // Mo�esz doda� tutaj dodatkowe efekty, np. koniec gry
        }
    }

    public void Heal(int amount)
    {
        CurrentHealth = Mathf.Min(CurrentHealth + amount, MaxHealth); // Nie przekraczaj maksymalnego zdrowia
        Debug.Log($"Gracz zosta� uleczony o {amount}. Aktualne HP: {CurrentHealth}");
    }
}
