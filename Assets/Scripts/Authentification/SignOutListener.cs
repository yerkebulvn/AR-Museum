using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SignOutListener : MonoBehaviour
{
    public Button signOutBtn;
    protected Firebase.Auth.FirebaseAuth auth;

    // Start is called before the first frame update
    void Start()
    {
        auth = Firebase.Auth.FirebaseAuth.DefaultInstance;

        signOutBtn.onClick.AddListener(() => auth.SignOut());
    }
}
