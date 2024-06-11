using Firebase.Database;
using Firebase;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class GetTextFromDatabase : MonoBehaviour
{
    public TextMeshProUGUI textMeshPro; // Reference to your TextMeshPro component
    public string ARTEFACT;

    private DatabaseReference databaseReference;

    private void Start()
    {
        // Initialize Firebase
        FirebaseApp.CheckAndFixDependenciesAsync().ContinueWith(task =>
        {
            FirebaseApp app = FirebaseApp.DefaultInstance;
            databaseReference = FirebaseDatabase.DefaultInstance.RootReference;
        });

        // Retrieve data
        RetrieveData();
    }

    private void RetrieveData()
    {
        // Replace "messages" with your specific node in the database
        databaseReference.Child("Artefacts").Child(ARTEFACT).Child("text").GetValueAsync().ContinueWith(task =>
        {
            if (task.IsFaulted)
            {
                Debug.LogError("Error retrieving data: " + task.Exception);
                return;
            }

            if (task.IsCompleted)
            {
                DataSnapshot snapshot = task.Result;
                string message = snapshot.Value.ToString(); // Get the message data

                // Update your TextMeshPro component
                textMeshPro.text = message;
            }
        });
    }
}
