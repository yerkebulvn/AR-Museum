using Firebase;

using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.XR.ARFoundation;

using Firebase.Analytics;
using UnityEngine.XR.ARSubsystems;
using System.Collections;
using Firebase.Database;
using Firebase.Auth;
using Unity.VisualScripting;
using Firebase.Extensions;


public class ImageScanControl : MonoBehaviour
{

    public GameObject[] arPrefabs;
    /* [SerializeField] private ARTrackedImageManager imageManager; */
    [SerializeField] private ARTrackedImageManager imageManager;
    List<GameObject> aRObjects = new List<GameObject>();

    private bool isScanning = false;
    public Button scanButton;
    public TMP_Text debugText;

    //private DatabaseReference databaseReference;
    private FirebaseAuth auth;

    // Start is called before the first frame update
    void Start()
    {
        //scanButton.GetComponentInChildren<TMP_Text>().text = "START SCAN IMAGE";
    }

    void Awake()
    {

        FirebaseApp.CheckAndFixDependenciesAsync().ContinueWith(task =>
        {
            FirebaseApp app = FirebaseApp.DefaultInstance;
            FirebaseAnalytics.SetAnalyticsCollectionEnabled(true);
            //databaseReference = FirebaseDatabase.DefaultInstance.RootReference;
        });

    }

    public void ToggleScan()
    {
        if (isScanning)
        {
            // Остановите сканирование объектов.
            stopScanImage();
        }
        else
        {
            // Возобновите сканирование объектов.
            startScanImage();
            
        }

        isScanning = !isScanning;
    }

    private void OnEnable()
    {
        // Подпишитесь на событие обнаружения объекта.
        if (imageManager.enabled == true)
            imageManager.trackedImagesChanged += OnTrackedImageChanged;
    }

    private void OnDisable()
    {
        // Отпишитесь от события обнаружения объекта.
        if (imageManager.enabled == true)
            imageManager.trackedImagesChanged -= OnTrackedImageChanged;
    }

    // Update is called once per frame
    void Update()
    {
        /*if (imageManager.enabled == true)
        {
            OutputTracking();
        }*/
    }

    void OutputTracking()
    {
        debugText.text = "Tracking Image: \n";

        int i = 0;

        foreach (var trackedImage in imageManager.trackables)
        {
            debugText.text += "Image: " + trackedImage.referenceImage.name + " "
                + trackedImage.trackingState.ToString() + " "
                + " \n";
            /*if (trackedImage.trackingState == TrackingState.Limited)
            {
                aRObjects[i].SetActive(false);
            }
            if (trackedImage.trackingState == TrackingState.Tracking)
            {
                aRObjects[i].SetActive(true);
            }*/
            i++;
        }
    }

    private void OnTrackedImageChanged(ARTrackedImagesChangedEventArgs eventArgs)
    {
        debugText.text = "OnTrackedImageChanged";

        //Create object based on image tracked
        foreach (var trackedImage in eventArgs.added)
        {
            foreach (var arPrefab in arPrefabs)
            {
                if (trackedImage.referenceImage.name == arPrefab.name)
                {
                    Debug.Log("***** Detected Image: " + trackedImage.referenceImage.name);
                    var newPrefab = Instantiate(arPrefab, trackedImage.transform);
                    aRObjects.Add(newPrefab);

                    FirebaseAnalytics.LogEvent("DetectedImage", "ImageName", trackedImage.referenceImage.name.ToString());
                    FirebaseAnalytics.LogEvent(trackedImage.referenceImage.name.ToString());
                    
                }
            }
        }

        //Update tracking position
        foreach (var trackedImage in eventArgs.updated)
        {
            foreach (var gameObject in aRObjects)
            {
                if (gameObject.name == trackedImage.name)
                {
                    gameObject.SetActive(trackedImage.trackingState == TrackingState.Tracking);
                }
            }
        }

        /*if (eventArgs.added.Count > 0)
        {
            imageManager.enabled = false;
            Debug.Log("***Image detected. Scan stopped***");

            scanButton.GetComponentInChildren<TMP_Text>().text = "SCAN";

            // Отправьте аналитическое событие в Firebase.
            FirebaseAnalytics.LogEvent("ObjectDetected", "ObjectName", eventArgs.added[0].referenceImage.name);
        }*/
    }
    void stopScanImage()
    {
        imageManager.trackedImagesChanged -= OnTrackedImageChanged;
        imageManager.enabled = false;
        Debug.Log("*** IMAGE SCAN STOPPED ***");
        //scanButton.GetComponentInChildren<TMP_Text>().text = "SCAN IMAGE";
    }
    void startScanImage()
    {
        imageManager.enabled = true;
        imageManager.trackedImagesChanged += OnTrackedImageChanged;
        Debug.Log("*** IMAGE SCAN STARTED ***");
        //scanButton.GetComponentInChildren<TMP_Text>().text = "STOP SCAN IMAGE";
        //RetrieveData();
    }

}
