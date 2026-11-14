using System.Collections.Generic;
using UnityEngine;

public class BouncyWalls : MonoBehaviour
{
    public PlayerController controller;
    bool areWallsBouncy = false;
    public List<Transform> wallsPlayerIsTouching = new List<Transform>();

    private void Start()
    {
        EventCore.instance.UE_EnableBouncyWalls.AddListener(EnableBouncyWalls);
        EventCore.instance.UE_EnterEndElevator.AddListener(DisableBouncyWalls);
    }
    private void Update()
    {
        if (areWallsBouncy && wallsPlayerIsTouching.Count > 0) controller.Jump(controller.jumpStrength);
    }
    private void OnTriggerEnter(Collider other)
    {
        if (!wallsPlayerIsTouching.Contains(other.transform) && !other.CompareTag("Coin"))
        {
            wallsPlayerIsTouching.Add(other.transform);

        }
    }
    private void OnTriggerExit(Collider other)
    {
        if (wallsPlayerIsTouching.Contains(other.transform))
        {
            wallsPlayerIsTouching.Remove(other.transform);
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
