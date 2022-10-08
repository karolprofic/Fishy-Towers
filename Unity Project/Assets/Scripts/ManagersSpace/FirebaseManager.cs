using System;
using System.Collections;
using System.Linq;
using Firebase.Database;
using UnityEngine.SceneManagement;
using UnityEngine;
using UnityEngine.Localization.Settings;

namespace ManagersSpace
{
	public class FirebaseManager : Manager
	{
        //public/inspector
        [SerializeField] public bool updateUser;
        [SerializeField] public string status;
        [SerializeField] private string email;
		[SerializeField] private string uid;

        //unity methods
        private void Start()
		{
            updateUser = false;
            UpdateUserInfo();
		}

        private void FixedUpdate()
        {
            //Todo: Czy nie można tego umieścić w Awake() lub Start()? 
            //Todo: wyciągnąć stringi jako const na początku klasy
            //Todo: można użyć SceneManager.GetActiveScene().name is "Settings" or "TestScene"
            if (SceneManager.GetActiveScene().name == "Settings" || SceneManager.GetActiveScene().name == "TestScene")
            {
                if (Managers.Firebase.updateUser)
                {
                    StartCoroutine(Managers.Firebase.UpdateUserAfterOneSecond());
                }
            }
        }
        
        //public methods
        public IEnumerator UpdateUserAfterOneSecond()
        {
            updateUser = false;
            yield return new WaitForSeconds(1);
            UpdateUserInfo();
        }

        public void StartSynchronization()
        {
            if (IsLogIn() == false)
            {
                return;
            }

            StartCoroutine(GetFirebaseProfile((profile) => {
                FirebaseProfile localeProfile = CreateFirebaseProfile();
                FirebaseProfile databaseProfile = profile;
                SynchronizeData(databaseProfile, localeProfile);
            }));

        }
        
        public void UpdateUserInfo()
        {
            Firebase.Auth.FirebaseAuth auth = Firebase.Auth.FirebaseAuth.DefaultInstance;
            Firebase.Auth.FirebaseUser user = auth.CurrentUser;

            if (user != null)
            {
                status = LocalizationSettings.StringDatabase.GetLocalizedString("Settings", "STATUS_LOGGED");
                email = user.Email;
                uid = user.UserId;
            }
            else
            {
                status = LocalizationSettings.StringDatabase.GetLocalizedString("Settings", "STATUS_NOT_LOGGED");
                email = "";
                uid = "";
            }
        }

        public void LogInOrSignIn(string userEmail, string userPassword)
        {
            Firebase.Auth.FirebaseAuth auth = Firebase.Auth.FirebaseAuth.DefaultInstance;
            auth.FetchProvidersForEmailAsync(userEmail).ContinueWith((authTask) => {
                if (authTask.IsFaulted || authTask.IsCanceled)
                {
                    Debug.Log("An error occurred:");
                    if (authTask.Exception != null)
                        Debug.Log(authTask.Exception.ToString());
                }
                else if (authTask.IsCompleted)
                {
                    if (authTask.Result.Contains("password"))
                    {
                        LogInViaEmailAndPassword(userEmail, userPassword);
                    }
                    else
                    {
                        CreateAccountViaEmailAndPassword(userEmail, userPassword);
                    }

                }
            });
        }

        public void LogInViaEmailAndPassword(string userEmail, string userPassword)
        {
            Firebase.Auth.FirebaseAuth auth = Firebase.Auth.FirebaseAuth.DefaultInstance;
            auth.SignInWithEmailAndPasswordAsync(userEmail, userPassword).ContinueWith(task =>
            {
                if (task.IsFaulted || task.IsCanceled)
                {
                    status = LocalizationSettings.StringDatabase.GetLocalizedString("Settings", "STATUS_ERROR");
                    Debug.Log("An error occurred during login:");
                    if (task.Exception != null)
                        Debug.Log(task.Exception.ToString());
                    return;
                }
                Firebase.Auth.FirebaseUser newUser = task.Result;
                Debug.LogFormat("User signed in successfully: {0} ({1})", newUser.Email, newUser.UserId);
                updateUser = true;
            });
        }
        
        public void SignOut()
        {
            Firebase.Auth.FirebaseAuth auth = Firebase.Auth.FirebaseAuth.DefaultInstance;
            auth.SignOut();
            UpdateUserInfo();
        }
        
        //private methods
        private void SynchronizeData(FirebaseProfile databaseProfile, FirebaseProfile localeProfile)
        {
            if (databaseProfile == null)
            {
                SendFirebaseProfile(localeProfile);
            }

            if (databaseProfile != null && databaseProfile.score < localeProfile.score)
            {
                SendFirebaseProfile(localeProfile);
            }

            if (databaseProfile != null && databaseProfile.score > localeProfile.score)
            {
                Managers.Achievements.achievements = databaseProfile.achievements;
                Managers.Statistics.statistics = databaseProfile.statistics;
            }

        }

        private FirebaseProfile CreateFirebaseProfile()
        {
            FirebaseProfile profile = new FirebaseProfile
            {
                uid = uid,
                email = email,
                timestamp = DateTime.Now.Ticks,
                score = Managers.Statistics.GetScore(),
                achievements = Managers.Achievements.achievements,
                statistics = Managers.Statistics.statistics
            };
            return profile;
        }

        private void SendFirebaseProfile(FirebaseProfile profile)
        {
            DatabaseReference reference = FirebaseDatabase.DefaultInstance.RootReference;
            reference.Child("users").Child(uid).SetRawJsonValueAsync(JsonUtility.ToJson(profile));
        }

        private IEnumerator GetFirebaseProfile(Action<FirebaseProfile> onCallback)
        {
            DatabaseReference reference = FirebaseDatabase.DefaultInstance.RootReference;

            var firebaseData = reference.Child("users").Child(uid).GetValueAsync();

            yield return new WaitUntil(predicate: () => firebaseData.IsCompleted);

            if (firebaseData != null)
            {
                DataSnapshot snapshot = firebaseData.Result;
                FirebaseProfile profile = JsonUtility.FromJson<FirebaseProfile>(snapshot.GetRawJsonValue());
                onCallback.Invoke(profile);
            }

        }
        
        private void CreateAccountViaEmailAndPassword(string userEmail, string userPassword)
        {
            Firebase.Auth.FirebaseAuth auth = Firebase.Auth.FirebaseAuth.DefaultInstance;
            auth.CreateUserWithEmailAndPasswordAsync(userEmail, userPassword).ContinueWith(task =>
            {
                if (task.IsFaulted || task.IsCanceled)
                {
                    status = LocalizationSettings.StringDatabase.GetLocalizedString("Settings", "STATUS_ERROR");
                    Debug.Log("An error occurred during creating account:");
                    if (task.Exception != null)
                        Debug.Log(task.Exception.ToString());
                    return;
                }
                Firebase.Auth.FirebaseUser newUser = task.Result;
                Debug.LogFormat("Firebase user created successfully: {0} ({1})", newUser.Email, newUser.UserId);
                updateUser = true;
            });
        }

        private static bool IsLogIn()
        {
            Firebase.Auth.FirebaseAuth auth = Firebase.Auth.FirebaseAuth.DefaultInstance;
            Firebase.Auth.FirebaseUser user = auth.CurrentUser;
            return user != null;
        }

    }
}