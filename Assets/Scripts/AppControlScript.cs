using Firebase;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Firebase.Analytics;

public class AppControlScript : MonoBehaviour
{
    private void Awake()
    {
        FirebaseApp.CheckAndFixDependenciesAsync().ContinueWith(task =>
        {
            FirebaseApp app = FirebaseApp.DefaultInstance;
            FirebaseAnalytics.SetAnalyticsCollectionEnabled(true);
        });
    }
}
