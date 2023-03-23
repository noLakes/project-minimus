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
    [HideInInspector]

    public InputState State
    {
        get => _inputState;
        set => _inputState = value;
    }

    public void Initialize(InputState startingState)
    {
        _inputState = startingState;
    }

    void Update()
    {
        if (Input.GetMouseButtonDown(0)) HandleLeftClick();
    }

    private void HandleLeftClick()
    {   
        Vector2 mousePos = Utility.GetMouseWorldPosition2D();
        if (State == InputState.ControllingPlayer) Game.Instance.PlayerCharacter.Attack(mousePos);
    }
}
