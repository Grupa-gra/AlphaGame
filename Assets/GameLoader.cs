using UnityEngine;

public class GameLoader : MonoBehaviour
{
    [SerializeField] private GameObject player; // Referencja do obiektu gracza
    private Vector3 defaultStartPosition = new(-1.5f, 0.2f, -15.5f); // Domy�lna pozycja startowa

    private void Start()
    {
        // Sprawdzenie, czy GameDataManager.Instance nie jest null
        if (GameDataManager.Instance == null)
        {
            Debug.LogError("GameDataManager.Instance jest null!");
            return;
        }

        // Teraz mo�emy bezpiecznie korzysta� z LoadedSaveData
        if (GameDataManager.Instance.LoadedSaveData != null)
        {
            SaveData saveData = GameDataManager.Instance.LoadedSaveData;

            // Ustawienie pozycji gracza
            Vector3 loadedPosition = new Vector3(
                saveData.playerPositionX,
                saveData.playerPositionY,
                saveData.playerPositionZ
            );

            if (player != null)
            {
                player.transform.position = loadedPosition;
                Debug.Log($"Pozycja gracza za�adowana: {loadedPosition}");
            }
            else
            {
                Debug.LogWarning("Nie przypisano obiektu gracza!");
            }
        }
        else
        {
            // Je�li brak danych, ustaw pozycj� na domy�ln�
            if (player != null)
            {
                GameDataManager.Instance.ClearData();
                player.transform.position = defaultStartPosition;
                Debug.Log($"Rozpocz�to now� gr�. Pozycja gracza ustawiona na: {defaultStartPosition}");
            }
            else
            {
                Debug.LogWarning("Nie przypisano obiektu gracza!");
            }
        }
    }

    private void LoadSavedGame()
    {
        SaveData saveData = GameDataManager.Instance.LoadedSaveData;

        // Ustaw pozycj� gracza z zapisu
        Vector3 loadedPosition = new Vector3(
            saveData.playerPositionX,
            saveData.playerPositionY,
            saveData.playerPositionZ
        );

        if (player != null)
        {
            player.transform.position = loadedPosition;
            Debug.Log($"Pozycja gracza za�adowana: {loadedPosition}");
        }
        else
        {
            Debug.LogWarning("Nie przypisano obiektu gracza!");
        }
    }

    private void StartNewGame()
    {
        // Rozpocznij now� gr� i ustaw domy�ln� pozycj� gracza
        if (player != null)
        {
            player.transform.position = defaultStartPosition;
            Debug.Log($"Rozpocz�to now� gr�. Pozycja gracza ustawiona na: {defaultStartPosition}");
        }
        else
        {
            Debug.LogWarning("Nie przypisano obiektu gracza!");
        }

        // Opcjonalnie, wyczyszcz dane zapisu, je�li chcesz zresetowa� stan gry
        GameDataManager.Instance.ClearData();
    }
}