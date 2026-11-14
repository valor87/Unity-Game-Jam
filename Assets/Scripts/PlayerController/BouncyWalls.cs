using UnityEngine;

public class BouncyWalls : MonoBehaviour
{
    PlayerController controller;
    bool areWallsBouncy = false;
    private void Start()
    {
        EventCore.instance.UE_EnableBouncyWalls.AddListener(EnableBouncyWalls);
        EventCore.instance.UE_EnterEndElevator.AddListener(DisableBouncyWalls);
    }
    private void OnTriggerEnter(Collider other)
    {
        if (!other.CompareTag("Coin") && areWallsBouncy)
        {
            controller.Jump();
        }
    }

    private void EnableBouncyWalls()
    {
        areWallsBouncy = true;
    }
    private void DisableBouncyWalls(Vector3 arg)
    {
        areWallsBouncy = false;
    }
}
