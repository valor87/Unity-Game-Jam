using UnityEngine;

public class RespawnPoint : MonoBehaviour
{
    private void Start()
    {
        EventCore.instance.UE_RespawnPlayer.AddListener(RespawnPlayer);
    }
    private void RespawnPlayer(Transform _playerTransform)
    {
        _playerTransform.position = transform.position + new Vector3(0,_playerTransform.localScale.y,0);
    }
}
