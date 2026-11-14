using UnityEngine;
using System.Collections.Generic;

public class ShopManager : MonoBehaviour
{
    public static ShopManager instance;
    [SerializeField] private CanvasGroup menu;
    public bool isShopOpen;
    [SerializeField] private List<GameObject> items = new List<GameObject>();
    public List<GameObject> spawnedItems;
    private void Start()
    {
        instance = this;
    }
    public void ReplenishShop()
    {
        spawnedItems = new List<GameObject>();
        for (int i = 0; i < 2; i++)
        {
            int randomNum = Random.Range(1, items.Count);
            spawnedItems.Add(Instantiate(items[randomNum]));
        }
    }
    public void OpenShop()
    {
        PlayerController.instance.enabled = false;
        menu.alpha = 1;
    }
    public void CloseShop()
    {
        menu.alpha = 0;
        PlayerController.instance.enabled = true;
    }
}

