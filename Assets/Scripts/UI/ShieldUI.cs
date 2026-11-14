using UnityEngine;
using UnityEngine.UI;

public class ShieldUI : MonoBehaviour
{
    [SerializeField] private Image[] _shieldUI = new Image[3];

    void Start()
    {
        EventCore.instance.UE_UsedShield.AddListener(RemoveShield);
        EventCore.instance.UE_GivePlayerSheild.AddListener(GiveShield);
    }

    void GiveShield()
    {
        foreach (Image _shieldUI in _shieldUI)
        {
            _shieldUI.enabled = true;
        }
    }

    void RemoveShield()
    {
        for (int i = 0; i < _shieldUI.Length; i++)
        {
            if (_shieldUI[i].isActiveAndEnabled)
            {
                _shieldUI[i].enabled = false;
                break;
            }
        }
    }
}
