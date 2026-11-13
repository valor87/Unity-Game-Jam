using UnityEngine;

public class Elevator : MonoBehaviour
{
    public static Elevator instance;
    private Transform playerTransform;
    [SerializeField] Transform startElevator;
    [SerializeField] ElevatorDoor levelExitDoors, levelEntranceDoors; //Player will ALWAYS enter through NORTH and exit through SOUTH
    [SerializeField] SlotMachine startElevatorSlotMachine;
    private void Start()
    {
        instance = this;
        playerTransform = GameObject.FindWithTag("Player").transform;
    }
    public void EndCurrentLevel()
    {
        UnloadLevel();
        //roll payline numbers
        LoadLevel();
    }
    private void UnloadLevel()
    {
       //unload current level between elevators
    }
    private void LoadLevel()
    {
        //based on rolled payline numbers
    }
    private void TeleportPlayerToStartElevator()
    {
        //called when succesfully interacted with slot machine (payline)
    }
    public void BeginLevel()
    {
        //Teleport player to start elevator
        Vector3 playerOffset = transform.position - playerTransform.position;
        EventCore.instance.UE_EnterEndElevator.Invoke(playerOffset);
        //do payline animation
        levelEntranceDoors.OpenDoors();
    }
}
