using UnityEngine;

public class TeleportPlayer : MonoBehaviour
{
    [SerializeField] private Transform _playerTransform;
    void Start()
    {
        //EventCore.instance.UE_EnterEndElevator.AddListener(MovePlayer);
    }

    void MovePlayer(Vector3 playerOffset)
    {
        playerOffset.y = 0;
        _playerTransform.position = transform.position - playerOffset;
    }
}
