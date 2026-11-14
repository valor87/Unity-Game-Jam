using TMPro;
using UnityEngine;

public class MessageUI : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI text;

    void Start()
    {
        EventCore.instance.UE_DisplayMessage.AddListener(SetText);
    }
    void SetText(string s)
    {
        transform.GetChild(0).gameObject.SetActive(true);
        text.text = s;
    }
}
