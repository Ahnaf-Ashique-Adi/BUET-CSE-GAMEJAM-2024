using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InvincibilityController : MonoBehaviour
{
   private HealthController _healthController;
    private SpriteFlash _spriteFlash;


    private void Awake()
    {
        _healthController = GetComponent<HealthController>();
        _spriteFlash = GetComponent<SpriteFlash>();
    }
    public void StartInvincibility(float invincibilityDuration, Color flashColor, int numberOffFlashes ) 
    {
        StartCoroutine(InvincibilityCoroutine(invincibilityDuration, flashColor, numberOffFlashes));
    }
    private IEnumerator InvincibilityCoroutine(float invincibilityDuration, Color flashColor, int numberOffFlashes) 
    {
        _healthController.IsInvincible = true;
         yield return _spriteFlash.FlashCoroutine(invincibilityDuration,flashColor, numberOffFlashes);
        _healthController.IsInvincible = false;
    }

}
