using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyMovement : MonoBehaviour
{
    [SerializeField]
    private float speed;
    [SerializeField]
    private float rotationSpeed;
    [SerializeField]
    private float screenBorder;
    [SerializeField]
    private float _obstacleCheckCircleRadius;
    [SerializeField]
    private float _obstacleCheckDistance;
    [SerializeField]
    private LayerMask _obstacleLayerMask;

    private Rigidbody2D rb;
    private Vector2 targetDirection;
    private PlayerAwareness playerAwareness;
    private float changeDirectionCooldown;
    private Camera _camera;
    private RaycastHit2D[] _obstacleCollisions;
    private float _obstacleAvoidanceCooldown;
    private Vector2 _obstacleAvoidanceTargetDirection;


    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        playerAwareness = GetComponent<PlayerAwareness>();
        targetDirection = transform.up;
        _camera = Camera.main;
        _obstacleCollisions = new RaycastHit2D[10]; 

        if (rb == null)
        {
            Debug.LogError("Rigidbody2D component missing from this GameObject.");
        }
        if (playerAwareness == null)
        {
            Debug.LogError("PlayerAwareness component missing from this GameObject.");
        }
    }

    private void Start()
    {
       
        changeDirectionCooldown = Random.Range(1f, 5f);
    }

    private void FixedUpdate()
    {
        UpdateTargetDirection();
        RotateTowardsTarget();
        SetVelocity();
    }

    private void UpdateTargetDirection()
    {
        HandleRandomDirectionChange();
        HandlePlayerTargeting();
        HandleObstacles();
        HandleEnemyOffScreen();
    }

    private void HandleRandomDirectionChange()
    {
        changeDirectionCooldown -= Time.deltaTime; 

        if (changeDirectionCooldown <= 0)
        {
            float angleChange = Random.Range(-90f, 90f);
            Quaternion rotation = Quaternion.AngleAxis(angleChange, Vector3.forward);
            targetDirection = rotation * targetDirection;
            changeDirectionCooldown = Random.Range(1f, 5f);
        }
    }

    private void HandlePlayerTargeting()
    {
        if (playerAwareness.AwareOfPlayer)
        {
            targetDirection = playerAwareness.DirectionToPlayer;
        }
    }
    private void HandleEnemyOffScreen() 
    {

        Vector2 screenPoint = _camera.WorldToScreenPoint(transform.position);

        if ((screenPoint.x < screenBorder && targetDirection.x < 0) || (screenPoint.x > _camera.pixelWidth - screenBorder && targetDirection.x > 0))
        {
            targetDirection = new Vector2(- targetDirection.x, targetDirection.y);
        }

        if ((screenPoint.y < screenBorder && targetDirection.y < 0) || (screenPoint.y > _camera.pixelHeight - screenBorder && targetDirection.y > 0))
        {
            targetDirection = new Vector2(targetDirection.x, - targetDirection.y);
        }

    }

    private void HandleObstacles()
    {
        _obstacleAvoidanceCooldown-= Time.deltaTime;

        var contactFilter = new ContactFilter2D();
        contactFilter.SetLayerMask(_obstacleLayerMask);
        int numberOffCollisions = Physics2D.CircleCast(
            transform.position,
            _obstacleCheckCircleRadius,
            transform.up,
            contactFilter,
            _obstacleCollisions,
            _obstacleCheckDistance);

        for (int index = 0; index < numberOffCollisions; index++)
        {
            var obstacleCollision = _obstacleCollisions[index];

            if (obstacleCollision.collider.gameObject == gameObject)
            {
                continue;

            }
            if (_obstacleAvoidanceCooldown <= 0) 
            {
                _obstacleAvoidanceTargetDirection = obstacleCollision.normal;
                _obstacleAvoidanceCooldown = 0.5f;
            }
            var targetRotation = Quaternion.LookRotation(transform.forward, _obstacleAvoidanceTargetDirection); 
            var rotation = Quaternion.RotateTowards(transform.rotation,targetRotation,rotationSpeed* Time.deltaTime);

            targetDirection = rotation * Vector2.up;
            break;

        }
    }

    private void RotateTowardsTarget()
    {
        float targetAngle = Mathf.Atan2(targetDirection.y, targetDirection.x) * Mathf.Rad2Deg - 90f;
        float newAngle = Mathf.MoveTowardsAngle(rb.rotation, targetAngle, rotationSpeed * Time.deltaTime);
        rb.rotation = newAngle;

        Debug.Log($"Target Direction: {targetDirection}, Target Angle: {targetAngle}, New Rotation: {newAngle}");
    }

    private void SetVelocity()
    {
        
        rb.velocity = transform.up * speed;

        Debug.Log($"Velocity: {rb.velocity}");
    }
}
