using UnityEngine;

public class FaceCamera : MonoBehaviour
{
    void LateUpdate()
    {
        Camera camera = Camera.main;
        if (camera != null)
        {
            transform.LookAt(transform.position + camera.transform.rotation * Vector3.forward,
                             camera.transform.rotation * Vector3.up);
        }
    }
}