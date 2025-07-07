using UnityEngine;

public class ObjectInfoDisplay : MonoBehaviour
{
    public void Setup(DetectableObject detectedObject)
    {
        // Just print to console for now
        Debug.Log($"Detected: {detectedObject.objectName}");

        // Maybe change cube color based on object type
        GetComponent<Renderer>().material.color = Color.green;
    }
}