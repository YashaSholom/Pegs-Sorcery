using UnityEngine;

public class CameraScale : MonoBehaviour
{
    // How many world units you want visible from left to right
    public float targetWidth = 20.0f;
    public Camera cam;

    void Update()
    {
        float aspectRatio = (float)Screen.width / Screen.height;
        // Set ortho size based on width instead of height
        cam.orthographicSize = targetWidth / (aspectRatio * 2f);
    }
}