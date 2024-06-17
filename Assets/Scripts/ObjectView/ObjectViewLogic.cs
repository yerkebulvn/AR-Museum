using Firebase.Database;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using Firebase.Extensions;
using UnityEngine.SceneManagement;

public class ObjectViewLogic : MonoBehaviour
{
    public List<GameObject> prefabs;
    private GameObject exhibit;
    public GameObject ARCamera;
    public GameObject infoCanvas;
    public GameObject MainCanvas;
    public TextMeshProUGUI textMeshPro;

    private DatabaseReference databaseReference;
    // Start is called before the first frame update
    void Start()
    {
        databaseReference = FirebaseDatabase.DefaultInstance.RootReference;
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void spawnObject(string objectName)
    {
        foreach (GameObject prefab in prefabs)
        {
            if (prefab.name == objectName)
            {
                infoCanvas.SetActive(true);
                RetrieveData(objectName);
                exhibit = Instantiate(prefab, ARCamera.transform.position + new Vector3(0, 0, 1.5f), ARCamera.transform.rotation); break;
            }
        }
    }

    public void backButtonPressed()
    {
        Destroy(exhibit);
        MainCanvas.SetActive(true);
        infoCanvas.SetActive(false);
    }
    /*
    private string RetrieveData(string ARTEFACT)
    {
        string result = "Default Text";

        FirebaseDatabase.DefaultInstance
      .GetReference("Artefacts").Child(ARTEFACT).Child("text")
      .GetValueAsync().ContinueWithOnMainThread(task =>
      {
          if (task.IsFaulted)
          {
              // Handle the error...
              Debug.LogError("Error retrieving data: " + task.Exception);
              result = task.Exception.ToString();
              return;
          }
          else if (task.IsCompleted)
          {
              DataSnapshot snapshot = task.Result;
              // Do something with snapshot...
              result = snapshot.Value.ToString(); // Get the message data

          }
      });

        return result;
    }*/

    private void RetrieveData(string ARTEFACT)
    {
        FirebaseDatabase.DefaultInstance
          .GetReference("Artefacts").Child(ARTEFACT).Child("text")
          .GetValueAsync().ContinueWithOnMainThread(task =>
          {
              if (task.IsFaulted)
              {
                  // Handle the error...
                  Debug.LogError("Error retrieving data: " + task.Exception);
              }
              else if (task.IsCompleted)
              {
                  DataSnapshot snapshot = task.Result;
                  // Update UI here with snapshot data
                  textMeshPro.text = snapshot.Value.ToString();
              }
          });
    }

    public void goToMainScene()
    {
        SceneManager.LoadScene("SampleScene");
    }
}
