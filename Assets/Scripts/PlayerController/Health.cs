using UnityEngine;
using UnityEngine.Rendering;

public class Health : MonoBehaviour, IDamageable
{
    private Rigidbody rb;
    [SerializeField] private float _maxHealth;
    [SerializeField] public float _currentHealth;
    [SerializeField] public int _currentSheild;

    public float maxHealth { get => _maxHealth; set => _maxHealth = value; }
    public float currentHealth { get => _currentHealth; set => _currentHealth = value; }
    public bool isPlayer { get; set;}
    public int _sheildValue { get => _currentSheild; set => _currentSheild = value; }

    //Effect stuff
    private bool takeDamageWhenStill = false, healWhenMoving = false;
    [SerializeField] float tickDamage = 0.1f;
    void Start()
    {
        rb = GetComponent<Rigidbody>();
        if(gameObject.CompareTag("Player"))
            isPlayer = true;
        //one phobia
        EventCore.instance.UE_TakeDamageWhenStill.AddListener(EnableTakeDamageWhileStill);
        EventCore.instance.UE_EnterEndElevator.AddListener(DisableTakeDamageWhileStill);
        //one philia
        EventCore.instance.UE_TakeDamageWhenStill.AddListener(EnableHealWhenMoving);
        EventCore.instance.UE_EnterEndElevator.AddListener(DisableHealWhenMoving);

        EventCore.instance.UE_GivePlayerSheild.AddListener(GiveSheild);
    }

    private void Update()
    {
        if (rb.linearVelocity.magnitude < 0.3f && takeDamageWhenStill)
        {
            GetComponent<IDamageable>().TakeDamage(tickDamage);
        }
        if (rb.linearVelocity.magnitude > 0.3f && healWhenMoving)
        {
            GetComponent<IDamageable>().TakeDamage(-tickDamage);
        }
    }
    private void GiveSheild()
    {
        _currentSheild = 3;
    }
    private void EnableTakeDamageWhileStill()
    {
        takeDamageWhenStill = true;
    }
    private void DisableTakeDamageWhileStill(Vector3 arg)
    {
        takeDamageWhenStill = false;
    }
    private void EnableHealWhenMoving()
    {
        healWhenMoving = true;
    }
    private void DisableHealWhenMoving(Vector3 arf)
    {
        healWhenMoving = false;
    }
}
