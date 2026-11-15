using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class HealthBarUI : MonoBehaviour
{
    [SerializeField] private Image _healthImage;
    void Start()
    {
        EventCore.instance.UE_UpdateHealthBar.AddListener(ChangeHealthBar);
    }
    void ChangeHealthBar(float currentHealth, float maxHP)
    {
        _healthImage.fillAmount = currentHealth/maxHP;
    }

    IEnumerator HeartAnimation()
    {
        yield return null;
    }
}
