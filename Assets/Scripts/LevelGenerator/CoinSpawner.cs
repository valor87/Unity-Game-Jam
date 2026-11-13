using UnityEngine;
public enum CoinPosition {
    None,
    Top,
    Bottom,
    Right,
    Left,
}

public class CoinSpawner : MonoBehaviour
{
    [SerializeField] private GameObject coin;
    [SerializeField] private CoinPosition coinPos;
    public void SpawnCoint(GameObject c)
    {
        Vector3 spawnPositino = transform.position;
        switch (coinPos)
        {
            case CoinPosition.Top:
                spawnPositino.y += transform.localScale.y * 2;
                break;
            case CoinPosition.Bottom:
                spawnPositino.y -= transform.localScale.y * 2;
                break;
            case CoinPosition.Right:
                spawnPositino.x -= transform.localScale.x;
                break;
            case CoinPosition.Left:
                spawnPositino.x += transform.localScale.x;
                break;
        }

        c.transform.position = spawnPositino;
        c.SetActive(true);
    }
}
