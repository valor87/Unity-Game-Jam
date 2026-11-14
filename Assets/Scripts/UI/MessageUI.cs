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
        Time.timeScale = 0f;
        EventCore.instance.UE_PausePlayer.Invoke();
        transform.GetChild(0).gameObject.SetActive(true);
        text.text = s;

        if (SoundManager.Instance != null)
            SoundManager.Instance.PlayPopup();
    }

    public void ResumePlayerInvoke()
    {
        Time.timeScale = 1f;
        EventCore.instance.UE_ResumePlayer.Invoke();
    }
}
