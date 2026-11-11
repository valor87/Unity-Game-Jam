using UnityEngine;

public class CameraController : MonoBehaviour
{
    private float XRotation,  YRotation;
    public float sensitivity = 2;

    private void Start()
    {
        //Cursor.visible = false;
        Cursor.lockState = CursorLockMode.Locked;
    }
    private void Update()
    {
        //allow players to unlock their cursor
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            if(Cursor.lockState != CursorLockMode.Locked)
            {
                Cursor.lockState = CursorLockMode.Locked;
            } else
            {
                Cursor.lockState = CursorLockMode.Confined;
            }
        }

        XRotation += Input.GetAxis("Mouse Y") * -1 * sensitivity;
        YRotation += Input.GetAxis("Mouse X") * sensitivity;
        transform.localEulerAngles = new Vector3(XRotation, 0, 0);
        transform.parent.localEulerAngles = new Vector3(0, YRotation, 0);
    }
}
