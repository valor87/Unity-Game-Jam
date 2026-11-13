using UnityEngine;
using UnityEngine.UI;

public class SlotMachineInteract : MonoBehaviour, IInteractable
{
    private float heldTime, progress;
    [SerializeField] float timeToHold, decayrate;
    [SerializeField] AnimationCurve fillCurve;
    [SerializeField] Slider progressBarUI;
    private bool interactable = true;
    private void Update()
    {
        heldTime -= Time.deltaTime * decayrate;
        heldTime = Mathf.Clamp(heldTime, 0, timeToHold);

        progressBarUI.value = progress = fillCurve.Evaluate(heldTime / timeToHold);

        if(progress == timeToHold && interactable)
        {
            SlotMachine.Instance.RollRandomNumbers();
            interactable = false;
        }
    }
    public void Exacute()
    {
        if (interactable)
        {
            heldTime += Time.deltaTime;
        }
    }
}
