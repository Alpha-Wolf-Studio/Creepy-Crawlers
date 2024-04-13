using System.Collections.Generic;

namespace CustomPlayFabAPI.Data
{
    public class CustomUserData : IUserData
    {
        private const string LoginAmountKey = "LOGIN_AMOUNT"; 
        private const string TimePlayedKey = "TIME_PLAYED";
        private const string Item1Key = "ITEM1KEY";
        private const string Item2Key = "ITEM2KEY";
        private const string Item3Key = "ITEM3KEY";
        private const string Item4Key = "ITEM4KEY";
        
        public Dictionary<string, string> FullDataDictionary { get; private set; }

        public int TimePlayed { get; private set; } = 0;
        public int LoginAmount { get; private set; } = 0;
        public float Item1Amount { get; private set; } = 0;
        public bool Item2Amount { get; private set; } = false;
        public int Item3Amount { get; private set; } = 0;
        public int Item4Amount { get; private set; } = 0;

        public void AddToLogInCounter() => LoginAmount++;
        public void AddTimePlayed(int time) => TimePlayed += time;
        
        public void ParseDataFromDictionary(Dictionary<string, string> userData)
        {
            if (userData.TryGetValue(TimePlayedKey, out string valueTime))
                TimePlayed = int.Parse(valueTime);

            if(userData.TryGetValue(LoginAmountKey, out string valueLogin))
                LoginAmount = int.Parse(valueLogin);

            //if(userData.TryGetValue(Item1Key, out string valueItem1))
            //    Item1Amount = float.Parse(valueItem1);
            //
            //if(userData.TryGetValue(Item2Key, out string valueItem2))
            //    Item2Amount = bool.Parse(valueItem2);
            //
            //if(userData.TryGetValue(Item3Key, out string valueItem3))
            //    Item3Amount = int.Parse(valueItem3);
            //
            //if(userData.TryGetValue(Item4Key, out string valueItem4))
            //    Item4Amount = int.Parse(valueItem4);
        }

        public void ParseDataToDictionary()
        {
            FullDataDictionary = new Dictionary<string, string>
            {
                { LoginAmountKey, LoginAmount.ToString() },
                { TimePlayedKey, TimePlayed.ToString() }//,
                //{ Item1Key, Item1Amount.ToString("F2") },
                //{ Item2Key, Item2Amount.ToString() },
                //{ Item3Key, Item3Amount.ToString() },
                //{ Item4Key, Item4Amount.ToString() }
            };
        }
    }
}
