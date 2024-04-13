using UnityEngine;

namespace CustomPlayFabAPI
{
    public static class Initializer
    {
        [RuntimeInitializeOnLoadMethod]
        private static void Initialize()
        {
            var customPlayFabSingleton = Resources.Load<CustomPlayFabSingleton>("Custom PlayFab API Singleton");
            Object.Instantiate(customPlayFabSingleton);
        }
    }
}
