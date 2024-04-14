using System;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using Gameplay.Creatures;

namespace UI.Gameplay
{
    public class UICreaturesCard : MonoBehaviour
    {
        public event Action<CreatureData> OnCardSelected = delegate { };

        [SerializeField] private Button cardButton;
        [SerializeField] private TextMeshProUGUI cardText;
        [SerializeField] private Image cardImage;

        private CreatureData _creatureData;

        private void Awake()
        {
            cardButton.onClick.AddListener(CallCardSelectedEvent);
        }

        private void OnDestroy()
        {
            cardButton.onClick.RemoveListener(CallCardSelectedEvent);
        }

        public void SetCard(CreatureData data) 
        {
            _creatureData = data;

            if(cardText)
                cardText.text = data.creatureName;

            if (cardImage)
                cardImage.sprite = data.creatureThumbnail;
        }

        private void CallCardSelectedEvent() 
        {
            OnCardSelected?.Invoke(_creatureData);
        }
    }
}