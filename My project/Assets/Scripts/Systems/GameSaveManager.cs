using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections.Generic;
using System;
using System.IO;
using System.Linq;

[Serializable]
public class GameSaveManager : Singleton<GameSaveManager> 
{
    [SerializeField]
    private string gameDataFilename = "game-save.json";
    private string gameDataFilePath;
    private List<string> saveItems;

    void Awake() 
    {
        gameDataFilePath = Application.persistentDataPath + "/" + gameDataFilename;
        saveItems = new List<string>();
        Debug.Log(gameDataFilePath);
    }

    public void AddObject(string item) 
    {
        saveItems.Add(item);
    }

    public void Save() 
    {
        saveItems.Clear();

        ISaveable[] saveableObjects = FindObjectsByType<MonoBehaviour>(FindObjectsSortMode.None).OfType<ISaveable>().ToArray();
        Debug.Log(saveableObjects.Length.ToString());
        foreach (ISaveable saveableObject in saveableObjects) 
        {
            saveItems.Add(saveableObject.Serialize());
        }

        using (StreamWriter gameDataFileStream = new StreamWriter(gameDataFilePath)) 
        {
            foreach (string item in saveItems) 
            {
                gameDataFileStream.WriteLine(item);
            }
        }
    }

    public void Load() 
    {
        using (StreamReader gameDataFileStream = new StreamReader(gameDataFilePath)) 
        {
            while (gameDataFileStream.Peek() >= 0) 
            {
                string line = gameDataFileStream.ReadLine().Trim();
                if (line.Length > 0) 
                {
                    saveItems.Add(line);
                }
            }
        }
        DestroyAllSaveableObjectsInScene();
        CreateSavedGameObjects();
    }

    void DestroyAllSaveableObjectsInScene() 
    {
        GameObject[] saveableObjects = FindObjectsByType<MonoBehaviour>(FindObjectsSortMode.None).Where(mo => mo is ISaveable).Select(mo => mo.gameObject).ToArray();
        foreach (GameObject saveableObject in saveableObjects) 
        {
            Destroy(saveableObject);
        }
    }

    void CreateSavedGameObjects() 
    {
        foreach (string saveItem in saveItems) 
        {
            Debug.Log(saveItem);
            string pattern = @"""prefabName"":""";
            int patternIndex = saveItem.IndexOf(pattern);
            int valueStartIndex = saveItem.IndexOf('"', patternIndex + pattern.Length - 1) + 1;
            int valueEndIndex = saveItem.IndexOf('"', valueStartIndex);
            string prefabName = saveItem.Substring(valueStartIndex, valueEndIndex - valueStartIndex);
            Debug.Log(prefabName);
            GameObject prefabObject = Resources.Load<GameObject>(prefabName);
            GameObject item = Instantiate(prefabObject);
            ISaveable saveable = item.GetComponent<ISaveable>();
            if(saveable != null)
            {
                saveable.Deserialize(saveItem);
            }
        }
    }
}
