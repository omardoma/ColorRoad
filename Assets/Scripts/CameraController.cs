using UnityEngine;

public class CameraController : MonoBehaviour
{
    public Camera cam1;
    public Camera cam2;

    private void Start()
    {
        cam1.enabled = true;
        cam2.enabled = false;
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.C))
        {
            SwitchCamera();
        }
    }

    public void SwitchCamera()
    {
        cam1.enabled = !cam1.enabled;
        cam2.enabled = !cam2.enabled;
    }
}
