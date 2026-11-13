using UnityEngine;

public class PlayerInteraction : MonoBehaviour
{
    [SerializeField] private Transform _cameraTransform;
    [SerializeField] private KeyCode _interactionKey = KeyCode.E;
    [SerializeField] private float _interactionDistance = 2.0f;
    void Update()
    {
        if (Input.GetKey(_interactionKey))
        {
            Interact();
        }

        DebugDrawRay();
    }
    private void Interact()
    {
        RaycastHit hit;

        if (Physics.Raycast(_cameraTransform.position, _cameraTransform.forward, out hit, _interactionDistance))
        {
            if (hit.collider.GetComponent<IInteractable>() == null)
                return;

            hit.collider.GetComponent<IInteractable>().Exacute();
        }
    }

    private void DebugDrawRay()
    {
        Debug.DrawLine(_cameraTransform.position, _cameraTransform.position + _cameraTransform.forward * _interactionDistance, Color.rebeccaPurple);
    }
}
