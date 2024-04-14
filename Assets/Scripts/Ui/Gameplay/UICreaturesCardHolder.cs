using System;
using System.Collections.Generic;
using UnityEngine;
using Gameplay.Creatures;

namespace UI.Gameplay
{
    public class UICreaturesCardHolder : MonoBehaviour
    {
        public event Action<CreatureData> OnCardSelected = delegate { };

        private readonly List<UICreaturesCard> _cardsInHolder = new();

        public void AddCard(UICreaturesCard creaturesCard) 
        {
            _cardsInHolder.Add(creaturesCard);

            creaturesCard.transform.SetParent(transform, false);
            creaturesCard.OnCardSelected += CallCardSelectedEvent;
        }

        private void OnDestroy()
        {
            foreach (var card in _cardsInHolder)
            {
                card.OnCardSelected -= CallCardSelectedEvent;
            }
        }

        public void ShowCards() 
        {
            foreach (var card in _cardsInHolder)
            {
                card.gameObject.SetActive(true);
            }
        }

        public void HideCards() 
        {
            foreach (var card in _cardsInHolder)
            {
                card.gameObject.SetActive(false);
            }
        }

        private void CallCardSelectedEvent(CreatureData creatureData) 
        {
            OnCardSelected?.Invoke(creatureData);
        }
    }
}