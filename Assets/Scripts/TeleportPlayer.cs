using UnityEngine;

public class TeleportPlayer : MonoBehaviour
{
    [SerializeField] private Transform _playerTransform;
    void Start()
    {
        EventCore.instance.UE_EnterEndElevator.AddListener(MovePlayer);
    }

    void MovePlayer()
    {
        Debug.Log("Instance filled");
        _playerTransform.position = transform.position;
    }
}
