using System.Collections.Generic;

namespace CustomPlayFabAPI.Data
{
    public interface IUserData
    {
        public void ParseDataFromDictionary(Dictionary<string, string> userData);

        public void ParseDataToDictionary();
    }
}
