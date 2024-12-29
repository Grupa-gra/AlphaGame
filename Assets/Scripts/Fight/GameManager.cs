using UnityEngine;

namespace GG
{
    public class GameManager : MonoBehaviour
    {
        // Singleton
        public static GameManager Instance { get; private set; }

        // Przechowywanie danych o przeciwniku
        public EnemyBattleData CurrentEnemyData { get; private set; }

        // Talie kart dla r�nych poziom�w trudno�ci
        public Deck Deck_diff_1;
        public Deck Deck_diff_2;
        public Deck Deck_diff_3;

        private void Awake()
        {
            // Sprawdzenie, czy istnieje ju� instancja GameManagera
            if (Instance != null && Instance != this)
            {
                Destroy(gameObject); // Usuwamy duplikat
            }
            else
            {
                Instance = this; // Ustawiamy instancj�
                DontDestroyOnLoad(gameObject); // Zachowujemy GameManager mi�dzy scenami
            }
        }

        // Funkcja do ustawiania danych przeciwnika i przypisania odpowiedniej talii
        public void SetCurrentEnemy(Enemy enemy)
        {
            if (enemy != null)
            {
                CurrentEnemyData = new EnemyBattleData
                {
                    HealthPoints = enemy.HealthPoints,
                    DifficultyLevel = enemy.DifficultyLevel,
                    Name = enemy.Name
                };

                Debug.Log("Przeciwnik przypisany: " + enemy.Name);
                Debug.Log("Zdrowie przeciwnika: " + enemy.HealthPoints);
                Debug.Log("Poziom trudno�ci przeciwnika: " + enemy.DifficultyLevel);

                // Przypisanie talii przeciwnika na podstawie poziomu trudno�ci
                AssignEnemyDeck(enemy.DifficultyLevel);
            }
            else
            {
                Debug.LogError("Brak przeciwnika do przypisania!");
            }
        }

        // Funkcja przypisuj�ca tali� w zale�no�ci od poziomu trudno�ci przeciwnika
        private void AssignEnemyDeck(int difficultyLevel)
        {
            Deck enemyDeck = null;

            switch (difficultyLevel)
            {
                case 1: // �atwy poziom
                    enemyDeck = Deck_diff_1;
                    break;
                case 2: // �redni poziom
                    enemyDeck = Deck_diff_2;
                    break;
                case 3: // Trudny poziom
                    enemyDeck = Deck_diff_3;
                    break;
                default:
                    Debug.LogError("Nieznany poziom trudno�ci!");
                    break;
            }

            if (enemyDeck != null)
            {
                Debug.Log($"Przypisano tali� dla przeciwnika: {enemyDeck.name}");
                // Mo�esz teraz przypisa� tali� do przeciwnika lub wykona� inne operacje
                // Przyk�ad: enemy.SetDeck(enemyDeck); - w zale�no�ci od tego, jak masz zaimplementowanego przeciwnika
            }
        }
    }

    // Klasa przechowuj�ca dane przeciwnika w walce
    [System.Serializable]
    public class EnemyBattleData
    {
        public int HealthPoints;
        public int DifficultyLevel;
        public string Name;
    }
}
