using System;
using GG;
using TMPro;
using UnityEngine;

public class GameLoader : MonoBehaviour
{
    [SerializeField] private TMP_Text coinsText;
    [SerializeField] private TMP_Text timeText;
    [SerializeField] private Light directionalLight; // Referencja do �wiat�a
    [SerializeField] private GameObject player; // Referencja do obiektu gracza
    public static event Action OnMapStateLoaded;

    private void Start()
    {
        // Sprawdzenie, czy GameDataManager istnieje i czy zosta� za�adowany zapis
        if (GameDataManager.Instance == null || GameDataManager.Instance.LoadedSaveData == null)
        {
            Debug.LogWarning("Brak wczytanego zapisu gry. Skrypt GameLoader nie zostanie wykonany.");
            return;
        }

        Debug.Log("Wczytano zapis gry. Rozpoczynam �adowanie stanu gry.");
        LoadGameState();
    }

    private void LoadGameState()
    {
        var saveData = GameDataManager.Instance.LoadedSaveData;

        // Ustawienie pozycji gracza
        if (player != null)
        {
            var loadedPosition = new Vector3(
                saveData.playerPositionX,
                saveData.playerPositionY,
                saveData.playerPositionZ
            );
            player.transform.position = loadedPosition;
            Debug.Log($"Pozycja gracza za�adowana: {loadedPosition}");
        }
        else
        {
            Debug.LogWarning("Nie przypisano obiektu gracza!");
        }

        // Ustawienie pozycji i rotacji �wiat�a
        if (directionalLight != null)
        {
            directionalLight.transform.position = new Vector3(
                saveData.lightPositionX,
                saveData.lightPositionY,
                saveData.lightPositionZ
            );

            directionalLight.transform.eulerAngles = new Vector3(
                saveData.lightRotationX,
                saveData.lightRotationY,
                saveData.lightRotationZ
            );

            Debug.Log("Pozycja i rotacja �wiat�a zosta�y za�adowane.");
        }
        else
        {
            Debug.LogWarning("Nie przypisano obiektu �wiat�a!");
        }

        // Ustawienie czasu gry
        if (timeText != null)
        {
            var gameTimeInMinutes = saveData.gameTimeInMinutes;
            var hours = Mathf.FloorToInt(gameTimeInMinutes / 60);
            var minutes = Mathf.FloorToInt(gameTimeInMinutes % 60);
            timeText.text = $"{hours:D2}:{minutes:D2}";
            Debug.Log($"Czas gry ustawiony na: {hours:D2}:{minutes:D2}");
        }
        else
        {
            Debug.LogWarning("Nie przypisano obiektu tekstu do wy�wietlania czasu gry!");
        }

        // Ustawienie liczby monet
        if (coinsText != null)
        {
            coinsText.text = saveData.playerCoins.ToString();
            Debug.Log($"Monety ustawione na: {saveData.playerCoins}");
        }
        else
        {
            Debug.LogWarning("Nie przypisano obiektu tekstu do wy�wietlania monet!");
        }

        // Usuni�cie martwych wrog�w
        if (saveData.enemies != null && saveData.enemies.Count > 0)
        {
            LayerMask overGroundLayer = LayerMask.GetMask("Tilemap_OverGround");

            foreach (var enemyPosition in saveData.enemies)
            {
                var rayOrigin = new Vector3(enemyPosition.x, enemyPosition.y + 30f, enemyPosition.z);
                if (Physics.Raycast(rayOrigin, Vector3.down, out var hit, Mathf.Infinity, overGroundLayer))
                {
                    var enemy = hit.collider.GetComponent<Enemy>();
                    if (enemy != null)
                    {
                        enemy.EnemyDead();
                        Destroy(enemy.gameObject);
                        Debug.Log($"Wr�g na pozycji {enemyPosition} zosta� usuni�ty.");
                    }
                    else
                    {
                        Debug.LogWarning($"Nie znaleziono wroga na pozycji {enemyPosition}.");
                    }
                }
            }
        }

        // Wywo�anie zdarzenia o za�adowaniu mapy
        OnMapStateLoaded?.Invoke();
    }
}
