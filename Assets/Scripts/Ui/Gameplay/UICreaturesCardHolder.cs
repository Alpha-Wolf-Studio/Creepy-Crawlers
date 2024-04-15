using System;
using System.Collections.Generic;
using UnityEngine;
using Gameplay.Creatures;

namespace UI.Gameplay
{
    public class UICreaturesCardHolder : MonoBehaviour
    {
        public event Action<CreatureSceneData> OnCardSelected = delegate { };

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

        public void UpdateCardsData() 
        {
            foreach (var card in _cardsInHolder)
            {
                card.UpdateCard();
            }

            List<UICreaturesCard> cardsToRemove = _cardsInHolder.FindAll(i => i.CreatureSceneData.CreatureAmount <= 0);
            foreach (var card in cardsToRemove)
            {
                _cardsInHolder.Remove(card);
                Destroy(card.gameObject);
            }
        }

        public void ChangeCardsLockState(bool locked)
        {
            foreach (var card in _cardsInHolder)
            {
                card.ChangeLockState(locked);
            }
        }

        private void CallCardSelectedEvent(CreatureSceneData creatureSceneData) 
        {
            OnCardSelected?.Invoke(creatureSceneData);
        }
    }
}