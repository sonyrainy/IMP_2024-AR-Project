using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.XR.ARFoundation;
using UnityEngine.XR.ARSubsystems;

[System.Serializable]
public struct PlaceablePrefab
{
    public string name;
    public GameObject prefab;
}

public class ARImageInteraction : MonoBehaviour
{
    public Camera mainCamera;
    public ARTrackedImageManager trackedImageManager;
    public List<PlaceablePrefab> prefabs;

    private Dictionary<string, GameObject> spawnedObjects = new Dictionary<string, GameObject>();
    public HashSet<GameObject> activeObjects = new HashSet<GameObject>();

    void Awake()
    {
        trackedImageManager.trackedImagesChanged += OnTrackedImagesChanged;
    }

    void OnDestroy()
    {
        trackedImageManager.trackedImagesChanged -= OnTrackedImagesChanged;
    }

    void OnTrackedImagesChanged(ARTrackedImagesChangedEventArgs eventArgs)
    {
        foreach (var trackedImage in eventArgs.added)
        {
            HandleNewTrackedImage(trackedImage);
        }

        foreach (var trackedImage in eventArgs.updated)
        {
            UpdateTrackedImage(trackedImage);
        }

        foreach (var trackedImage in eventArgs.removed)
        {
            RemoveTrackedImage(trackedImage);
        }

        AdjustObjectBehavior();
    }

    private void HandleNewTrackedImage(ARTrackedImage trackedImage)
    {
        var prefab = prefabs.Find(p => p.name == trackedImage.referenceImage.name).prefab;
        if (prefab != null && !spawnedObjects.ContainsKey(trackedImage.referenceImage.name))
        {
            var instance = Instantiate(prefab, trackedImage.transform.position, trackedImage.transform.rotation);
            spawnedObjects[trackedImage.referenceImage.name] = instance;
            activeObjects.Add(instance);
        }
    }

    private void UpdateTrackedImage(ARTrackedImage trackedImage)
    {
        if (trackedImage == null || trackedImage.gameObject == null)
        {
            Debug.Log("Tracked image or its GameObject is destroyed.");
            return;
        }

        if (spawnedObjects.ContainsKey(trackedImage.referenceImage.name))
        {
            var instance = spawnedObjects[trackedImage.referenceImage.name];
            instance.transform.position = trackedImage.transform.position;
            instance.transform.rotation = trackedImage.transform.rotation;
            instance.SetActive(trackedImage.trackingState == TrackingState.Tracking);
            if (trackedImage.trackingState == TrackingState.Tracking)
            {
                activeObjects.Add(instance);
            }
            else
            {
                activeObjects.Remove(instance);
                DisablePulsator(instance);
            }
        }
    }

    private void RemoveTrackedImage(ARTrackedImage trackedImage)
    {
        if (spawnedObjects.ContainsKey(trackedImage.referenceImage.name))
        {
            var instance = spawnedObjects[trackedImage.referenceImage.name];
            activeObjects.Remove(instance);
            Destroy(instance);
            spawnedObjects.Remove(trackedImage.referenceImage.name);
        }
    }

    private void AdjustObjectBehavior()
    {
        foreach (var obj in activeObjects) // activeObjects 집합에 있는 모든 오브젝트에 대해 반복
        {
            EnablePulsator(obj); // Pulsator 컴포넌트 활성화
        }
    }

    public void EnablePulsator(GameObject obj)
    {
        var pulsator = obj.GetComponent<Pulsator>();
        if (pulsator == null)
        {
            pulsator = obj.AddComponent<Pulsator>();
        }
        pulsator.enabled = true;
    }

    private void DisablePulsator(GameObject obj)
    {
        var pulsator = obj.GetComponent<Pulsator>();
        if (pulsator != null)
        {
            pulsator.enabled = false;
        }
    }
}
