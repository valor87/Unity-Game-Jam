using System.Collections;
using UnityEngine;

public class ElevatorDoor : MonoBehaviour, IInteractable
{
    public bool interactable; //determines whether elevator door can be opened or not
    private bool isOpened;
    [SerializeField] private Transform leftDoor, rightDoor;
    [SerializeField] private float distanceToOpenDoor = 3.21999884f;
    [SerializeField] private float timeToOpenDoors;
    [SerializeField] private AnimationCurve doorOpenAnimationCurve;
    private Coroutine doorMovingCR;
    public void Exacute()
    {
        if (interactable)
        {
            OpenDoors();
        }
    }
    public void OpenDoors()
    {
        if (doorMovingCR == null && !isOpened)
        {
            doorMovingCR = StartCoroutine(MoveDoors(distanceToOpenDoor));
            isOpened = true;
        }
    }
    public void CloseDoors()
    {
        if(doorMovingCR == null && isOpened)
        {
            doorMovingCR = StartCoroutine(MoveDoors(distanceToOpenDoor));
            isOpened = false;
        }
        
    }
    private IEnumerator MoveDoors(float distance)
    {
        float time = 0;
        Vector3 startPosLeft = leftDoor.position;
        Vector3 startPosRight = rightDoor.position;
        while (time < timeToOpenDoors)
        {
            time+= Time.deltaTime;

            leftDoor.position = Vector3.Lerp(startPosLeft, startPosLeft + transform.forward * distance, doorOpenAnimationCurve.Evaluate(time / timeToOpenDoors));
            rightDoor.position = Vector3.Lerp(startPosRight, startPosRight - transform.forward * distance, doorOpenAnimationCurve.Evaluate(time / timeToOpenDoors));
            if (time > 8)
            {
                Debug.Log("SAFETY TRIPPED, EXITING ELEVATOR DOOR COROUTINE");
                break;
            }
            yield return null;
        }
        doorMovingCR = null;
    }

}
