using Unity.VisualScripting;
using UnityEngine;

public class CameraController : MonoBehaviour
{
    public static CameraController instance;
    private float XRotation,  YRotation;
    public float sensitivity = 2;
    private float _pausedSens;
    private void Start()
    {
        instance = this;
        //Cursor.visible = false;
        Cursor.lockState = CursorLockMode.Locked;

        EventCore.instance.UE_PausePlayer.AddListener(PausePlayer);
        EventCore.instance.UE_ResumePlayer.AddListener(ResumePlayer);

    }
    
    private void Update()
    {
        //allow players to unlock their cursor

        XRotation += Input.GetAxis("Mouse Y") * -1 * sensitivity;
        YRotation += Input.GetAxis("Mouse X") * sensitivity;
        transform.localEulerAngles = new Vector3(XRotation, 0, 0);
        transform.parent.localEulerAngles = new Vector3(0, YRotation, 0);
    }

    void PausePlayer()
    {
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;
        _pausedSens = sensitivity;
        sensitivity = 0;
    }
    void ResumePlayer()
    {
        sensitivity = _pausedSens;
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }
}
