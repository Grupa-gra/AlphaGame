using UnityEngine;
using System.Collections.Generic;

namespace GG
{
    [CreateAssetMenu(menuName = "Card")]
    public class Card : ScriptableObject
    {
        public string CardTitleText;
        public Sprite CardImage;
        public List<Sprite> CardEffectImagePrimary;
        public List<Sprite> CardEffectImageSecondary;

        // Statystyki karty
        public int Damage { get; private set; }
        public int Shield { get; private set; }
        public int Healing { get; private set; }
        public bool IgnoreBlock { get; private set; }

        // Mapa efekt�w i ich warto�ci
        private static Dictionary<string, CardEffect> effectMapping = new Dictionary<string, CardEffect>
        {
            { "Damage", new CardEffect { Damage = 1 } },
            { "Shield", new CardEffect { Shield = 1 } },
            { "Healing", new CardEffect { Healing = 1 } },
            { "IgnoreBlock", new CardEffect { IgnoreBlock = true } }
        };

        public void CalculateStats()
        {
            // Reset statystyk
            Damage = 0;
            Shield = 0;
            Healing = 0;
            IgnoreBlock = false;

            // Przetwarzanie efekt�w primary
            if (CardEffectImagePrimary != null && CardEffectImagePrimary.Count > 0)
            {
                ProcessEffects(CardEffectImagePrimary);
            }

            // Przetwarzanie efekt�w secondary
            if (CardEffectImageSecondary != null && CardEffectImageSecondary.Count > 0)
            {
                ProcessEffects(CardEffectImageSecondary);
            }

            Debug.Log($"Karta: {CardTitleText} | Obra�enia: {Damage}, Tarcza: {Shield}, Leczenie: {Healing}, Ignoruj Blok: {IgnoreBlock}");
        }

        private void ProcessEffects(List<Sprite> effectSprites)
        {
            foreach (var sprite in effectSprites)
            {
                //Debug.Log($"Pr�ba znalezienia efektu dla: {sprite.name}");

                if (sprite != null && effectMapping.TryGetValue(sprite.name, out CardEffect effect))
                {
                    // Logowanie warto�ci, kt�re zosta�y przypisane
                    Debug.Log($"Efekt: {sprite.name} -> Damage: {effect.Damage}, Shield: {effect.Shield}, Healing: {effect.Healing}, IgnoreBlock: {effect.IgnoreBlock}");

                    Damage += effect.Damage;
                    Shield += effect.Shield;
                    Healing += effect.Healing;
                    if (effect.IgnoreBlock)
                        IgnoreBlock = true;
                }
                else
                {
                    Debug.LogWarning($"Efekt {sprite?.name ?? "null"} nie zosta� zmapowany.");
                }

            }
        }
        public void SetCardData(Card cardData)
        {
            if (cardData == null) return;

            CardTitleText = cardData.CardTitleText;
            Damage = cardData.Damage;
            Healing = cardData.Healing;
            Shield = cardData.Shield;

            // Dostosuj wizualizacj� na podstawie danych
            //UpdateCardVisuals();
        }
        private void UpdateCardVisuals()
        {
            // Zaktualizuj teksty, obrazy itp., aby odzwierciedla�y dane karty
            Debug.Log($"Karta: {CardTitleText}, DMG: {Damage}, HEAL: {Healing}, SHIELD: {Shield}");
        }

    }

    // Klasa efekt�w
    public class CardEffect
    {
        public int Damage;
        public int Shield;
        public int Healing;
        public bool IgnoreBlock;
    }
}
