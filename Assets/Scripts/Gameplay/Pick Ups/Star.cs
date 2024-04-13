using UnityEngine;

public class Star : MonoBehaviour, IPickup
{
    public void PickUp()
    {
        //GameManager.StarPickup();
        gameObject.SetActive(false);
    }
}
