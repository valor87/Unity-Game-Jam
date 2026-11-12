using System.Collections;
using UnityEngine;

public class ElevatorDoor : MonoBehaviour, IInteractable
{
    public bool interactable; //determines whether elevator door can be opened or not

    [SerializeField] private float distanceToOpenDoor = 3.21999884f;
    [SerializeField] private float timeToOpenDoor;
    [SerializeField] private AnimationCurve doorOpenAnimationCurve;
    private Vector3 doorOpenPosition, doorClosePosition;
    private void Start()
    {
        doorClosePosition = transform.position;
        doorOpenPosition = transform.position + Vector3.right * distanceToOpenDoor;
    }
    public void Exacute()
    {
        if (interactable)
        {
            StartCoroutine(OpenOrCloseDoor(doorOpenPosition));
        }
    }

    public IEnumerator OpenOrCloseDoor(Vector3 destination)
    {
        float time = 0;
        Vector3 startPos = transform.position;
        while (transform.position != destination)
        {
            time+= Time.deltaTime;

            transform.position = Vector3.Lerp(startPos, destination, doorOpenAnimationCurve.Evaluate(time / timeToOpenDoor));

            if(time > 8)
            {
                Debug.Log("SAFETY TRIPPED, EXITING ELEVATOR DOOR COROUTINE");
                break;
            }
            yield return null;
        }
    }

}
