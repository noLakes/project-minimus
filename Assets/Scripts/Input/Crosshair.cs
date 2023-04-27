using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Crosshair : MonoBehaviour
{
    private Color _initialColor;
    private Color _activeColor;
    private SpriteRenderer _spriteRenderer;
    
    private void Start()
    {
        _spriteRenderer = GetComponent<SpriteRenderer>();
        _initialColor = _spriteRenderer.color;
        _activeColor = _initialColor;
        _activeColor.a = 0.9f;
        _spriteRenderer.color = _activeColor;
    }
    
    private void Update()
    {
        transform.position = Utility.GetMouseWorldPosition2D();
    }
}
