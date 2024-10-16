using Firebase;
using Firebase.Analytics;

using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.XR.ARFoundation;
using UnityEngine.XR.ARSubsystems;
using Firebase.Database;
using Firebase.Auth;
using Firebase.Extensions;

public class ScanController : MonoBehaviour
{

    public GameObject[] ArPrefabs;
    [SerializeField] private ARTrackedObjectManager trackedObjectManager;
    private bool isScanning = false;

    List<GameObject> ARObjects = new List<GameObject>();

    public Button scanButton;
    public TMP_Text Infobox;

    private void Start()
    {
        // Настройте кнопку в вашем интерфейсе пользователя (UI).
        // Привяжите этот метод к событию нажатия кнопки.
        // Например, если у вас есть кнопка "Start/Stop Scan", привяжите этот метод к ее событию OnClick.

        // Получите ссылку на компонент Button.
        //scanButton = GetComponent<Button>();

        // Настройте начальный текст кнопки.
        scanButton.GetComponentInChildren<TMP_Text>().text = "SCAN";
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
            trackedObjectManager.enabled = false;
            Debug.LogWarning("*** OBJECTS SCAN STOPPED ***");
            scanButton.GetComponentInChildren<TMP_Text>().text = "SCAN";
        }
        else
        {
            // Возобновите сканирование объектов.
            trackedObjectManager.enabled = true;
            Debug.Log("*** OBJECTS SCAN STARTED ***");
            scanButton.GetComponentInChildren<TMP_Text>().text = "STOP";
        }

        isScanning = !isScanning;
    }

    private void OnEnable()
    {
        // Подпишитесь на событие обнаружения объекта.
        if(trackedObjectManager.enabled == true)
            trackedObjectManager.trackedObjectsChanged += OnTrackedObjectsChanged;
    }

    private void OnDisable()
    {
        // Отпишитесь от события обнаружения объекта.
        if (trackedObjectManager.enabled == true)
            trackedObjectManager.trackedObjectsChanged -= OnTrackedObjectsChanged;
    }

    private void Update()
    {
        /*if (trackedObjectManager.enabled == true) { 
            OutputTracking(); 
        }*/
    }
    void OutputTracking()
    {
        Infobox.text = "Tracking Objects Data: \n";

        int i = 0;

        foreach (var trackedObject in trackedObjectManager.trackables)
        {
            //Infobox.text += "Object: " + trackedObject.referenceObject.name + " "
              //  + trackedObject.trackingState.ToString() + " "
               // + " \n";
            if(trackedObject.trackingState == TrackingState.Limited)
            {
                ARObjects[i].SetActive(false);
            }
            if (trackedObject.trackingState == TrackingState.Tracking)
            {
                ARObjects[i].SetActive(true);
            }
            i++;
        }
    }
    private void OnTrackedObjectsChanged(ARTrackedObjectsChangedEventArgs eventArgs)
    {

        //Create object based on object tracked
        foreach (var trackedObject in eventArgs.added)
        {
            foreach (var arPrefab in ArPrefabs)
            {
                if (trackedObject.referenceObject.name == arPrefab.name)
                {
                    var newPrefab = Instantiate(arPrefab, trackedObject.transform);
                    ARObjects.Add(newPrefab);
                    FirebaseAnalytics.LogEvent("ObjectDetected", "ObjectName", trackedObject.referenceObject.name.ToString());
                    FirebaseAnalytics.LogEvent(trackedObject.referenceObject.name.ToString());
                }
            }
        }

        //Update tracking position
        foreach (var trackedObject in eventArgs.updated)
        {
            foreach (var gameObject in ARObjects)
            {
                if (gameObject.name == trackedObject.name)
                {
                    gameObject.SetActive(trackedObject.trackingState == TrackingState.Tracking);
                }
            }
        }
        // Если обнаружен хотя бы один объект, остановите сканирование.
        /*if (eventArgs.added.Count > 0)
        {
            trackedObjectManager.enabled = false;
            Debug.Log("***Object detected. Scan stopped***");

            scanButton.GetComponentInChildren<TMP_Text>().text = "SCAN";

            // Отправьте аналитическое событие в Firebase.
            FirebaseAnalytics.LogEvent("ObjectDetected", "ObjectName", eventArgs.added[0].referenceObject.name);
        }*/
    }
}
