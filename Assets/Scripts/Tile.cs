using UnityEngine;
using UnityEngine.Tilemaps;

public class Tile : MonoBehaviour
{
    public bool IsAvailableForSelection { get; set; }

    private Renderer tileRenderer;
    private Color originalColor;

    void Awake()
    {
        tileRenderer = GetComponent<Renderer>();
        originalColor = tileRenderer.material.color;
    }

    public void Highlight(Color color)
    {
        tileRenderer.material.color = color;

        if (color == Color.green)
        {
            IsAvailableForSelection = true;
        }
        else if (color == Color.red)
        {
            IsAvailableForSelection = false;
        }
        else
        {
            IsAvailableForSelection = false;
        }
    }

    public bool IsAdjacent(Tile other)
    {
        return Vector3.Distance(transform.position, other.transform.position) == 1;
    }

    public bool HasObstacle()
    {
        Vector3 tilePosition = transform.position;
        Tilemap overGroundTilemap = GameObject.Find("Tilemap_OverGround").GetComponent<Tilemap>();

        Vector3 worldPosition = overGroundTilemap.WorldToCell(tilePosition);
        Collider[] colliders = Physics.OverlapBox(tilePosition, new Vector3(0.5f, 0.5f, 0.5f));

        bool obstacleFound = false;
        foreach (var collider in colliders)
        {
            if (collider != null && collider.CompareTag("Obstacle"))
            {
                obstacleFound = true;
                Debug.Log("Przeszkoda wykryta na polu: " + tilePosition + ", Obiekt: " + collider.gameObject.name);
            }
        }

        if (obstacleFound)
        {
            Debug.Log("Na polu: " + tilePosition + " znajduje si� przeszkoda.");
            return true;
        }

        Debug.Log("Brak przeszkody na polu: " + tilePosition);
        return false;
    }


    public bool HasEvent()
    {
        // Tutaj mo�na sprawdzi�, czy pole ma jakie� zdarzenie (np. walka)
        return false;
    }
}