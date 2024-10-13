using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class HealthController : MonoBehaviour
{
    [SerializeField]
    private float _currentHealth;
    [SerializeField]
    private float _maxmimunHealth;
    
    public float RemainingHealthPercentage 
    {
        get 
        {
            return _currentHealth/_maxmimunHealth;
        }

    }
    public bool IsInvincible { get; set; }

    public UnityEvent OnDied;
    public UnityEvent OnDamaged;
    public UnityEvent OnHealthChanged;

    public void TakeDamage(float damageAmount) 
    {
        if (_currentHealth == 0) 
        {
            return;
        }

        if (IsInvincible) 
        {
            return;
        }


        _currentHealth-=damageAmount;
        OnHealthChanged.Invoke();
        if (_currentHealth < 0) 
        {
            _currentHealth = 0; 
        }
        if (_currentHealth == 0) 
        {
            OnDied.Invoke();
        }
        else 
        {
            OnDamaged.Invoke();
        }
    }
    public void AddHealth(float amountToAdd) 
    {
        if (_currentHealth == _maxmimunHealth) 
        {
            return ;
        }
        _currentHealth += amountToAdd;
        OnHealthChanged.Invoke();
        if (_currentHealth > _maxmimunHealth)  
        {
            _currentHealth= _maxmimunHealth;
        }
    }

}
