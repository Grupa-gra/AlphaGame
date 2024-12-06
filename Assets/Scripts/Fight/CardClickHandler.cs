using UnityEngine;
using UnityEngine.EventSystems;

public class CardClickHandler : MonoBehaviour, IPointerClickHandler
{
    public System.Action<CardClickHandler> OnCardClicked;

    private Transform handContainer;

    public void OnPointerClick(PointerEventData eventData)
    {
        if (transform.parent == null)
        {
            Debug.LogError("Rodzic karty jest null!");
            return;
        }
        // Sprawd�, czy karta jest w r�ce gracza
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

    public void SetHandContainer(Transform container)
    {
        handContainer = container;
    }
}