using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class PanelHoverAlpha : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    private Image panelImage; // Komponent Image panelu
    private Color originalColor;
    private Color hoverColor;

    private void Start()
    {
        // Pobranie komponentu Image tylko z tego obiektu (panelu)
        panelImage = GetComponent<Image>();
        if (panelImage != null)
        {
            // Definicja kolor�w: czarny z r�nym Alpha
            originalColor = new Color(0, 0, 0, 200f / 255f); // Alpha 200
            hoverColor = new Color(0, 0, 0, 245f / 255f); // Alpha 245

            // Ustaw pierwotny kolor
            panelImage.color = originalColor;
        }
        else
        {
            Debug.LogError("Nie znaleziono komponentu Image! Upewnij si�, �e ten obiekt ma komponent Image.");
        }
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        if (panelImage != null)
        {
            panelImage.color = hoverColor; // Ustaw kolor hover
        }
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        if (panelImage != null)
        {
            panelImage.color = originalColor; // Przywr�� pierwotny kolor
        }
    }
}