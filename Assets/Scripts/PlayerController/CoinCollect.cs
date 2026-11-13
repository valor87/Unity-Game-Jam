using UnityEngine;

public class CoinCollect : MonoBehaviour
{
    public int currentCoins = 0;
    public int coinIncreaseValue = 1;
    private int doubledCoinValue;
    private void Start()
    {
        doubledCoinValue = coinIncreaseValue * 2;
        EventCore.instance.UE_DoubleCoinValue.AddListener(DoubleCoinValue);
        EventCore.instance.UE_EnterEndElevator.AddListener(ResetCoinValue);
    }
    private void OnTriggerEnter(Collider other)
    {
        if (!other.CompareTag("Coin"))
            return;
        currentCoins += coinIncreaseValue;
        EventCore.instance.UE_ChangeCoinValue.Invoke(currentCoins);
        other.gameObject.SetActive(false);
    }

    void DoubleCoinValue()
    {
        coinIncreaseValue = doubledCoinValue;
    }

    void ResetCoinValue(Vector3 arg)
    {
        coinIncreaseValue = coinIncreaseValue/2;
    }
}
