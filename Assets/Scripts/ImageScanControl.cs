using Firebase;

using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.XR.ARFoundation;

using Firebase.Analytics;
using UnityEngine.XR.ARSubsystems;


public class ImageScanControl : MonoBehaviour
{

    public GameObject[] arPrefabs;
    [SerializeField] private ARTrackedImageManager imageManager;
    List<GameObject> aRObjects = new List<GameObject>();

    private bool isScanning = false;
    public Button scanButton;
    public TMP_Text debugText;

    // Start is called before the first frame update
    void Start()
    {
        scanButton.GetComponentInChildren<TMP_Text>().text = "START SCAN IMAGE";
    }

    void Awake()
    {
        FirebaseApp.CheckAndFixDependenciesAsync().ContinueWith(task =>
        {
            FirebaseApp app = FirebaseApp.DefaultInstance;
            FirebaseAnalytics.SetAnalyticsCollectionEnabled(true);
        });

    }

    public void ToggleScan()
    {
        if (isScanning)
        {
            // Остановите сканирование объектов.
            imageManager.enabled = false;
            Debug.Log("*** IMAGE SCAN STOPPED ***.");
            scanButton.GetComponentInChildren<TMP_Text>().text = "SCAN IMAGE";
        }
        else
        {
            // Возобновите сканирование объектов.
            imageManager.enabled = true;
            Debug.Log("*** IMAGE SCAN STARTED ***");
            scanButton.GetComponentInChildren<TMP_Text>().text = "STOP SCAN IMAGE";
        }

        isScanning = !isScanning;
    }

    private void OnEnable()
    {
        // Подпишитесь на событие обнаружения объекта.
        imageManager.trackedImagesChanged += OnTrackedImageChanged;
    }

    private void OnDisable()
    {
        // Отпишитесь от события обнаружения объекта.
        imageManager.trackedImagesChanged -= OnTrackedImageChanged;
    }

    // Update is called once per frame
    void Update()
    {
        OutputTracking();
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
            if (trackedImage.trackingState == TrackingState.Limited)
            {
                aRObjects[i].SetActive(false);
            }
            if (trackedImage.trackingState == TrackingState.Tracking)
            {
                aRObjects[i].SetActive(true);
            }
            i++;
        }
    }

    private void OnTrackedImageChanged(ARTrackedImagesChangedEventArgs eventArgs)
    {

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
                    FirebaseAnalytics.LogEvent("ObjectDetected", "ObjectName", trackedImage.referenceImage.name);
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
}
