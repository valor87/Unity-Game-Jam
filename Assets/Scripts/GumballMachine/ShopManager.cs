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
    private void Start()
    {
        instance = this;
    }
    public void ReplenishShop()
    {
        spawnedItems = new List<GameObject>();
        ListSpawnableItems();
        for (int i = 0; i < 2; i++)
        {
            int randomNum = Random.Range(1, availableItemsToSpawnList.Count);
            spawnedItems.Add(Instantiate(availableItemsToSpawnList[randomNum], menu.transform));
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
        PlayerController.instance.enabled = false;
        CameraController.instance.enabled = false;
        menu.alpha = 1;
        isShopOpen = true;
    }
    public void CloseShop()
    {
        menu.alpha = 0;
        PlayerController.instance.enabled = true;
        CameraController.instance.enabled = true;
        isShopOpen = false;
    }
}

