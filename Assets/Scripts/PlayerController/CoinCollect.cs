using UnityEngine;

public class CoinCollect : MonoBehaviour
{
    public int currentCoins = 0;
    public int coinIncreaseValue = 1;
    private void OnTriggerEnter(Collider other)
    {
        if (!other.CompareTag("Coin"))
            return;
        currentCoins += coinIncreaseValue;
        EventCore.instance.UE_ChangeCoinValue.Invoke(currentCoins);
        other.gameObject.SetActive(false);
    }
}
