using UnityEngine;

public class Key : Pickable
{
    public override void PickUp()
    {
        onPickUp?.Invoke(this);
        gameObject.SetActive(false);
    }

}