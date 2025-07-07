using UnityEngine;
using UnityEngine.XR.ARFoundation;
using UnityEngine.XR.ARSubsystems;
using System.Collections.Generic;

public class ARImageDetector : MonoBehaviour
{
    [Header("AR Setup")]
    public ARTrackedImageManager imageManager;
    public GameObject infoPrefab;

    [Header("Objects to Detect")]
    public List<DetectableObject> detectableObjects;

    private Dictionary<string, DetectableObject> objectLookup = new Dictionary<string, DetectableObject>();
    private Dictionary<TrackableId, GameObject> spawnedObjects = new Dictionary<TrackableId, GameObject>();

    void Start()
    {
        // Build lookup table
        foreach (var obj in detectableObjects)
        {
            if (obj.referenceImage != null)
            {
                objectLookup[obj.referenceImage.name] = obj;
            }
        }
    }

    void OnEnable()
    {
        imageManager.trackedImagesChanged += OnImagesChanged;
    }

    void OnDisable()
    {
        imageManager.trackedImagesChanged -= OnImagesChanged;
    }

    void OnImagesChanged(ARTrackedImagesChangedEventArgs eventArgs)
    {
        // New detections
        foreach (var trackedImage in eventArgs.added)
        {
            CreateInfoDisplay(trackedImage);
        }

        // Updates
        foreach (var trackedImage in eventArgs.updated)
        {
            UpdateInfoDisplay(trackedImage);
        }

        // Lost tracking
        foreach (var trackedImage in eventArgs.removed)
        {
            RemoveInfoDisplay(trackedImage);
        }
    }

    void CreateInfoDisplay(ARTrackedImage trackedImage)
    {
        string imageName = trackedImage.referenceImage.name;

        if (objectLookup.TryGetValue(imageName, out DetectableObject detectedObj))
        {
            GameObject info = Instantiate(infoPrefab, trackedImage.transform);
            var display = info.GetComponent<ObjectInfoDisplay>();
            if (display != null)
            {
                display.Setup(detectedObj);
            }
            spawnedObjects[trackedImage.trackableId] = info;
        }
    }

    void UpdateInfoDisplay(ARTrackedImage trackedImage)
    {
        if (spawnedObjects.TryGetValue(trackedImage.trackableId, out GameObject info))
        {
            info.SetActive(trackedImage.trackingState == TrackingState.Tracking);
        }
    }

    void RemoveInfoDisplay(ARTrackedImage trackedImage)
    {
        if (spawnedObjects.TryGetValue(trackedImage.trackableId, out GameObject info))
        {
            Destroy(info);
            spawnedObjects.Remove(trackedImage.trackableId);
        }
    }
}