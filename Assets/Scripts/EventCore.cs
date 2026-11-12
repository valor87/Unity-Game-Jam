using UnityEngine;
using UnityEngine.Events;

public class EventCore : MonoBehaviour
{
    public static EventCore instance;
    /// <summary>
    /// when the player is in the end elevator and 
    /// needs to be move to the begining again
    /// </summary>
    public UnityEvent<Vector3> UE_EnterEndElevator;
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
