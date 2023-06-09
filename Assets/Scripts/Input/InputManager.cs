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
    private Vector2 mousePos;

    public void Initialize(InputState startingState)
    {
        _inputState = startingState;
    }

    private void Update()
    {
        // store mouse pos if any input occurs or is ongoing
        if (Input.anyKeyDown || Input.anyKey) mousePos = Utility.GetMouseWorldPosition2D();
        else return;
        
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            if (Game.Instance.GameIsPaused) Game.Instance.Resume();
            else Game.Instance.Pause();
        }

        if (Game.Instance.GameIsPaused) return;

        if (Input.GetMouseButtonDown(0)) HandleLeftClick();
        else if (Input.GetMouseButton(0)) HandleLeftClickHeld();
        else if (Input.GetMouseButtonUp(0)) HandleLeftClickReleased();
            
        if (Input.GetKeyDown(KeyCode.E)) HandleInteractionPressed();
        if (Input.mouseScrollDelta.y != 0f) HandleWeaponChange();

        if (Input.GetMouseButtonDown(1))
        {
            // handle right click
            // secondary fire?
        }

        if (Input.GetKeyDown(KeyCode.Q)) HandleActiveItemUsed();
        if (Input.GetKeyDown(KeyCode.Space)) HandleSpecialAbilityUsed();
    }

    private void HandleLeftClick()
    {
        if (_inputState == InputState.ControllingPlayer) Game.Instance.PlayerCharacter.Attack(mousePos);
    }

    private void HandleLeftClickHeld()
    {
        if (_inputState == InputState.ControllingPlayer) Game.Instance.PlayerCharacter.Attack(mousePos);
    }

    private void HandleLeftClickReleased()
    {
        // trigger some action for certain weapon fire types?
    }

    private void HandleInteractionPressed()
    {
        if (_inputState == InputState.ControllingPlayer) Game.Instance.PlayerCharacter.Interact();
    }

    private void HandleWeaponChange()
    {
        if (_inputState == InputState.ControllingPlayer) Game.Instance.PlayerCharacter.SwitchWeapon();
    }

    private void HandleActiveItemUsed()
    {
        if (_inputState == InputState.ControllingPlayer) Game.Instance.PlayerCharacter.UseActiveItem(mousePos);
    }

    private void HandleSpecialAbilityUsed()
    {
        if (_inputState == InputState.ControllingPlayer) Game.Instance.PlayerCharacter.UseAbility(mousePos);
    }
    
    public InputState State
    {
        get => _inputState;
    }
}
