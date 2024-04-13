using UnityEngine;
using System;
public abstract class Pickable : MonoBehaviour
{
    public Action<Pickable> onPickUp;
    public PickableType pickableType;
    public abstract void PickUp();
}
public enum PickableType
{
    Key,
    Star
}