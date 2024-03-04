using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR.ARFoundation;
using UnityEngine.XR.ARSubsystems;

public class ObjectTracker : MonoBehaviour
{
    public GameObject[] ArPrefabs;
    private ARTrackedObjectManager trackedObjects;

    List<GameObject> ARObjects = new List<GameObject>();

    void Awake()
    {
        trackedObjects = GetComponent<ARTrackedObjectManager>();
    }

    void OnEnable()
    {
        trackedObjects.trackedObjectsChanged += OnTrackedObjectsChanged;
    }

    void OnDisable()
    {
        trackedObjects.trackedObjectsChanged -= OnTrackedObjectsChanged;
    }

    // Event Handler
    private void OnTrackedObjectsChanged(ARTrackedObjectsChangedEventArgs eventArgs)
    {
        //Create object based on image tracked
        foreach (var trackedObject in eventArgs.added)
        {
            foreach (var arPrefab in ArPrefabs)
            {
                if (trackedObject.referenceObject.name == arPrefab.name)
                {
                    var newPrefab = Instantiate(arPrefab, trackedObject.transform);
                    ARObjects.Add(newPrefab);
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

    }
}
