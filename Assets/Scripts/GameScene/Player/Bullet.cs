using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : MonoBehaviour
{
    private Camera _camera;
    private void Awake()
    {
        _camera = Camera.main;  
    }
    private void Update()
    {
         DestroyWhenOffScreen();    
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
       if(collision.GetComponent < EnemyMovement>()) 
        {
            HealthController healthController = collision.GetComponent<HealthController>();
            healthController.TakeDamage(10);
          
            Destroy(gameObject);
        }
    }


    private void DestroyWhenOffScreen()
    {
        Vector2 screenPoint = _camera.WorldToScreenPoint(transform.position);
        if (screenPoint.x < 0 || screenPoint.x > _camera.pixelWidth || screenPoint.y < 0 || screenPoint.y > _camera.pixelHeight)

        {
            Destroy(gameObject);
        }
    }
}
