using System;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using Gameplay.Creatures;
using UnityEngine.EventSystems;
using System.Collections;

namespace UI.Gameplay
{
    public class UICreaturesCard : MonoBehaviour, IPointerClickHandler, IPointerEnterHandler, IPointerExitHandler
    {
        public event Action<CreatureSceneData> OnCardSelected = delegate { };
        public CreatureSceneData CreatureSceneData => _creatureSceneData;

        [SerializeField] private Image cardCreatureImage;
        [SerializeField] private TextMeshProUGUI cardAmountText;
        [SerializeField] private float cardSelectionScale = 1.25f;

        private CreatureSceneData _creatureSceneData;
        private bool _pointerOnCard;
        private bool _interactable;

        private void Update()
        {
            if (_pointerOnCard) 
            {
                Vector3 newScale = Vector3.one;
                newScale.x *= cardSelectionScale;
                newScale.y *= cardSelectionScale;
                transform.localScale = newScale;
            }
            else 
            {
                transform.localScale = Vector3.one;
            }
        }

        public void SetCard(CreatureSceneData creatureSceneData) 
        {
            _creatureSceneData = creatureSceneData;
            UpdateCard();
        }

        public void ChangeInteractableState(bool interactable) => _interactable = interactable;

        public void UpdateCard() 
        {
            if (cardCreatureImage)
                cardCreatureImage.sprite = _creatureSceneData.CreatureData.creatureCardSprite;

            if (cardAmountText)
                cardAmountText.text = _creatureSceneData.CreatureAmount.ToString();
        }

        private void CallCardSelectedEvent() 
        {
            OnCardSelected?.Invoke(_creatureSceneData);
        }

        public void OnPointerClick(PointerEventData eventData)
        {
            if (_interactable)
                CallCardSelectedEvent();
        }

        public void OnPointerEnter(PointerEventData eventData) => _pointerOnCard = true;

        public void OnPointerExit(PointerEventData eventData) => _pointerOnCard = false;
    }
}