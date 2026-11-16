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
        EventCore.instance.UE_ClearItemsFromShop.Invoke();
        GameObject spawnedHealthItem = Instantiate(items[0], menu.transform);
        spawnedHealthItem.transform.position = itemPlacements[itemPlacements.Count - 1].transform.position;
        spawnedItems = new List<GameObject>() { spawnedHealthItem };

        ListSpawnableItems();

        int numberOfItems = Mathf.Clamp(availableItemsToSpawnList.Count, 0, 2);

        for (int i = 0; i < numberOfItems; i++)
        {
            int randomNum = Random.Range(0, availableItemsToSpawnList.Count);
            foreach(GameObject item in spawnedItems)
            {
                if (availableItemsToSpawnList.Count == 1) break;
                Debug.Log("Checking item for duplicates: " + item.name + "|Item being checked: " + availableItemsToSpawnList[randomNum].name);
                if (availableItemsToSpawnList[randomNum].GetComponent<PhobiaItem>().number == item.GetComponent<PhobiaItem>()?.number)
                {
                    Debug.Log("Duplicate detected increasing random Num by 1");
                    randomNum++;
                    if (randomNum > availableItemsToSpawnList.Count - 1) randomNum = 0;
                    break;
                }
            }
            GameObject spawnedItem = Instantiate(availableItemsToSpawnList[randomNum], menu.transform);
            spawnedItems.Add(spawnedItem);
            spawnedItem.transform.position = itemPlacements[i].position;
        }
    }
    public void ListSpawnableItems()
    {
        availableItemsToSpawnList = new List<GameObject>();
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
        EventCore.instance.UE_OpenedShop.Invoke(true);
        EventCore.instance.UE_PausePlayer.Invoke();
        PlayerController.instance.enabled = false;
        menu.alpha = 1;
        isShopOpen = true;
        /*
        if (SoundManager.Instance != null)
            SoundManager.Instance.EnterShop();*/
    }
    public void CloseShop()
    {
        EventCore.instance.UE_OpenedShop.Invoke(false);
        EventCore.instance.UE_ResumePlayer.Invoke();
        menu.alpha = 0;
        PlayerController.instance.enabled = true;
        isShopOpen = false;

        if (SoundManager.Instance != null)
            SoundManager.Instance.ExitShop();
    }
}

