using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum InputState
{
    ControllingPlayer
}

public class InputManager : MonoBehaviour
{
    private InputState _inputState;

    public void Initialize(InputState startingState)
    {
        _inputState = startingState;
    }

    private void Update()
    {
        if (Input.GetMouseButtonDown(0)) HandleLeftClick();
    }

    private void HandleLeftClick()
    {   
        var mousePos = Utility.GetMouseWorldPosition2D();
        if (_inputState == InputState.ControllingPlayer) Game.Instance.PlayerCharacter.Attack(mousePos);
    }
    
    public InputState State
    {
        get => _inputState;
    }
}
