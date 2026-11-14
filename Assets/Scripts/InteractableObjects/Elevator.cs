using UnityEngine;

public class Elevator : MonoBehaviour
{
    public static Elevator instance;
    private Transform playerTransform;
    [SerializeField] Transform startElevator, exitElevator;
    [SerializeField] ElevatorDoor levelExitDoors, levelEntranceDoors; //Player will ALWAYS enter through NORTH and exit through SOUTH
    [SerializeField] SlotMachine startElevatorSlotMachine;
    private int numberOfCoinsToSpawn = 1;
    private void Start()
    {
        instance = this;
        playerTransform = GameObject.FindWithTag("Player").transform;
        EventCore.instance.UE_SpawnTwoCoins.AddListener(SevenGoodEffect);
        EventCore.instance.UE_SpawnNoCoins.AddListener(SevenBadEffect);
        EventCore.instance.UE_JackpotEffect.AddListener(JackPotEffect);
        EventCore.instance.UE_EnterEndElevator.AddListener(ResetCoinvalue);
    }
    public void PlayerReachedExit()
    {
        EndLevel();
    }
    public void PlayerStartedLevel()
    {
        Debug.Log("Player started level");
        PhobiaManager.instance.StartAllActiveEffects();
        SlotMachine.Instance.ResetSlotMachineInteracts();
        EventCore.instance.UE_SpawnCoins.Invoke(numberOfCoinsToSpawn);
        levelExitDoors.OpenDoors();
        levelEntranceDoors.interactable = false;
        ShopManager.instance.ReplenishShop();

        if (SoundManager.Instance != null)
            SoundManager.Instance.SetMusicState(SoundManager.MusicState.Level);
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
        //Vector3 offset = playerTransform.position - exitElevator.position;
        playerTransform.position = startElevator.position + new Vector3(0f,playerTransform.localScale.y,0f);
        EventCore.instance.UE_EnterEndElevator.Invoke(Vector3.zero);

    }

    public void SevenGoodEffect()
    {
        numberOfCoinsToSpawn = 2;
    }
    public void SevenBadEffect()
    {
        numberOfCoinsToSpawn = 0;
    }
    public void ResetCoinvalue(Vector3 arg)
    {
        numberOfCoinsToSpawn = 1;
    }
    private void JackPotEffect()
    {
        numberOfCoinsToSpawn = 20;
    }
}
