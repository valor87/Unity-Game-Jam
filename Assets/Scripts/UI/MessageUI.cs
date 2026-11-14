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
        EventCore.instance.UE_PausePlayer.Invoke();
        transform.GetChild(0).gameObject.SetActive(true);
        text.text = s;
    }

    public void ResumePlayerInvoke()
    {
        EventCore.instance.UE_ResumePlayer.Invoke();
    }
}
