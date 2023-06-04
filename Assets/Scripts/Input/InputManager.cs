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
        if (Input.GetKeyDown(KeyCode.E)) HandleInteractionPressed();
        if (Input.GetKeyDown(KeyCode.Space)) HandleSpacePressed();

        if (Input.GetMouseButtonDown(1))
        {
            // handle right click
        }
    }

    private void HandleLeftClick()
    {   
        var mousePos = Utility.GetMouseWorldPosition2D();
        if (_inputState == InputState.ControllingPlayer) Game.Instance.PlayerCharacter.Attack(mousePos);
    }

    private void HandleInteractionPressed()
    {
        if (_inputState == InputState.ControllingPlayer) Game.Instance.PlayerCharacter.Interact();
    }

    private void HandleSpacePressed()
    {
        if (_inputState == InputState.ControllingPlayer) Game.Instance.PlayerCharacter.SwitchWeapon();
    }
    
    public InputState State
    {
        get => _inputState;
    }
}
