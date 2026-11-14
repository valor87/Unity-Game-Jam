using UnityEngine;

public class ElevatorMusicStart : MonoBehaviour
{
    private void OnTriggerEnter(Collider other)
    {
        SoundManager.Instance.SetMusicState(SoundManager.MusicState.Elevator);
    }
}
