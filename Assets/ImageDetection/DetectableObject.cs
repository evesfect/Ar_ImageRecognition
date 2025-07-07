using UnityEngine;

[CreateAssetMenu(fileName = "DetectableObject", menuName = "AR Detection/Detectable Object")]
public class DetectableObject : ScriptableObject
{
    public string objectName;
    public string description;
    public Texture2D referenceImage;
    public Vector2 physicalSize = new Vector2(0.1f, 0.1f);
}