using UnityEngine;

public class CoinCollect : MonoBehaviour
{
    public int currentCoins = 0;

    private void OnTriggerEnter(Collider other)
    {
        if (!other.CompareTag("Coin"))
            return;
        currentCoins++;
        EventCore.instance.UE_ChangeCoinValue.Invoke(currentCoins);
        other.gameObject.SetActive(false);
    }
}
