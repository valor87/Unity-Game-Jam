using UnityEngine;

public class Elevator : MonoBehaviour
{
    [SerializeField] Transform startElevator;
    [SerializeField] ElevatorDoor northDoors, southDoors; //Player will ALWAYS enter through NORTH and exit through SOUTH
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
        //do payline animation
        //open south doors after animation ends
    }
}
