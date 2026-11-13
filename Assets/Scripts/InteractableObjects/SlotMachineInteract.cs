using UnityEngine;
using UnityEngine.UI;

public class SlotMachineInteract : MonoBehaviour, IInteractable
{
    private float heldTime, progress;
    [SerializeField] float timeToHold, decayrate;
    [SerializeField] AnimationCurve fillCurve;
    [SerializeField] Slider progressBarUI;
    private void Update()
    {
        heldTime -= Time.deltaTime * decayrate;
        heldTime = Mathf.Clamp(heldTime, 0, timeToHold);

        progressBarUI.value = progress = fillCurve.Evaluate(heldTime / timeToHold);

        if(progress == timeToHold)
        {
            Debug.Log("SLOT MACHINE FINISHED");
        }
    }
    public void Exacute()
    {
        heldTime += Time.deltaTime;
    }
}
