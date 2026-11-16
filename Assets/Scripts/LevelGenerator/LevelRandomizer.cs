using NUnit.Framework;
using UnityEngine;
using System.Collections.Generic;
public class LevelRandomizer : MonoBehaviour
{
    [SerializeField] private List<GameObject> levelPrefabs;
    private int currentLevelIndex;
    private void Start()
    {
        EventCore.instance.UE_LoadNewLevel.AddListener(StartNewLevel);
    }
    public void StartNewLevel()
    {
        UnloadLevel();
        RandomizeLevel();
        LoadLevel();
    }
    private void RandomizeLevel()
    {
        int randomNum = Random.Range(0, levelPrefabs.Count-1);
        currentLevelIndex = (randomNum == currentLevelIndex) ? randomNum + 1 : randomNum;
        currentLevelIndex = (currentLevelIndex > levelPrefabs.Count) ? 0 : currentLevelIndex;
    }
    private void UnloadLevel()
    {
        levelPrefabs[currentLevelIndex].SetActive(false);
    }
    private void LoadLevel()
    {
        levelPrefabs[currentLevelIndex].SetActive(true);
    }
}

