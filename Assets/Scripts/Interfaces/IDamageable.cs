using System.Runtime.CompilerServices;
using UnityEngine;

public interface IDamageable
{
    public float maxHealth { get; set; }
    public float currentHealth { get; set; }
    public bool isPlayer { get; set;}
    public bool IsDead()
    {
        if (currentHealth <= 0)
        {
            return true;
        }
        return false;
    }
    public void TakeDamage(float incomingDamage)
    {
        currentHealth -= incomingDamage;
        if (IsDead())
        {
            Debug.Log("im dead");
        }

        if (isPlayer)
        {
            EventCore.instance.UE_PlayTakeDamge.Invoke(currentHealth);
        }
    }
}
