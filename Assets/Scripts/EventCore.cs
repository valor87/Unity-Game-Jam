using UnityEngine;
using UnityEngine.Events;

public class EventCore : MonoBehaviour
{
    public static EventCore instance;

    public UnityEvent<Transform> UE_RespawnPlayer;
    
    /// <summary>
    /// when the player is in the end elevator and 
    /// needs to be move to the begining again
    /// </summary>
    public UnityEvent<Vector3> UE_EnterEndElevator;
    /// <summary>
    /// Call when the obsticals move double the speed
    /// </summary>
    public UnityEvent UE_DoubleObsticalSpeed;
    /// <summary>
    /// Call when the obsticals move half the speed
    /// </summary>
    public UnityEvent UE_HalfObsticalSpeed;
    /// <summary>
    /// called everytime the player takes damage
    /// </summary>
    public UnityEvent<float> UE_PlayTakeDamge;
    /// <summary>
    /// For whenever the coin values changes. The int
    /// passed in the NEW coin value
    /// </summary>
    public UnityEvent<int> UE_ChangeCoinValue;
    /// <summary>
    /// double coin value got rolled
    /// </summary>
    public UnityEvent UE_DoubleCoinValue;
    /// <summary>
    /// Double damage got rolled
    /// </summary>
    public UnityEvent UE_DoubleDamage;
    /// <summary>
    /// Half damage got rolled
    /// </summary>
    public UnityEvent UE_HalfDamage;
    /// <summary>
    /// for when the player only takes three hits on the bottom platform
    /// </summary>
    public UnityEvent UE_TrippleDamge;
    /// <summary>
    /// Effect that causes the player to take damage when they're still
    /// </summary>
    public UnityEvent UE_TakeDamageWhenStill;
    /// <summary>
    /// Spawns coins for the coming level, int is the amount of coin in the level
    /// </summary>
    public UnityEvent<int> UE_SpawnCoins;
    /// <summary>
    /// Effect that causes the player to heal while they're moving
    /// </summary>
    public UnityEvent UE_HealWhenMoving;
    /// <summary>
    /// Effect that spawns 2 coins in the level
    /// </summary>
    public UnityEvent UE_SpawnTwoCoins;
    /// <summary>
    /// Effect that spawns no coins in the level
    /// </summary>
    public UnityEvent UE_SpawnNoCoins;
    /// <summary>
    /// give the player three sheilds
    /// </summary>
    public UnityEvent UE_GivePlayerSheild;
    /// Effect that makes floors slippery
    /// </summary>
    public UnityEvent UE_SetSlipperyFloors;
    /// <summary>
    /// Wind effect will push against you 
    /// </summary>
    public UnityEvent UE_StartWindEffect;
    /// <summary>
    /// Enables player double jumping mid air
    /// </summary>
    public UnityEvent UE_EnableDoubleJump;
    /// <summary>
    /// Enables bouncy walls
    /// </summary>
    public UnityEvent UE_EnableBouncyWalls;
    /// <summary>
    /// Enables bouncy walls
    /// </summary>
    public UnityEvent UE_EnableSuperJumpOffWalls;
    public UnityEvent UE_UsedShield;
    /// <summary>
    /// When triggering messages for the player
    /// </summary>
    public UnityEvent<string> UE_DisplayMessage;
        void Awake()
    {
        if (instance == null)
        {
            instance = this;
            return;
        }

        Destroy(gameObject);
    }

}
