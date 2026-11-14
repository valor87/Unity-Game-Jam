using UnityEngine;
using System.Collections.Generic;
public class CoinManager : MonoBehaviour
{
    [SerializeField] private int _amountOfCoins;
    [SerializeField] private List<GameObject> _coinPool = new List<GameObject>();
    [SerializeField] GameObject _coinPrefab;

    [SerializeField] List<CoinSpawner> _allCoinPositions = new List<CoinSpawner>();
    [SerializeField] List<CoinSpawner> _remainingPositions = new List<CoinSpawner>();
    [SerializeField] private GameObject _groundBlock;
    [SerializeField] private Material _lavaAnimation;
    private Material _baseMaterial;
    void Start()
    {
        EventCore.instance.UE_SpawnCoins.AddListener(CoinSpawnLoop);
        EventCore.instance.UE_666Effect.AddListener(SetLavaGround);
        EventCore.instance.UE_EnterEndElevator.AddListener(ResetGroundMaterial);

        for (int i = 0; i < _amountOfCoins; i++)
        {
            GameObject c = Instantiate(_coinPrefab);
            c.SetActive(false);
            _coinPool.Add(c);
        }

        for (int i = 0; i < transform.childCount; i++)
        {
            if (transform.GetChild(i).GetComponent<CoinSpawner>() != null)
            {
                _allCoinPositions.Add(transform.GetChild(i).GetComponent<CoinSpawner>());
            }
        }
        SetLists<CoinSpawner>(_allCoinPositions, _remainingPositions);
    }

    void CoinSpawnLoop(int i)
    {
        for (int loopNum = 0; loopNum < i; loopNum++)
        {
            SpawnRandomCoin();
        }
    }

    [ContextMenu("SpawnRandomCoin")]
    void SpawnRandomCoin()
    {
        GameObject usedCoin = null;
        int r = Random.RandomRange(0, _remainingPositions.Count);

        foreach (var t in _coinPool)
        {
            if (!t.activeInHierarchy)
                usedCoin = t;
        }

        _remainingPositions[r].SpawnCoint(usedCoin);
        
        if (_remainingPositions.Count <= 1)
        {
            SetLists<CoinSpawner>(_allCoinPositions, _remainingPositions);
        }
        else
        {
            _remainingPositions.RemoveAt(r);
        }
    }

    void SetLists<t>(List<t> temp, List<t> temp2)
    {
        temp2.Clear();
        foreach (t var in temp)
        {
            temp2.Add(var);
        }
    }

    void ResetGroundMaterial(Vector3 arg)
    {
        _groundBlock.GetComponent<MeshRenderer>().material = _baseMaterial;
    }
    void SetLavaGround()
    {
        _baseMaterial = _groundBlock.GetComponent<MeshRenderer>().material;
        _groundBlock.GetComponent<MeshRenderer>().material = _lavaAnimation;
    }
}
