using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class TileSelector : MonoBehaviour
{
    public Tile startingTile;
    public int maxSteps;
    public GameObject character;
    public Animator characterAnimator;
    public Tilemap tilemapGround;  // Tilemap_Ground
    public Tilemap tilemapOverGround; // Tilemap_OverGround (przeszkody)
    public bool isActive = true;

    private List<Tile> selectedTiles = new List<Tile>();

    void Start()
    {

    }

    void Update()
    {
        if (!isActive) return;
        if (Input.GetMouseButtonDown(0))
        {
            Debug.Log("LPM klikni�ty - pr�ba zaznaczenia pola.");
            TrySelectTile();
        }
        else if (Input.GetMouseButtonDown(1))
        {
            Debug.Log("PPM klikni�ty - pr�ba odznaczenia pola.");
            TryDeselectTile();
        }
    }

    public void SetMaxSteps(int steps)
    {
        maxSteps = steps;
        Debug.Log("Max Steps ustawione na: " + maxSteps);
    }
    public void HighlightPotentialMoves()
    {
        UpdateTileHighlights();
    }

    private void TrySelectTile()
    {
        if (selectedTiles.Count >= maxSteps)
        {
            Debug.Log("Osi�gni�to maksymaln� liczb� zaznacze�.");
            return;
        }

        Tile tile = GetTileUnderMouse();
        if (tile == null)
        {
            Debug.Log("Nie wykryto �adnego pola pod myszk�.");
            return;
        }

        if (selectedTiles.Contains(tile))
        {
            Debug.Log("To pole jest ju� zaznaczone.");
            return;
        }

        bool isAdjacent = selectedTiles.Count == 0
            ? tile.IsAdjacent(startingTile)
            : tile.IsAdjacent(selectedTiles[selectedTiles.Count - 1]);

        if (isAdjacent)
        {
            Debug.Log("Pole przyleg�e: " + tile.name);

            if (!tile.HasObstacle() && tile.IsAvailableForSelection)
            {
                tile.Highlight(Color.cyan);
                selectedTiles.Add(tile);
                Debug.Log("Pole zaznaczone: " + tile.name);
            }
            else
            {
                Debug.Log("Pole ma przeszkod� lub jest niedost�pne: " + tile.name);
            }
        }
        else
        {
            Debug.Log("Pole nie jest przyleg�e do ostatniego zaznaczonego.");
        }

        UpdateTileHighlights();
    }

    private void TryDeselectTile()
    {
        Tile tile = GetTileUnderMouse();
        if (tile == null)
        {
            Debug.Log("Nie wykryto �adnego pola pod myszk� do odznaczenia.");
            return;
        }

        if (!selectedTiles.Contains(tile))
        {
            Debug.Log("To pole nie jest zaznaczone.");
            return;
        }

        int index = selectedTiles.IndexOf(tile);
        for (int i = selectedTiles.Count - 1; i >= index; i--)
        {
            selectedTiles[i].Highlight(Color.white);
            Debug.Log("Pole odznaczone: " + selectedTiles[i].name);
            selectedTiles.RemoveAt(i);
        }

        UpdateTileHighlights();
    }

    private Tile GetTileUnderMouse()
    {
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        if (Physics.Raycast(ray, out RaycastHit hit))
        {
            Tile tile = hit.collider.GetComponent<Tile>();
            Debug.Log("Pole pod myszk�: " + (tile != null ? tile.name : "brak"));
            return tile;
        }
        Debug.Log("Brak kolizji pod myszk�.");
        return null;
    }

    public void OnReadyButtonClicked()
    {
        Debug.Log("Przycisk 'Ready' klikni�ty - rozpoczynanie ruchu.");
        StartCoroutine(MoveCharacterAlongPath());
    }

    private System.Collections.IEnumerator MoveCharacterAlongPath()
    {
        try
        {
            foreach (Tile tile in selectedTiles)
            {
                characterAnimator.SetBool("isWalking", true);
                Debug.Log("Animacja chodzenia ustawiona na true");

                Vector3 targetPosition = tile.transform.position;
                targetPosition.y = character.transform.position.y;

                Vector3 direction = targetPosition - character.transform.position;
                direction.y = 0;

                Quaternion targetRotation = Quaternion.identity;
                if (Mathf.Abs(direction.x) > Mathf.Abs(direction.z))
                {
                    targetRotation = direction.x > 0
                        ? Quaternion.Euler(0, 90, 0)
                        : Quaternion.Euler(0, 270, 0);
                }
                else
                {
                    targetRotation = direction.z > 0
                        ? Quaternion.Euler(0, 0, 0)
                        : Quaternion.Euler(0, 180, 0);
                }

                Debug.Log("Rotacja ustawiona na: " + targetRotation.eulerAngles);

                while (Vector3.Distance(character.transform.position, targetPosition) > 0.1f)
                {
                    character.transform.rotation = Quaternion.Slerp(character.transform.rotation, targetRotation, Time.deltaTime * 5);
                    character.transform.position = Vector3.MoveTowards(character.transform.position, targetPosition, Time.deltaTime * 2);
                    yield return null;
                }

                characterAnimator.SetBool("isWalking", false);
                Debug.Log("Animacja chodzenia ustawiona na false");

                Debug.Log("Posta� przesz�a na pole: " + tile.name);

                if (tile.HasEvent())
                {
                    Debug.Log("Wykryto zdarzenie na polu " + tile.name + ". Zatrzymanie ruchu.");
                    yield break;
                }
            }
        }
        finally
        {
            Debug.Log("Ruch zako�czony. Czyszczenie zaznaczenia potencjalnych ruch�w.");

            ClearPotentialMoveHighlights();

            if (selectedTiles.Count > 0)
            {
                startingTile = selectedTiles[selectedTiles.Count - 1];
            }

            foreach (Tile tile in selectedTiles)
            {
                tile.Highlight(Color.white);
            }
            selectedTiles.Clear();
        }
    }
    private void ClearPotentialMoveHighlights()
    {
        foreach (Tile tile in UnityEngine.Object.FindObjectsByType<Tile>(UnityEngine.FindObjectsSortMode.None))
        {
            if (!selectedTiles.Contains(tile))
            {
                tile.Highlight(Color.white);
                tile.IsAvailableForSelection = false;
            }
        }
    }

    private void UpdateTileHighlights()
    {
        // Resetujemy wszystkie kafelki na bia�e
        foreach (Tile tile in UnityEngine.Object.FindObjectsByType<Tile>(UnityEngine.FindObjectsSortMode.None))
        {
            if (!selectedTiles.Contains(tile))
            {
                tile.Highlight(Color.white);
                tile.IsAvailableForSelection = false; // Domy�lnie pole nie jest dost�pne
            }
        }

        // Znajd� przyleg�e do ostatniego zaznaczonego lub startowego
        Tile referenceTile = selectedTiles.Count > 0 ? selectedTiles[selectedTiles.Count - 1] : startingTile;

        // Iterujemy po wszystkich kafelkach
        foreach (Tile tile in UnityEngine.Object.FindObjectsByType<Tile>(UnityEngine.FindObjectsSortMode.None))
        {
            if (tile == referenceTile || selectedTiles.Contains(tile))
                continue;  // Pomijamy ju� zaznaczone kafelki

            // Sprawdzamy, czy pole jest s�siednie do ostatniego zaznaczonego
            if (tile.IsAdjacent(referenceTile))
            {
                Debug.Log("Sprawdzanie s�siedniego pola: " + tile.name);

                // Sprawdzamy, czy na polu nie ma przeszk�d oraz czy jest dost�pne do zaznaczenia
                bool hasObstacle = tile.HasObstacle();
                bool isInMaxRange = selectedTiles.Count < maxSteps;

                if (hasObstacle)
                {
                    Debug.Log("Pole ma przeszkod�: " + tile.name);
                    tile.Highlight(Color.red);
                    tile.IsAvailableForSelection = false;
                }
                else if (!isInMaxRange)
                {
                    Debug.Log("Pole poza dozwolonym zasi�giem: " + tile.name);
                    tile.Highlight(Color.red);
                    tile.IsAvailableForSelection = false;
                }
                else
                {
                    tile.Highlight(Color.green);
                    tile.IsAvailableForSelection = true;
                    Debug.Log("Pole dost�pne do zaznaczenia: " + tile.name);
                }
            }
        }
    }



}
