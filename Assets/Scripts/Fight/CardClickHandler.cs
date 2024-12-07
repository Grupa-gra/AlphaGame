using UnityEngine;
using UnityEngine.EventSystems;

public class CardClickHandler : MonoBehaviour, IPointerClickHandler
{
    public System.Action<CardClickHandler> OnCardClicked;

    private Transform handContainer;

    void Start()
    {
        // Przyk�ad przypisania handContainer, zak�adaj�c, �e masz obiekt kontenera kart w scenie
        Transform container = GameObject.Find("HandContainer")?.transform;

        if (container != null)
        {
            SetHandContainer(container);
        }
        else
        {
            Debug.LogError("Nie znaleziono obiektu 'HandContainer' w scenie!");
        }
    }


    public void OnPointerClick(PointerEventData eventData)
    {
        // Sprawdzenie, czy handContainer jest przypisany
        if (handContainer == null)
        {
            Debug.LogError("HandContainer nie jest przypisany dla tej karty!");
            return;
        }

        // Sprawdzenie, czy karta jest w r�ce
        if (transform.IsChildOf(handContainer))
        {
            OnCardClicked?.Invoke(this);
            Debug.Log($"Klikni�to kart�: {gameObject.name}");
        }
        else
        {
            Debug.LogWarning("Nie mo�esz klikn�� karty, kt�ra nie znajduje si� w r�ce gracza.");
        }
    }

    // Funkcja do ustawienia handContainer
    public void SetHandContainer(Transform container)
    {
        if (container == null)
        {
            Debug.LogError("Kontener kart jest null! Nie mog� ustawi� handContainer.");
            return;
        }

        handContainer = container;
        Debug.Log("Kontener zosta� przypisany: " + handContainer.name);
    }
}