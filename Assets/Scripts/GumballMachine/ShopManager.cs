using UnityEngine;
using System.Collections.Generic;

public class ShopManager : MonoBehaviour
{
    public static ShopManager instance;
    [SerializeField] private CanvasGroup menu;
    public bool isShopOpen;
    private List<GameObject> availableItemsToSpawnList = new List<GameObject>();
    [SerializeField] private List<GameObject> items = new List<GameObject>();
    public List<GameObject> spawnedItems;
    [SerializeField] List<Transform> itemPlacements = new List<Transform>();
    private void Start()
    {
        instance = this;
    }
    public void ReplenishShop()
    {
        spawnedItems = new List<GameObject>() { Instantiate(items[0], menu.transform) };
        ListSpawnableItems();
        int numberOfItems = (availableItemsToSpawnList.Count > 2) ? 2 : availableItemsToSpawnList.Count;
        Debug.Log(numberOfItems);
        for (int i = 0; i < numberOfItems; i++)
        {
            int randomNum = Random.Range(0, numberOfItems);
            GameObject spawnedItem = Instantiate(availableItemsToSpawnList[randomNum], menu.transform);
            spawnedItems.Add(spawnedItem);
            spawnedItem.transform.position = itemPlacements[i].position;
            
        }
    }
    public void ListSpawnableItems()
    {
        for (int i = 1; i < items.Count; i++)
        {
            if (PhobiaManager.instance.Phobias[i])
            {
                availableItemsToSpawnList.Add(items[i]);
            }
        }
    }
    public void OpenShop()
    {
        Cursor.lockState = CursorLockMode.None;
        PlayerController.instance.enabled = false;
        CameraController.instance.enabled = false;
        menu.alpha = 1;
        isShopOpen = true;
    }
    public void CloseShop()
    {
        Cursor.lockState = CursorLockMode.Locked;
        menu.alpha = 0;
        PlayerController.instance.enabled = true;
        CameraController.instance.enabled = true;
        isShopOpen = false;
    }
}

