using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Firebase;
using Firebase.Auth;
using TMPro;

public class UserAuth : MonoBehaviour
{
    #region variables
    [Header("Login")]
    public TMP_InputField email;
    public TMP_InputField password;
    public TMP_InputField password_confirm;
    #endregion

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
