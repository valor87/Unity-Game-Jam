using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class DamgeUI : MonoBehaviour
{
    [SerializeField] private Image _hurtImage;
    [SerializeField] private float _transparencyAmount = .25f;
    private bool running = false;
    void Start()
    {
        EventCore.instance.UE_PlayTakeDamge.AddListener(startDamageUI);
    }
    private void startDamageUI()
    {
        if (running)
            return;
        StartCoroutine(DamgeUiAnimation());
    }
    IEnumerator DamgeUiAnimation()
    {
        running = true;
        Color baseColor = _hurtImage.color;
        float transparency = _transparencyAmount;
        while(transparency >= 0){
            transparency -= 0.1f;
            baseColor.a = transparency;
            _hurtImage.color = baseColor;
            yield return new WaitForSeconds(.10f);
        }
        running = false;
    }
}
