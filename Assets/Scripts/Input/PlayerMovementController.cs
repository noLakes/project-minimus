using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovementController : MonoBehaviour
{
    private Character _character;
    public Rigidbody2D rb;

    private Vector2 _moveDirection;

    private void Start()
    {
        _character = GetComponent<CharacterManager>().Character;
    }

    private void Update()
    {
        if (Game.Instance.GameIsPaused) return;
        
        var moveX = Input.GetAxisRaw("Horizontal");
        var moveY = Input.GetAxisRaw("Vertical");
        
        _moveDirection = new Vector2(moveX, moveY).normalized;
    }

    private void FixedUpdate() 
    {
        if (Game.Instance.GameIsPaused)
        {
            rb.velocity = Vector3.zero;
            return;
        }
        
        rb.velocity = _moveDirection * _character.Stats.speed;
    }
}
