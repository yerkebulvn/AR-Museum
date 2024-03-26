using Firebase;
using Firebase.Analytics;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.XR.ARFoundation;
using UnityEngine.XR.ARSubsystems;

public class ScanController : MonoBehaviour
{
    public GameObject[] ArPrefabs;
    [SerializeField] private ARTrackedObjectManager trackedObjectManager;
    private bool isScanning = false;

    List<GameObject> ARObjects = new List<GameObject>();

    public Button scanButton;

    private void Start()
    {
        // Настройте кнопку в вашем интерфейсе пользователя (UI).
        // Привяжите этот метод к событию нажатия кнопки.
        // Например, если у вас есть кнопка "Start/Stop Scan", привяжите этот метод к ее событию OnClick.

        // Получите ссылку на компонент Button.
        //scanButton = GetComponent<Button>();

        // Настройте начальный текст кнопки.
        scanButton.GetComponentInChildren<TMP_Text>().text = "START SCAN";
    }

    void Awake()
    {
        FirebaseApp.CheckAndFixDependenciesAsync().ContinueWith(task =>
        {
            FirebaseApp app = FirebaseApp.DefaultInstance;
            FirebaseAnalytics.SetAnalyticsCollectionEnabled(true);
        });

        //trackedObjectManager = GetComponent<ARTrackedObjectManager>();
    }
    public void ToggleScan()
    {
        if (isScanning)
        {
            // Остановите сканирование объектов.
            trackedObjectManager.enabled = false;
            Debug.LogWarning("***SCAN STOPPED***.");
            scanButton.GetComponentInChildren<TMP_Text>().text = "SCAN";
        }
        else
        {
            // Возобновите сканирование объектов.
            trackedObjectManager.enabled = true;
            Debug.Log("***SCAN STARTED***");
            scanButton.GetComponentInChildren<TMP_Text>().text = "STOP";
        }

        isScanning = !isScanning;
    }

    private void OnEnable()
    {
        // Подпишитесь на событие обнаружения объекта.
        trackedObjectManager.trackedObjectsChanged += OnTrackedObjectsChanged;
    }

    private void OnDisable()
    {
        // Отпишитесь от события обнаружения объекта.
        trackedObjectManager.trackedObjectsChanged -= OnTrackedObjectsChanged;
    }

    private void OnTrackedObjectsChanged(ARTrackedObjectsChangedEventArgs eventArgs)
    {

        //Create object based on image tracked
        foreach (var trackedObjectManager in eventArgs.added)
        {
            foreach (var arPrefab in ArPrefabs)
            {
                if (trackedObjectManager.referenceObject.name == arPrefab.name)
                {
                    var newPrefab = Instantiate(arPrefab, trackedObjectManager.transform);
                    ARObjects.Add(newPrefab);
                }
            }
        }

        //Update tracking position
        /*foreach (var trackedObjectManager in eventArgs.updated)
        {
            foreach (var gameObject in ARObjects)
            {
                if (gameObject.name == trackedObjectManager.name)
                {
                    gameObject.SetActive(trackedObjectManager.trackingState == TrackingState.Tracking);
                }
            }
        }*/
        // Если обнаружен хотя бы один объект, остановите сканирование.
        if (eventArgs.added.Count > 0)
        {
            trackedObjectManager.enabled = false;
            Debug.Log("***Object detected. Scan stopped***");

            scanButton.GetComponentInChildren<TMP_Text>().text = "SCAN";

            // Отправьте аналитическое событие в Firebase.
            FirebaseAnalytics.LogEvent("ObjectDetected", "ObjectName", eventArgs.added[0].referenceObject.name);
        }
    }
}
