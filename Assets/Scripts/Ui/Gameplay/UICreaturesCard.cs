using System;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using Gameplay.Creatures;

namespace UI.Gameplay
{
    public class UICreaturesCard : MonoBehaviour
    {
        public event Action<CreatureSceneData> OnCardSelected = delegate { };
        public CreatureSceneData CreatureSceneData => _creatureSceneData;

        [SerializeField] private Button cardButton;
        [SerializeField] private Image cardCreatureImage;
        [SerializeField] private TextMeshProUGUI cardAmountText;

        private CreatureSceneData _creatureSceneData;

        private void Awake()
        {
            cardButton.onClick.AddListener(CallCardSelectedEvent);
        }

        private void OnDestroy()
        {
            cardButton.onClick.RemoveListener(CallCardSelectedEvent);
        }

        public void SetCard(CreatureSceneData creatureSceneData) 
        {
            _creatureSceneData = creatureSceneData;
            UpdateCard();
        }

        public void ChangeLockState(bool locked) => cardButton.interactable = !locked;

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

    }
}