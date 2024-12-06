using GG; // Zak�adamy, �e Enemy i Card s� w tym namespace
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class BattleManager : MonoBehaviour
{
    public static BattleManager Instance;

    [Header("Game References")]
    public PlayerDeck PlayerDeck; // Talia gracza
    public Enemy CurrentEnemy; // Aktualny przeciwnik
    public Transform Hand; // Kontener kart w r�ce gracza
    public Transform PlayerCard; // Obiekt docelowy zagranych kart

    [Header("Card Prefabs")]
    public GameObject[] CardPrefabs; // Prefaby kart (Card1, Card2, Card3, Card4)

    private bool playerTurn = false; // Flaga okre�laj�ca, czy jest tura gracza
    private bool battleOngoing = true; // Czy walka trwa
    private Card EnemyPlayedCard; // Karta zagrana przez przeciwnika
    public GameObject playerCharacter;
    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject); 
        }
        else
        {
            Debug.Log($"usuwanie obiektu {gameObject}");
            Destroy(gameObject);
        }
    }
    public void SetPlayerCharacter(GameObject character)
    {
        playerCharacter = character;
    }


    public void SetupBattle(Enemy enemy)
    {
        // Zapewnienie, �e nie tworzysz nowego obiektu BattleManager
        if (Instance != this)
        {
            Debug.LogError("Pr�ba przypisania BattleManager do innego obiektu!");
            return;
        }
        SpawnPlayerCharacter();
        // Logika walki
        CurrentEnemy = enemy;
        Debug.Log($"Rozpocz�cie walki z przeciwnikiem: {enemy.Name} | HP: {enemy.HealthPoints}");
        StartEnemyTurn();
    }
    public void SpawnPlayerCharacter()
    {
        if (playerCharacter != null)
        {
            if (!playerCharacter.activeInHierarchy)
            {
                playerCharacter.SetActive(true); // Aktywowanie postaci na scenie
            }

            // Ustawienie pozycji postaci w scenie walki
            playerCharacter.transform.position = new Vector3(5f, 0f, 0f); // Przyk�adowa pozycja w scenie walki
        }
        else
        {
            Debug.LogError("Posta� gracza nie zosta�a przypisana do BattleManager!");
        }
    }

    private void StartEnemyTurn()
    {
        if (!battleOngoing) return;

        Debug.Log("Tura przeciwnika!");
        playerTurn = false;

        if (CurrentEnemy != null && CurrentEnemy.Deck != null)
        {
            EnemyPlayedCard = CurrentEnemy.Deck.PlayRandomCard();
            if (EnemyPlayedCard != null)
            {
                Debug.Log($"Przeciwnik zagra� kart�: {EnemyPlayedCard.CardTitleText}. Obra�enia: {EnemyPlayedCard.Damage}, Leczenie: {EnemyPlayedCard.Healing}, Blok: {EnemyPlayedCard.Shield}");
            }
        }
        else
        {
            Debug.LogError("Brak przypisanego przeciwnika lub jego talii.");
        }

        // Przej�cie do tury gracza
        Invoke(nameof(StartPlayerTurn), 2.0f);
    }

    private void StartPlayerTurn()
    {
        if (!battleOngoing) return;

        Debug.Log("Tura gracza!");
        playerTurn = true;

        // Przygotowanie r�ki gracza
        DrawCardsForPlayer();
    }

    private void DrawCardsForPlayer()
    {
        // Opr�nij r�k�
        foreach (Transform child in Hand)
        {
            Destroy(child.gameObject);
        }

        // Dodaj 3 losowe karty
        for (int i = 0; i < 3; i++)
        {
            GameObject card = CreateCard();
            card.transform.SetParent(Hand, false);
        }
    }

    private GameObject CreateCard()
    {
        if (CardPrefabs == null || CardPrefabs.Length == 0)
        {
            Debug.LogError("Brak przypisanych prefab�w kart w BattleManager!");
            return null;
        }

        // Wybierz losowy prefab
        int randomIndex = Random.Range(0, CardPrefabs.Length);
        GameObject cardPrefab = CardPrefabs[randomIndex];
        GameObject newCard = Instantiate(cardPrefab);

        // Za�aduj dane karty z talii gracza
        Card cardData = GetRandomCardData();
        Card cardComponent = newCard.GetComponent<Card>();
        if (cardComponent != null && cardData != null)
        {
            cardComponent.SetCardData(cardData);
        }
        else
        {
            Debug.LogWarning("Prefab karty nie posiada komponentu 'Card' lub dane karty s� nieprawid�owe.");
        }

        return newCard;
    }

    private Card GetRandomCardData()
    {
        if (PlayerDeck == null || PlayerDeck.InitialDeck == null || PlayerDeck.InitialDeck.Count == 0)
        {
            Debug.LogError("Talia gracza jest pusta lub nie zosta�a przypisana w PlayerDeck!");
            return null;
        }

        // Wybierz losow� kart� z talii
        int randomIndex = Random.Range(0, PlayerDeck.InitialDeck.Count);
        return PlayerDeck.InitialDeck[randomIndex];
    }

    public void OnPlayerCardSelected(CardClickHandler cardClickHandler)
    {
        if (!playerTurn || !battleOngoing)
        {
            Debug.LogError("Nie mo�esz teraz zagra� karty!");
            return;
        }

        // Przenie� kart� do obiektu PlayerCard
        cardClickHandler.transform.SetParent(PlayerCard, false);

        Debug.Log($"Gracz zagra� kart�: {cardClickHandler.gameObject.name}");

        // Oblicz efekty karty
        CalculateDamage(cardClickHandler.gameObject);
    }

    private void CalculateDamage(GameObject playerCard)
    {
        Card playerCardData = playerCard.GetComponent<Card>();
        if (playerCardData == null)
        {
            Debug.LogError("Wybrana karta nie posiada skryptu 'Card'!");
            return;
        }

        // Obliczenia obra�e�
        int playerDamage = playerCardData.Damage;
        int enemyDamage = EnemyPlayedCard != null ? EnemyPlayedCard.Damage : 0;
        int enemyHealing = EnemyPlayedCard != null ? EnemyPlayedCard.Healing : 0;

        CurrentEnemy.HealthPoints -= playerDamage;
        PlayerStats.Instance.CurrentHealth -= enemyDamage;
        CurrentEnemy.HealthPoints += enemyHealing;

        Debug.Log($"Gracz zadaje {playerDamage} obra�e�. HP przeciwnika: {CurrentEnemy.HealthPoints}");
        Debug.Log($"Przeciwnik zadaje {enemyDamage} obra�e�. HP gracza: {PlayerStats.Instance.CurrentHealth}");

        CheckBattleOutcome();
    }

    private void CheckBattleOutcome()
    {
        if (CurrentEnemy.HealthPoints <= 0)
        {
            EndBattle(true);
        }
        else if (PlayerStats.Instance.CurrentHealth <= 0)
        {
            EndBattle(false);
        }
        else
        {
            StartEnemyTurn();
        }
    }

    public void EndBattle(bool playerWon)
    {
        battleOngoing = false;

        if (playerWon)
        {
            Debug.Log("Gracz wygra� walk�!");
        }
        else
        {
            Debug.Log("Gracz przegra� walk�...");
        }

        // Zako�czenie walki, przej�cie do sceny SampleScene
        SceneManager.LoadScene("SampleScene");

        // Mo�esz tak�e ukry� posta�, je�li chcesz, by nie by�a widoczna w SampleScene
        if (playerCharacter != null)
        {
            playerCharacter.SetActive(false); // Ukrywa posta� przed za�adowaniem nowej sceny
        }

        CurrentEnemy = null;
    }

}
