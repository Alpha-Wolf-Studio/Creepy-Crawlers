using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using PlayFab;
using PlayFab.ClientModels;
using CustomPlayFabAPI.Data;

namespace CustomPlayFabAPI
{
    public class CustomPlayFabSingleton : MonoBehaviour
    {
        public event Action<bool> OnLoginResult;
        public event Action OnRequestDisplayName;
        public event Action<bool> OnClientDataSetResult;
        public event Action<bool> OnClientDataGetResult;

        public event Action OnInitializeComplete; 

        public bool LoggedIn { get; private set; }
        public CustomUserData UserData { get; private set; } = new CustomUserData();

        private const string LoginIDKey = "LOGIN_ID_KEY";
        private Dictionary<string, string> _clientData;
        
        private float _playTime;
        private string _displayName;
        
        private bool _newUser;
        private string _guid;
        
        public static CustomPlayFabSingleton Instance { get; private set; }

        private void Awake()
        {
            Initialize();
        }

        private void Start()
        {
            StartCoroutine(InitializeCoroutine());
        }

        private void Update()
        {
            _playTime += Time.deltaTime;
        }

        private void OnDestroy()
        {
            if (UserData != null)
            {
                int time = (int)_playTime;
                UserData.AddTimePlayed(time);
                
                SaveDataToPlayFabOnClose();
            }
        }

        private void Initialize()
        {
            if (Instance == null)
            {
                Instance = this;
                DontDestroyOnLoad(gameObject);
            }
            else
            {
                Destroy(gameObject);
            }
        }

        private IEnumerator InitializeCoroutine()
        {
            yield return null;
            yield return StartCoroutine(LoginToPlayFabDirectly());
            yield return StartCoroutine(GetClientDataFromPlayFab());
            
            UpdateUserData();
            UserData.AddToLogInCounter();

            _playTime = Time.time;
            
            OnInitializeComplete?.Invoke();
        }

        #region LOGIN

        private IEnumerator LoginToPlayFabDirectly()
        {
            bool? success = null;
            
            _newUser = !PlayerPrefs.HasKey(LoginIDKey);
            _guid = _newUser ? Guid.NewGuid().ToString() : PlayerPrefs.GetString(LoginIDKey);

            if (_newUser)
            {
                OnRequestDisplayName?.Invoke();
                yield return StartCoroutine(DisplayNameInitialize());
            }

            PlayFabClientAPI.LoginWithCustomID(new LoginWithCustomIDRequest()
                {
                    CustomId = _guid,
                    CreateAccount = true
                },
                resultCallback: result =>
                {
                    Debug.Log("Client logged in to PlayFab.");
                    success = true;
                },
                errorCallback: error =>
                {
                    Debug.Log("Error logging in client: " + error.GenerateErrorReport());
                    OnLoginResult?.Invoke(false);
                    success = false;
                });
            yield return new WaitUntil(() => success.HasValue);
            
            if(success.HasValue)
                LoggedIn = success.Value;
            
            OnLoginResult?.Invoke(_newUser);

            if (!_newUser)
            {
                UpdateDisplayName();
            }
            else
            {
                SaveDisplayName();
            }
        }

        #endregion

        #region DISPLAY_NAME

        private IEnumerator DisplayNameInitialize()
        {
            yield return new WaitUntil(() => !string.IsNullOrEmpty(_displayName));
            PlayerPrefs.SetString(LoginIDKey, _guid);
            PlayerPrefs.Save();
        }

        public void SetDisplayName(string displayName) => _displayName = displayName;
        
        private void SaveDisplayName()
        {
            if (LoggedIn)
            {
                PlayFabClientAPI.UpdateUserTitleDisplayName(new UpdateUserTitleDisplayNameRequest 
                    {
                        DisplayName = _displayName 
                    }, 
                    resultCallback: result => 
                    {
                        Debug.Log("The player's display name is now: " + result.DisplayName); 
                    }, 
                    errorCallback: error => 
                    {
                        Debug.LogError(error.GenerateErrorReport());
                    });
            }
        }

        private void UpdateDisplayName()
        {
            PlayFabClientAPI.GetPlayerProfile(new GetPlayerProfileRequest() 
                {
                    
                }, 
                resultCallback: result =>
                {
                    _displayName = result.PlayerProfile.DisplayName;
                }, 
                errorCallback: error => 
                {
                    Debug.LogError(error.GenerateErrorReport());
                });
        }

        #endregion

        #region EXTERNAL_SAVE_DATA

        public void SaveUserData()
        {
            UserData.ParseDataToDictionary();
            _clientData = UserData.FullDataDictionary;
            SaveClientData(_clientData);
        }

        #endregion
        
        #region EXTERNAL_LOAD_DATA

        public void UpdateUserData()
        {
            UserData.ParseDataFromDictionary(_clientData);
        }

        #endregion
        
        #region INTERNAL_SAVE_DATA
        
        private void SaveClientData(Dictionary<string, string> dataToSave)
        {
            if (LoggedIn)
            {
                StartCoroutine(SaveClientDataToPlayFab(dataToSave));
            }
            else
            {
                Debug.LogWarning("Invalid method before logging in to PlayFab. Wait until the LoggedIn" +
                                 " Variable is set to true or subscribe to the LoginResult event before calling " +
                                 "any method on the CustomPlayFabSingleton.");
            }
        }
        
        private IEnumerator SaveClientDataToPlayFab (Dictionary<string, string> dataToUpdate, List<string> keysToRemove = null)
        {
            bool? success = null;
            PlayFabClientAPI.UpdateUserData(new UpdateUserDataRequest
                {
                    Data = dataToUpdate,
                    KeysToRemove = keysToRemove,
                    Permission = UserDataPermission.Public
                },
                resultCallback: result =>
                {
                    Debug.Log("Client user data updated successfully.");
                    success = true;
                },
                errorCallback: error =>
                {
                    Debug.Log("Error updating client user data: " + error.GenerateErrorReport());
                    success = false;
                });
            yield return new WaitUntil(() => success.HasValue);
            
            if(success.HasValue)
                OnClientDataSetResult?.Invoke(success.Value);
        }

        private void SaveDataToPlayFabOnClose()
        {
            UserData.ParseDataToDictionary();
            _clientData = UserData.FullDataDictionary;
            PlayFabClientAPI.UpdateUserData(new UpdateUserDataRequest { Data = _clientData }, null, null);
        }
        
        #endregion

        #region INTERNAL_LOAD_DATA

        private void RequestClientData()
        {
            if (LoggedIn)
            {
                StartCoroutine(GetClientDataFromPlayFab());
            }
            else
            {
                Debug.LogWarning("Invalid method before logging in to PlayFab. Wait until the LoggedIn" +
                                 " Variable is set to true or subscribe to the LoginResult event before calling " +
                                 "any method on the CustomPlayFabSingleton.");
            }
        }

        private IEnumerator GetClientDataFromPlayFab (string userId = null)
        {
            bool? success = null;

            PlayFabClientAPI.GetUserData(new GetUserDataRequest()
                {
                    PlayFabId = (!string.IsNullOrEmpty(userId)) ? userId : PlayFabSettings.staticPlayer.PlayFabId
                },
                resultCallback: result =>
                {
                    _clientData = new Dictionary<string, string>(result.Data.Count);
				
                    foreach (var keyValuePair in result.Data)
                        _clientData.Add(keyValuePair.Key, keyValuePair.Value.Value);
                    
                    UserData.ParseDataFromDictionary(_clientData);
				
                    Debug.Log("Client data received successfully.");
                    success = true;
                },
                errorCallback: error =>
                {
                    Debug.Log("Error getting client user data: " + error.GenerateErrorReport());
                    success = false;
                });
			
            yield return new WaitUntil(() => success.HasValue);
            
            if(success.HasValue)
                OnClientDataGetResult?.Invoke(success.Value);
        }
        
        #endregion
    }
}
