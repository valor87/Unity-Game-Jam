using UnityEngine;

public class PlayerLeftStartTrigger : MonoBehaviour
{
    [SerializeField] ElevatorDoor doors;
    private void OnTriggerEnter(Collider other)
    {
        if (other.name == "Player")
        {
            doors.CloseDoors();
        }
    }
}
