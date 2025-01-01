using UnityEngine;

public class GameDataManager : MonoBehaviour
{
    public static GameDataManager Instance;

    public SaveData LoadedSaveData; // Dane za�adowanego zapisu
    public Vector3 playerPosition;
    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
            //Debug.Log("SaveSystem initialized GAMEDATAMANAGER.");  // Dodaj logi w Awake()
        }
        else
        {
            Destroy(gameObject);
            //Debug.Log("SaveSystem already exists GAMEDATAMANAGER.");  // Je�li instancja ju� istnieje, usuwamy obiekt
        }
    }

    // Funkcja, kt�ra sprawdza, czy dane zosta�y ju� za�adowane z zapisu
    public void LoadGameData(SaveData saveData)
    {
        if (saveData != null)
        {
            LoadedSaveData = saveData;
            playerPosition = new Vector3(saveData.playerPositionX, saveData.playerPositionY, saveData.playerPositionZ);
            Debug.Log("Dane gry za�adowane.");
        }
        else
        {
            Debug.Log("Brak danych do za�adowania. U�ywam domy�lnych warto�ci.");
            ClearData();
        }
    }

    public void ClearData()
    {
        LoadedSaveData = null;
        playerPosition = new Vector3(-1.5f, 10f, -15.5f); // Domy�lna pozycja pocz�tkowa
        Debug.Log("Dane gry zresetowane. Nowa gra rozpocz�ta.");
    }

    public void LogGameDataActivity()
    {
        Debug.Log("Game data loaded: " + (LoadedSaveData != null ? "Yes" : "No"));
    }

}