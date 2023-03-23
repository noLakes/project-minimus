using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Crosshair : MonoBehaviour
{
    Color _initialColor;
    Color _activeColor;
    SpriteRenderer _spriteRenderer;

    // Start is called before the first frame update
    void Start()
    {
        _spriteRenderer = GetComponent<SpriteRenderer>();
        _initialColor = _spriteRenderer.color;
        _activeColor = _initialColor;
        _activeColor.a = 0.9f;
        _spriteRenderer.color = _activeColor;
    }

    // Update is called once per frame
    void Update()
    {
        transform.position = Utility.GetMouseWorldPosition2D();
    }
}
