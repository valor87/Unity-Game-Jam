using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class SlotMachineInteract : MonoBehaviour, IInteractable
{
    private float heldTime, progress;
    [SerializeField] float timeToHold, decayrate;

    //connects mesh animations
    public Animator SlotMeshAnim;
    [SerializeField] AnimationCurve fillCurve;
    [SerializeField] Slider progressBarUI;
    [SerializeField] CanvasGroup canvasGroup;
    private void Update()
    {
        if (SlotMachine.Instance.interactable)
        {
            heldTime -= Time.deltaTime * decayrate;
        }
        heldTime = Mathf.Clamp(heldTime, 0, timeToHold);

        progressBarUI.value = progress = fillCurve.Evaluate(heldTime / timeToHold);

        if(progress == timeToHold && SlotMachine.Instance.interactable)
        {
            StartCoroutine(FadeCanvasGroup());
            SlotMachine.Instance.StartSlotMachine();
            SlotMachine.Instance.interactable = false;
        }
    }
    public void Exacute()
    {
        SlotMeshAnim.SetTrigger("Spin");
        if (SlotMachine.Instance.interactable)
        {
            heldTime += Time.deltaTime;
        }
    }

    private IEnumerator FadeCanvasGroup()
    {
        float alpha = 1;
        while (alpha > 0)
        {
            alpha -= Time.deltaTime;
            canvasGroup.alpha = alpha;
            yield return null;
        }
    }
    public void ResetValues()
    {
        heldTime = 0;
        canvasGroup.alpha = 1;
    }
}
