using System.Collections.Generic;
using UnityEngine;

namespace GG
{
    public class PlayerDeck : MonoBehaviour
    {
        public List<Card> InitialDeck; // Startowe karty
        private List<Card> deck; // Aktualna talia
        private List<Card> discardPile; // Stos kart odrzuconych
        private List<Card> hand; // Aktualna r�ka

        public Transform HandContainer; // Kontener dla kart w r�ce
        public CardViz CardPrefab; // Prefab karty do wy�wietlenia w r�ce
        public int MaxHandSize = 3;

        void Start()
        {
            // Inicjalizacja talii
            ResetDeck();
            hand = new List<Card>();
            discardPile = new List<Card>();

            DrawStartingHand();
        }

        public void ResetDeck()
        {
            deck = new List<Card>(InitialDeck);

            // Usu� karty aktualnie w r�ce z nowej talii
            foreach (var card in hand)
            {
                deck.Remove(card);
            }
        }

        public void DrawStartingHand()
        {
            for (int i = 0; i < MaxHandSize; i++)
            {
                DrawCard();
            }
        }

        public void DrawCard()
        {
            if (deck.Count == 0)
            {
                ResetDeck();
            }

            if (deck.Count > 0 && hand.Count < MaxHandSize)
            {
                int randomIndex = Random.Range(0, deck.Count);
                Card drawnCard = deck[randomIndex];
                deck.RemoveAt(randomIndex);
                hand.Add(drawnCard);

                // Dodaj wizualizacj� do r�ki
                if (HandContainer != null && CardPrefab != null)
                {
                    CardViz cardViz = Instantiate(CardPrefab, HandContainer);
                    cardViz.LoadCard(drawnCard);

                    // Dodaj obs�ug� klikni�cia
                    var clickHandler = cardViz.gameObject.AddComponent<CardClickHandler>();
                    clickHandler.OnCardClicked = (clickedCard) => PlayCard(drawnCard, cardViz);
                }
            }
        }


        public void PlayCard(Card card, CardViz cardViz)
        {
            if (!hand.Contains(card)) return;

            // Przelicz obra�enia (implementacja zale�y od mechaniki gry)
            Debug.Log($"Zagrano kart�: {card.CardTitleText}");

            // Przenie� kart� do stosu kart odrzuconych
            hand.Remove(card);
            Debug.Log($"usunieta karta");
            discardPile.Add(card);
            Debug.Log($"na stos odrzuconych:");

            // Usu� wizualizacj�
            Destroy(cardViz.gameObject);

            // Dobierz now� kart�
            DrawCard();
        }
    }
}
