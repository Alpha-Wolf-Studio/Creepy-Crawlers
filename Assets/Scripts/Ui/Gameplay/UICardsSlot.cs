using UnityEngine;
using UnityEngine.EventSystems;

public class UICardsSlot : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    public bool PointerInSlot { get; private set; }

    public void OnPointerEnter(PointerEventData eventData) => PointerInSlot = true;

    public void OnPointerExit(PointerEventData eventData) => PointerInSlot = false;
}
