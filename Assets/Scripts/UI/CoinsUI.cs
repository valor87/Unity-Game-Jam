using TMPro;
using UnityEngine;

public class CoinsUI : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI text;
    
    void Start()
    {
        EventCore.instance.UE_ChangeCoinValue.AddListener(SetUIText);
        EventCore.instance.UE_ChangeCoinValue.Invoke(0);
    }
    private void SetUIText(int coinAmount)
    {
        text.text = $"Coins {coinAmount}";
    }
}
