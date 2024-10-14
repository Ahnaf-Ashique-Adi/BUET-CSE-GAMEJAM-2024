using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.Rendering;

public class PlayerMovement : MonoBehaviour
{
    [SerializeField]
    private float speed;
    [SerializeField]
    private float rotationSpeed;
    [SerializeField]
    private float screenBorder;
    private Rigidbody2D rigidbody;
    private Vector2 movementInput;
    private Vector2 smoothedMovementInput;
    private Vector2 movementInputSmoothVelocity;
    private Camera _camera;
    private object screenPosition;
    private Animator _animator;

    private void Awake()
    {
        rigidbody = GetComponent<Rigidbody2D>();
        _camera = Camera.main;
        _animator = GetComponent<Animator>();
    }


    private void FixedUpdate()
    {
        SetPlayerVelocity();
        RotateInDirectionInput();
        SetAnimation();
    }
    private void SetAnimation() 
    {
        bool isMoving = movementInput != Vector2.zero;

        _animator.SetBool("IsMoving",isMoving);
    }



    private void SetPlayerVelocity()
    {
        smoothedMovementInput = Vector2.SmoothDamp(smoothedMovementInput, movementInput, ref movementInputSmoothVelocity, 0.1f);

        rigidbody.velocity = smoothedMovementInput * speed;
        PreventPlayerGoingOffScreen();
    }

    private void PreventPlayerGoingOffScreen()
    {
        

        Vector2 screenPoint = _camera.WorldToScreenPoint(transform.position);

        if ((screenPoint.x < screenBorder && rigidbody.velocity.x < 0) || (screenPoint.x > _camera.pixelWidth - screenBorder && rigidbody.velocity.x > 0))
        {
            rigidbody.velocity = new Vector2(0, rigidbody.velocity.y);
        }

        if ((screenPoint.y < screenBorder && rigidbody.velocity.y < 0) || (screenPoint.y > _camera.pixelHeight - screenBorder && rigidbody.velocity.y > 0))
        {
            rigidbody.velocity = new Vector2(rigidbody.velocity.x, 0);
        }
    }

    private void RotateInDirectionInput() 
    {
        if (movementInput != Vector2.zero)
        {
            Quaternion targetRotation = Quaternion.LookRotation(transform.forward,smoothedMovementInput);
            Quaternion rotation = Quaternion.RotateTowards(transform.rotation, targetRotation, rotationSpeed * Time.deltaTime);
            rigidbody.MoveRotation(rotation);
        }
    }


    private void OnMove(InputValue inputValue)
    {
        movementInput= inputValue.Get<Vector2>();
    }
}
