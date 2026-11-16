using UnityEngine;
using UnityEngine.EventSystems;

public class ShopItem : MonoBehaviour, IPointerClickHandler
{
    [SerializeField]protected int cost;
    protected void Start()
    {
        EventCore.instance.UE_ClearItemsFromShop.AddListener(DestroyItem);
    }
    protected virtual void Purchase()
    {
        CoinCollect.instance.currentCoins -= cost;
        EventCore.instance.UE_ChangeCoinValue.Invoke(CoinCollect.instance.currentCoins);
        if (SoundManager.Instance != null)
            SoundManager.Instance.PlayShopPurchase();

        Destroy(gameObject);
    }
    protected virtual void TryPurchase() { } //put the conditions required for purchase here and call Purchase() from inside
    
    protected void DestroyItem()
    {
        Destroy(gameObject);
    }

    void IPointerClickHandler.OnPointerClick(PointerEventData eventData)
    {
        Debug.Log("Ive been clicked");
        TryPurchase();
    }
}
