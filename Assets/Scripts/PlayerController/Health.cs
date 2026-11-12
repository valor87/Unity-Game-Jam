using UnityEngine;
using UnityEngine.Rendering;

public class Health : MonoBehaviour, IDamageable
{
    [SerializeField] private float _maxHealth;
    [SerializeField] public float _currentHealth;
    public float maxHealth { get => _maxHealth; set => _maxHealth = value; }
    public float currentHealth { get => _currentHealth; set => _currentHealth = value; }
    public bool isPlayer { get; set;}

    void Start()
    {
        if(gameObject.CompareTag("Player"))
            isPlayer = true;
    }
}
