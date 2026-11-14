using UnityEngine;

public class DialaugeTriggers : MonoBehaviour
{
    [TextArea(1,10)]
    [SerializeField] private string message;

    private void OnTriggerEnter(Collider other)
    {
        EventCore.instance.UE_DisplayMessage.Invoke(message);
    }
}
