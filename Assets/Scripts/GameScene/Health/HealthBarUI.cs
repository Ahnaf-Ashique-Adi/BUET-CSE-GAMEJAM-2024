using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI; 

public class NewBehaviourScript : MonoBehaviour
{
    [SerializeField]
    private Image _healthBarForegroundImage; 

    private float lerpSpeed = 5f; 

   
    public void UpdateHealthBar(HealthController healthController)
    {
        
        StartCoroutine(SmoothHealthChange(healthController.RemainingHealthPercentage));
    }

    
    private IEnumerator SmoothHealthChange(float targetFillAmount)
    {
       
        float currentFillAmount = _healthBarForegroundImage.fillAmount;

        
        float elapsedTime = 0f;

        
        while (elapsedTime < 1f)
        {
            
            elapsedTime += Time.deltaTime * lerpSpeed;

            
            _healthBarForegroundImage.fillAmount = Mathf.Lerp(currentFillAmount, targetFillAmount, elapsedTime);

            
            yield return null;
        }

        
        _healthBarForegroundImage.fillAmount = targetFillAmount;
    }
}
