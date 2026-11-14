using UnityEngine;

public interface IDamageable
{
    public float maxHealth { get; set; }
    public float currentHealth { get; set; }
    public bool isPlayer { get; set;}
    
    public int _sheildValue { get; set; }

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

        _sheildValue = Mathf.Clamp(_sheildValue, 0 , 3);
        if (_sheildValue != 0)
        {
            Debug.Log("The player had a sheild that protected them");
            _sheildValue--;
            EventCore.instance.UE_UsedShield.Invoke();
            return;
        }
        currentHealth -= incomingDamage;
        if (IsDead())
        {
            Debug.Log("im dead, send the player back to the main menu");
        }

        if (isPlayer)
        {
            EventCore.instance.UE_PlayTakeDamge.Invoke(currentHealth);

            if (SoundManager.Instance != null)
                SoundManager.Instance.PlayPlayerDamage();
        }
    }
}
