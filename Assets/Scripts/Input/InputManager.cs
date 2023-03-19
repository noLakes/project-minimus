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
    public Vector3 mouseWorldPosition { get; private set; }

    public InputState State
    {
        get => _inputState;
        set => _inputState = value;
    }

    public void Initialize(InputState startingState)
    {
        _inputState = startingState;
        mouseWorldPosition = Utility.GetMouseWorldPosition2D();
    }

    void Update()
    {
        mouseWorldPosition = Utility.GetMouseWorldPosition();
        if (Input.GetMouseButtonDown(0)) HandleLeftClick();
    }

    private void HandleLeftClick()
    {
        if (State == InputState.ControllingPlayer) Game.Instance.PlayerCharacter.Attack(mouseWorldPosition);
    }
}
