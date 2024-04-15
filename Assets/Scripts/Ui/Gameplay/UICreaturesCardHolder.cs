using System;
using System.Collections.Generic;
using UnityEngine;
using Gameplay.Creatures;

namespace UI.Gameplay
{
    public class UICreaturesCardHolder : MonoBehaviour
    {
        public event Action<CreatureSceneData> OnCardSelected = delegate { };

        [Header("Cards Positions")]
        [SerializeField] private float offsetBetweenCards;
        [SerializeField] private float rotationFromCenterFactor;

        [Header("Offset Curve")]
        [SerializeField] private AnimationCurve yOffsetCurve;
        [SerializeField] private float yOffsetStrenght;

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

        public void UpdateCards() 
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

            UpdateCardPositions();
        }

        public void ChangeCardsLockState(bool locked)
        {
            foreach (var card in _cardsInHolder)
            {
                card.ChangeLockState(locked);
            }
        }

        private void UpdateCardPositions() 
        {
            int cardsAmount = _cardsInHolder.Count;

            if(cardsAmount == 0)
                return;

            Vector3 centerPosition = transform.position;
            Vector3 startPosition = new Vector3(centerPosition.x - offsetBetweenCards * (cardsAmount / (float)2), centerPosition.y, centerPosition.z);
            Vector3 endPosition = new Vector3(centerPosition.x + offsetBetweenCards * (cardsAmount / (float)2), centerPosition.y, centerPosition.z);

            PositionCards(cardsAmount, startPosition, endPosition);
            RotateCards(cardsAmount, centerPosition);
        }

        private void PositionCards(int cardsAmount, Vector3 startPosition, Vector3 endPosition) 
        {
            for (int i = 0; i < cardsAmount; i++)
            {
                float t = (i + .5f) / cardsAmount;

                Vector3 newPosition = Vector3.Lerp(startPosition, endPosition, (i + .5f) / cardsAmount);
                newPosition.y -= yOffsetCurve.Evaluate(t) * yOffsetStrenght;

                _cardsInHolder[i].transform.position = newPosition;
            }
        }

        private void RotateCards(int cardsAmount, Vector3 centerPosition) 
        {
            Vector3 inclinationPoint = new Vector3(centerPosition.x, centerPosition.y - rotationFromCenterFactor, centerPosition.z);

            for (int i = 0; i < cardsAmount; i++)
            {
                Vector3 upDirection = (_cardsInHolder[i].transform.position - inclinationPoint).normalized;
                _cardsInHolder[i].transform.up = upDirection;
            }
        }

        private void CallCardSelectedEvent(CreatureSceneData creatureSceneData) 
        {
            OnCardSelected?.Invoke(creatureSceneData);
        }
    }
}