using UnityEngine;

public class Elevator : MonoBehaviour
{
    public static Elevator instance;
    private Transform playerTransform;
    [SerializeField] Transform startElevator, exitElevator;
    [SerializeField] ElevatorDoor levelExitDoors, levelEntranceDoors; //Player will ALWAYS enter through NORTH and exit through SOUTH
    [SerializeField] SlotMachine startElevatorSlotMachine;
    private void Start()
    {
        instance = this;
        playerTransform = GameObject.FindWithTag("Player").transform;
    }
    public void PlayerReachedExit()
    {
        EndLevel();
    }
    public void MakeStartDoorsInteractable()
    {
        levelEntranceDoors.interactable = true;
    }
    private void EndLevel()
    {
        //Teleport player to start elevator and make start doors uninteractable and make slot machine interactable
        levelEntranceDoors.interactable = false;
        SlotMachine.Instance.interactable = true;
        Vector3 offset = playerTransform.position - exitElevator.position;
        playerTransform.position = startElevator.position + offset;
        EventCore.instance.UE_EnterEndElevator.Invoke(offset);
    }
}
