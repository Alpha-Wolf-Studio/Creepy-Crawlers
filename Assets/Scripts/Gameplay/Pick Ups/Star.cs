public class Star : Pickable
{
    public override void PickUp()
    {
        onPickUp?.Invoke(this);
        gameObject.SetActive(false);
    }
}
