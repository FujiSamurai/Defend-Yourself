using SayMyName;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.Remoting.Contexts;
using UnityEditor.Rendering;
using UnityEngine;
using UnityEngine.InputSystem;

[CreateAssetMenu(menuName = "GameInput")]
public class GameInput : ScriptableObject, InputActions.IGamePlayActions, InputActions.IUIActions
{
    private InputActions _inputActions;

    private void OnEnable()
    {
        if (_inputActions == null)
        {
            _inputActions = new InputActions();

            _inputActions.GamePlay.SetCallbacks(this);
            _inputActions.UI.SetCallbacks(this);

            SetGamePlay();
        }
    }

    public event Action<Vector2> MovementEvent;

    public event Action SprintEvent;
    public event Action SprintCancelledEvent;

    public event Action JumpEvent;
    public event Action JumpCancelledEvent;

    public event Action PauseEvent;
    public event Action ResumeEvent;

    public event Action ExploreCamStyleEvent;
    public event Action CombatCamStyleEvent;
    public event Action TopdownCamStyleEvent;

    public void SetGamePlay()
    {
        _inputActions.GamePlay.Enable();
        _inputActions.UI.Disable();
    }

    public void SetUI()
    {
        _inputActions.GamePlay.Disable();
        _inputActions.UI.Enable();
    }

    public void OnMovement(InputAction.CallbackContext context)
    {
        MovementEvent?.Invoke(context.ReadValue<Vector2>());
    }

    public void OnJump(InputAction.CallbackContext context)
    {
        if (context.phase == InputActionPhase.Performed)
        {
            JumpEvent?.Invoke();
        }
        else
        {
            JumpCancelledEvent?.Invoke();
        }
    }

    public void OnPause(InputAction.CallbackContext context)
    {
        if (context.phase == InputActionPhase.Performed)
        {
            PauseEvent?.Invoke();
            SetUI();
        }
    }

    public void OnResume(InputAction.CallbackContext context)
    {
        if (context.phase == InputActionPhase.Performed)
        {
            ResumeEvent?.Invoke();
            SetGamePlay();
        }
    }

    public void OnSprint(InputAction.CallbackContext context)
    {
        if (context.phase == InputActionPhase.Performed)
        {
            SprintEvent?.Invoke();
        }
        else
        {
            SprintCancelledEvent?.Invoke();
        }
    }

    public void OnExploreCamStyle(InputAction.CallbackContext context)
    {
        if (context.phase == InputActionPhase.Performed)
        {
            ExploreCamStyleEvent?.Invoke();
        }
    }

    public void OnCombatCamStyle(InputAction.CallbackContext context)
    {
        if (context.phase == InputActionPhase.Performed)
        {
            CombatCamStyleEvent?.Invoke();
        }
    }

    public void OnTopdownCamStyle(InputAction.CallbackContext context)
    {
        if (context.phase == InputActionPhase.Performed)
        {
            TopdownCamStyleEvent?.Invoke();
        }
    }
}
