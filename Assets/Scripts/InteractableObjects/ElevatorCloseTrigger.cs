using UnityEngine;

public class ElevatorCloseTrigger : MonoBehaviour
{
    [SerializeField] ElevatorDoor doors;
    private void OnTriggerEnter(Collider other)
    {
        if(other.name == "Player")
        {
            Elevator.instance.PlayerReachedExit();
            doors.CloseDoors();
        }
    }

    
}
