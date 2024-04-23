using System;
using System.Collections.Generic;
using UnityEngine;
using Gameplay.Creatures;
using TMPro;

namespace UI.Gameplay
{
    public class UICreaturesCardHolder : MonoBehaviour
    {
        public event Action<CreatureSceneData> OnCardSelected = delegate { };

        [Header("Card Slot Position")]
        [SerializeField] private UICardsSlot cardsSlot;
        [SerializeField] private float cardSlotSmoothTime;
        [SerializeField] private float hidePartiallyOffset;
        [SerializeField] private float hideCompletelyOffset;

        [Header("Cards Positions")]
        [SerializeField] private bool rotateCardsFromCenter;
        [SerializeField] private float offsetBetweenCards;
        [SerializeField] private float rotationFromCenterFactor;

        [Header("Offset Curve")]
        [SerializeField] private AnimationCurve yOffsetCurve;
        [SerializeField] private float yOffsetStrenght;

        private readonly List<UICreaturesCard> _cardsInHolder = new();

        private Vector3 _cardsSlotShownPosition = Vector3.zero;
        private Vector3 _cardsSlotHidePartiallyPosition = Vector3.zero;
        private Vector3 _cardsSlotHideCompletelyPosition = Vector3.zero;
        private Vector3 _cardsSlotTarget = Vector3.zero;
        private Vector3 _cardsSlotVelocity = Vector3.zero;
        private bool _cardsSlotLock = false;

        private void Awake()
        {
            _cardsSlotShownPosition = cardsSlot.transform.localPosition;
            _cardsSlotHidePartiallyPosition = new Vector3(_cardsSlotShownPosition.x, _cardsSlotShownPosition.y + hidePartiallyOffset, _cardsSlotShownPosition.z);
            _cardsSlotHideCompletelyPosition = new Vector3(_cardsSlotShownPosition.x, _cardsSlotShownPosition.y + hideCompletelyOffset, _cardsSlotShownPosition.z);
        }

        private void Update()
        {
            cardsSlot.transform.localPosition = Vector3.SmoothDamp(cardsSlot.transform.localPosition, _cardsSlotTarget, ref _cardsSlotVelocity, cardSlotSmoothTime);

            if (!_cardsSlotLock) 
            {
                if (cardsSlot.PointerInSlot)
                    ShowCards();
                else
                    HideCardsPartially();
            }
        }

        private void OnDestroy()
        {
            foreach (var card in _cardsInHolder)
            {
                card.OnCardSelected -= CallCardSelectedEvent;
            }
        }

        public void AddCard(UICreaturesCard creaturesCard)
        {
            _cardsInHolder.Add(creaturesCard);

            creaturesCard.transform.SetParent(cardsSlot.transform, false);
            creaturesCard.ChangeInteractableState(true);
            creaturesCard.OnCardSelected += CallCardSelectedEvent;
        }

        public void ShowCards() 
        {
            foreach (var card in _cardsInHolder)
            {
                card.ChangeInteractableState(true);
            }
            _cardsSlotTarget = _cardsSlotShownPosition;
        }

        public void HideCardsPartially() 
        {
            foreach (var card in _cardsInHolder)
            {
                card.ChangeInteractableState(false);
            }
            _cardsSlotTarget = _cardsSlotHidePartiallyPosition;
        }

        public void HideCardsCompletely() 
        {
            foreach (var card in _cardsInHolder)
            {
                card.ChangeInteractableState(false);
            }
            _cardsSlotTarget = _cardsSlotHideCompletelyPosition;
        }

        public void LockCardSlot() => _cardsSlotLock = true;
        public void UnlockCardSlot() => _cardsSlotLock = false; 

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
                card.ChangeInteractableState(!locked);
            }
        }

        private void UpdateCardPositions() 
        {
            int cardsAmount = _cardsInHolder.Count;

            if(cardsAmount == 0)
                return;

            Vector3 centerPosition = cardsSlot.transform.position;
            Vector3 startPosition = new Vector3(centerPosition.x - offsetBetweenCards * (cardsAmount / (float)2), centerPosition.y, centerPosition.z);
            Vector3 endPosition = new Vector3(centerPosition.x + offsetBetweenCards * (cardsAmount / (float)2), centerPosition.y, centerPosition.z);

            PositionCards(cardsAmount, startPosition, endPosition);

            if(rotateCardsFromCenter)
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