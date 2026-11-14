using UnityEngine;

public class DialaugeTriggers : MonoBehaviour
{
    [TextArea(1,10)]
    [SerializeField] private string _message;
    private void DisplayMessage()
    {
        EventCore.instance.UE_DisplayMessage.Invoke(_message);
    }
    private void OnTriggerEnter(Collider other)
    {
        DisplayMessage();
        GetComponent<BoxCollider>().enabled = false;
    }
}
