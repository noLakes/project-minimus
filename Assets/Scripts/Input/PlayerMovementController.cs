using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovementController : MonoBehaviour
{
    [SerializeField] float speed;
    public Rigidbody2D rb;

    private Vector2 _moveDirection;
    
    private void Update()
    {
        var moveX = Input.GetAxisRaw("Horizontal");
        var moveY = Input.GetAxisRaw("Vertical");
        
        _moveDirection = new Vector2(moveX, moveY).normalized;
    }

    private void FixedUpdate() 
    {
        rb.velocity = _moveDirection * speed;
    }
}
