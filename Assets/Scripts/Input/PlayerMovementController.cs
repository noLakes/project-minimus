using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class PlayerMovementController : MonoBehaviour
{
    private Character _character;
    public Rigidbody2D rb;
    private List<Rigidbody2D> _pushedRigidBodies;

    private Vector2 _moveDirection;

    private void Start()
    {
        _character = GetComponent<CharacterManager>().Character;
        _pushedRigidBodies = new List<Rigidbody2D>();
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

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.layer == 7)
        {
            if (!other.transform.TryGetComponent<Rigidbody2D>(out var rb)) return;
            _pushedRigidBodies.Add(rb);
            rb.bodyType = RigidbodyType2D.Dynamic;
        }
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.gameObject.layer == 7)
        {
            if (!other.transform.TryGetComponent<Rigidbody2D>(out var rb)) return;
            rb.bodyType = RigidbodyType2D.Kinematic;
            rb.velocity = Vector2.zero;
            _pushedRigidBodies.Remove(rb);
        }
    }
}
