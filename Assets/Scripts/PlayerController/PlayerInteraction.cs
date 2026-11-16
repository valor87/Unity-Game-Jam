using UnityEngine;

public class PlayerInteraction : MonoBehaviour
{
    [SerializeField] private Transform _cameraTransform;
    [SerializeField] private KeyCode _interactionKey = KeyCode.E;
    [SerializeField] private float _interactionDistance = 2.0f;
    [SerializeField] private CanvasGroup holdECG, tapECG;
    void Update()
    {
        CheckInteractions();
        DebugDrawRay();
    }
    private void CheckInteractions()
    {
        holdECG.alpha = tapECG.alpha = 0;
        RaycastHit hit;

        if (Physics.Raycast(_cameraTransform.position, _cameraTransform.forward, out hit, _interactionDistance))
        {

            if (hit.collider.GetComponent<IInteractable>() == null) return;


            //code for showing E icon when looking at something interactable
            if (hit.collider.GetComponent<IInteractable>().shouldShowInteractSign())
            {
                if (hit.collider.GetComponent<IInteractable>().isHoldToInteract())
                {
                    holdECG.alpha = 1;
                }
                else
                {
                    tapECG.alpha = 1;
                }
            }
            
            if (Input.GetKey(_interactionKey))
            {
                hit.collider.GetComponent<IInteractable>().Exacute();
                holdECG.alpha = tapECG.alpha = 0;
            }
        }
    }

    private void DebugDrawRay()
    {
        Debug.DrawLine(_cameraTransform.position, _cameraTransform.position + _cameraTransform.forward * _interactionDistance, Color.rebeccaPurple);
    }
}
