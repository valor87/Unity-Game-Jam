using Unity.VisualScripting;
using UnityEngine;

public class CameraController : MonoBehaviour
{
    private float XRotation,  YRotation;
    public float sensitivity = 2;
    private float _pausedSens;
    private void Start()
    {
        //Cursor.visible = false;
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;

        EventCore.instance.UE_PausePlayer.AddListener(PausePlayer);
        EventCore.instance.UE_ResumePlayer.AddListener(ResumePlayer);

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

    void PausePlayer()
    {
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;
        sensitivity = _pausedSens;
        sensitivity = 0;
    }
    void ResumePlayer()
    {
        sensitivity = _pausedSens;
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }
}
