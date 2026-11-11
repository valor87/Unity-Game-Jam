using UnityEngine;

public class PlayerInteraction : MonoBehaviour
{
    [SerializeField] private KeyCode _interactionKey = KeyCode.E;
    [SerializeField] private float _interactionDistance = 2.0f;
    void Update()
    {
        Interact();
        DebugDrawRay();
    }
    private void Interact()
    {
        RaycastHit hit;

        if (Physics.Raycast(transform.position, transform.forward, out hit, _interactionDistance))
        {
            if (hit.collider.GetComponent<IInteractable>() == null)
                return;

            if (Input.GetKeyDown(_interactionKey))
            {
                hit.collider.GetComponent<IInteractable>().Exacute();
            }
        }
    }

    private void DebugDrawRay()
    {
        Debug.DrawLine(transform.position, transform.position + transform.forward * _interactionDistance, Color.rebeccaPurple);
    }
}
