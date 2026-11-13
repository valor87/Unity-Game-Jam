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
    /// called everytime the player takes damage
    /// </summary>
    public UnityEvent UE_PlayTakeDamge;
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
    /// for when the player only takes three hits on the bottom platform
    /// </summary>
    public UnityEvent UE_TrippleDamge;

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
