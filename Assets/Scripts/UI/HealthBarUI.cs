using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class HealthBarUI : MonoBehaviour
{
    [SerializeField] private Image _healthImage;
    void Start()
    {
        EventCore.instance.UE_PlayTakeDamge.AddListener(ChangeHealthBar);
    }
    void ChangeHealthBar(float currentHealth)
    {
        _healthImage.fillAmount = currentHealth/100;
    }

    IEnumerator HeartAnimation()
    {
        yield return null;
    }
}
