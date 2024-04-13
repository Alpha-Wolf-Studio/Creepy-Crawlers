using UnityEngine;

public class Key : MonoBehaviour, IPickup
{
    public void PickUp()
    {
        //GameManager.KeyPickup(id);
        gameObject.SetActive(false);
    }
}
