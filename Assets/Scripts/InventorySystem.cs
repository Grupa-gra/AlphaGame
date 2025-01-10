using UnityEngine;
using UnityEngine.UI;

public class InventorySystem : MonoBehaviour
{
    public GameObject inventoryUI; // Panel ekwipunku
    private bool isInventoryOpen = false;

    [SerializeField] private Button inventoryButton; // Przycisk otwieraj�cy/zamykaj�cy ekwipunek
    [SerializeField] private Button exitButton; // Przycisk zamykaj�cy ekwipunek (Exit)

    void Start()
    {
        // Pod��cz funkcj� ToggleInventory do przycisku inventoryButton (je�li przypisany)
        if (inventoryButton != null)
        {
            inventoryButton.onClick.AddListener(ToggleInventory);
        }

        // Pod��cz funkcj� CloseInventory do przycisku exitButton (je�li przypisany)
        if (exitButton != null)
        {
            exitButton.onClick.AddListener(CloseInventory);
        }
    }

    void Update()
    {
        // Sprawd�, czy gracz nacisn�� klawisz E
        if (Input.GetKeyDown(KeyCode.E))
        {
            ToggleInventory();
        }
    }

    void ToggleInventory()
    {
        isInventoryOpen = !isInventoryOpen; // Prze��cz stan ekwipunku
        inventoryUI.SetActive(isInventoryOpen); // W��cz/wy��cz UI ekwipunku

        // Pauzuj lub wzn�w gr�
        Time.timeScale = isInventoryOpen ? 0f : 1f;
    }

    void CloseInventory()
    {
        if (isInventoryOpen)
        {
            isInventoryOpen = false;
            inventoryUI.SetActive(false); // Wy��cz UI ekwipunku
            Time.timeScale = 1f; // Wzn�w gr�
        }
    }
}