using UnityEngine;

public class GameDataManager : MonoBehaviour
{
    public static GameDataManager Instance;

    public SaveData LoadedSaveData; // Dane za�adowanego zapisu
    public Vector3 playerPosition = new Vector3(-1.5f, 0f, -15.5f);
    public bool IsSaveLoaded { get; set; } = false;
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
        playerPosition = new Vector3(-1.5f, 0f, -15.5f); // Domy�lna pozycja pocz�tkowa
        Debug.Log("Dane gry zresetowane. Nowa gra rozpocz�ta.");
    }

    public void LogGameDataActivity()
    {
        Debug.Log("Game data loaded: " + (LoadedSaveData != null ? "Yes" : "No"));
    }

}