using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Firebase;
using Firebase.Auth;
using TMPro;
using UnityEngine.UI;

public class UserAuth : MonoBehaviour
{
    #region variables
    [Header("Login")]
    public TMP_InputField email;
    public TMP_InputField password;
    public TMP_InputField password_confirm;
    public Button buttonSignUp;
    public Button buttonLogin;
    public TMP_Text welcomeMSG;
    public Button buttonBack;
    #endregion

    private FirebaseAuth auth;

    // Start is called before the first frame update
    void Start()
    {
        auth = Firebase.Auth.FirebaseAuth.DefaultInstance;
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void SignUp()
    {
        //First page check
        if (password_confirm.IsActive() == false)
        {
            Debug.Log("SugnUp Page Open");
            email.gameObject.SetActive(true);
            password_confirm.gameObject.SetActive(true);
            password.gameObject.SetActive(true);
            buttonLogin.gameObject.SetActive(false);
            buttonBack.gameObject.SetActive(true);
            return;
        }

        if(email.text == "")
        {
            welcomeMSG.text = "Эл. поштаны теріңіз";
            return;
        }

        if (password.text == "")
        {
            welcomeMSG.text = "Құпия сөзді теріңіз";
            return;
        }

        if (password_confirm.text == "")
        {
            welcomeMSG.text = "Құпия сөзді қайта теріңіз";
            return;
        }

        if (password.text.Equals(password_confirm.text))
        {
            auth.CreateUserWithEmailAndPasswordAsync(email.text, password.text).ContinueWith(task => {
                if (task.IsCanceled)
                {
                    Debug.LogError("CreateUserWithEmailAndPasswordAsync was canceled.");
                    return;
                }
                if (task.IsFaulted)
                {
                    Debug.LogError("CreateUserWithEmailAndPasswordAsync encountered an error: " + task.Exception);
                    welcomeMSG.text = task.Exception.Message;
                    return;
                }

                // Firebase user has been created.
                Firebase.Auth.AuthResult result = task.Result;
                Debug.LogFormat("Firebase user created successfully: {0} ({1})",
                    result.User.DisplayName, result.User.UserId);
                welcomeMSG.text = "Қосымшаға тіркелуіңіз сәтті өтті, " + result.User.Email.ToString();

                //Sign Up Log to Analytics
                Firebase.Analytics.FirebaseAnalytics.LogEvent(
                    Firebase.Analytics.FirebaseAnalytics.EventSignUp,
                        new Firebase.Analytics.Parameter[] {
                        new Firebase.Analytics.Parameter(
                            Firebase.Analytics.FirebaseAnalytics.ParameterMethod, task.Id),
                    }
                );
            });
        }
    }

    public void LoginButtonClick()
    {
        //First page check
        if (password.IsActive() == false)
        {
            Debug.Log("Login Page Open");
            email.gameObject.SetActive(true);
            
            password.gameObject.SetActive(true);
            buttonSignUp.gameObject.SetActive(false);
            buttonBack.gameObject.SetActive(true);
            return;
        }

        if (email.text == "")
        {
            welcomeMSG.text = "Эл. поштаны теріңіз";
            return;
        }

        if (password.text == "")
        {
            welcomeMSG.text = "Құпия сөзді теріңіз";
            return;
        }

        auth.SignInWithEmailAndPasswordAsync(email.text, password.text).ContinueWith(task => {
            if (task.IsCanceled)
            {
                Debug.LogError("SignInWithEmailAndPasswordAsync was canceled.");
                return;
            }
            if (task.IsFaulted)
            {
                Debug.LogError("SignInWithEmailAndPasswordAsync encountered an error: " + task.Exception);
                welcomeMSG.text = task.Exception.Message;
                return;
            }

            Firebase.Auth.AuthResult result = task.Result;
            Debug.LogFormat("User signed in successfully: {0} ({1})",
                result.User.DisplayName, result.User.UserId);
            welcomeMSG.text = "Қош келдіңіз, " + result.User.Email.ToString();

            //Log Event Login
            Firebase.Analytics.FirebaseAnalytics.LogEvent(
                Firebase.Analytics.FirebaseAnalytics.EventLogin,
                    new Firebase.Analytics.Parameter[] {
                    new Firebase.Analytics.Parameter(
                        Firebase.Analytics.FirebaseAnalytics.ParameterMethod, task.Id),
                }
            );
        });
    }

    public void backButtonClick()
    {
        if (password_confirm.IsActive() == false) 
        {
            email.gameObject.SetActive(false);
            welcomeMSG.text = "";
            password.gameObject.SetActive(false);
            buttonSignUp.gameObject.SetActive(true);
            buttonBack.gameObject.SetActive(false);
            Debug.Log("Back to Main from Login");
        }

        else
        {
            welcomeMSG.text = "";
            email.gameObject.SetActive(false);
            password.gameObject.SetActive(false);
            buttonLogin.gameObject.SetActive(true);
            buttonBack.gameObject.SetActive(false);
            password_confirm.gameObject.SetActive(false);
            Debug.Log("Back to Main from SignUP");
        }
    }
}
