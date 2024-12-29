using UnityEngine;
using UnityEngine.EventSystems;

namespace GG
{
    public class CardClickHandler : MonoBehaviour, IPointerClickHandler
    {
        private BattleManager battleManager;
        private Card card;
        private GameObject cardObject;

        // Inicjalizacja
        public void Initialize(BattleManager manager, Card card, GameObject cardObj)
        {
            battleManager = manager;
            this.card = card;
            this.cardObject = cardObj;
        }

        // Obs�uguje klikni�cie na kart�
        public void OnPointerClick(PointerEventData eventData)
        {
            
            if (battleManager != null && card != null)
            {
                if (transform.IsChildOf(battleManager.HandContainer) && battleManager.PlayerCardContainer.childCount == 0)
                {
                    battleManager.PlayCard(card, cardObject);
                    Debug.Log($"Klikni�to kart�: {gameObject.name}");
                }
                else if (transform.IsChildOf(battleManager.PlayerCardContainer) && battleManager.isTurnInProgress)
                {
                    // Je�li tura trwa, przywracamy kart� do r�ki
                    battleManager.OnPlayerCardClicked(cardObject);
                    Debug.Log($"Karta {gameObject.name} przeniesiona z powrotem do r�ki.");
                }
                else
                {
                    Debug.LogWarning("Nie mo�esz klikn�� karty, kt�ra nie znajduje si� w r�ce, zosta�a ju� zagrana lub tura jest w toku.");
                }
            }
        }
    }
}