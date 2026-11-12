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
