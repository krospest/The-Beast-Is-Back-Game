using UnityEngine;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using System;

public class GameInput : MonoBehaviour
{
    public static GameInput Instance {  get; private set; }
    public event EventHandler OnPlayerAttack;
    private PlayerInputActions playerInputActions;
    [SerializeField] private InteractionDetector _interactionDetector;
    
    private void Awake()
    {
        Instance = this;

        playerInputActions = new PlayerInputActions();
        playerInputActions.Enable();

        playerInputActions.Attack.SwordAttack.started += SwordAttack_started;
        playerInputActions.Interaction.Interaction.started += Interaction_started;

    }

    private void Interaction_started(InputAction.CallbackContext obj)
    {
        _interactionDetector.OnInteract(obj);
    }

    private void SwordAttack_started(InputAction.CallbackContext obj)
    {
       OnPlayerAttack?.Invoke(this, EventArgs.Empty);
    }

    public Vector2 GetMovementVector()
    {
        Vector2 inputVector = playerInputActions.Player.Movement.ReadValue<Vector2>();
        return inputVector;
    }

}
