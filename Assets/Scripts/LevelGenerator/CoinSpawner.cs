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
    void Start()
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

        Instantiate(coin).transform.position = spawnPositino;
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
