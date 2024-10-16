using Firebase.Database;
using Firebase;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using Firebase.Extensions;

public class GetTextFromDatabase : MonoBehaviour
{
    public TextMeshProUGUI textMeshPro; // Reference to your TextMeshPro component
    public string ARTEFACT;

    private DatabaseReference databaseReference;

    private void Start()
    {
        // Initialize Firebase
        /*FirebaseApp.CheckAndFixDependenciesAsync().ContinueWith(task =>
        {
            FirebaseApp app = FirebaseApp.DefaultInstance;
            databaseReference = FirebaseDatabase.DefaultInstance.RootReference;
        });*/

        databaseReference = FirebaseDatabase.DefaultInstance.RootReference;

        // Retrieve data
        RetrieveData();
    }

    private void RetrieveData()
    {

        FirebaseDatabase.DefaultInstance
      .GetReference("Artefacts").Child(ARTEFACT).Child("text")
      .GetValueAsync().ContinueWithOnMainThread(task => {
          if (task.IsFaulted)
          {
              // Handle the error...
              Debug.LogError("Error retrieving data: " + task.Exception);
              return;
          }
          else if (task.IsCompleted)
          {
              DataSnapshot snapshot = task.Result;
              // Do something with snapshot...
              string message = snapshot.Value.ToString(); // Get the message data

              // Update your TextMeshPro component
              textMeshPro.text = message;
          }
      });
    }
}
