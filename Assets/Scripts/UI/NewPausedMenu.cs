using UnityEngine;

public class NewPausedMenu : MonoBehaviour
{
    [SerializeField] private GameObject _pauseMenu;
    [SerializeField] private KeyCode _menuInteract = KeyCode.Escape;

    [Header("For Effect Banner")]
    [SerializeField] private GameObject _bannerBackground;
    [SerializeField] private Vector3 _bannerLowerAmount = new (0, 100, 0);
    [SerializeField] private float _movementTime = 1;
    Vector2 _bannerStartPos;
    Vector2 _bannerEndPos;

    private float t;

    private void Start()
    {
        _bannerStartPos = _bannerBackground.transform.position;
        _bannerEndPos = _bannerBackground.transform.position;
        _bannerEndPos.y = 0;
    }

    public void OpenMenu()
    {
        _pauseMenu.SetActive(true);
    }

    public void CloseMenu()
    {
        _pauseMenu.SetActive(false);
        t = 0;
    }

    private void Update()
    {
        if (Input.GetKeyDown(_menuInteract))
        {
            if(!_pauseMenu.activeInHierarchy)
                OpenMenu();
            else if (_pauseMenu.activeInHierarchy)
                CloseMenu();
        }

        if (_pauseMenu.activeInHierarchy) {
            t += Time.deltaTime;
            _bannerBackground.transform.position = Vector2.Lerp(_bannerStartPos, _bannerEndPos, t / _movementTime);
        } 
    }
}
