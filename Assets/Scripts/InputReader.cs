using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class InputReader : MonoBehaviour {
    private GameInput gameInput;

    private void Awake() {
        gameInput = new GameInput();
        gameInput.Player.Enable();
    }

    public Vector2 GetMovementVector() {
        Vector2 inputVector = gameInput.Player.Move.ReadValue<Vector2>();
        return inputVector;
    }

    public bool GetSprintInput() {
        return gameInput.Player.Sprint.IsPressed();
    }

    public Vector2 GetLookVector() {
        Vector2 lookDir = gameInput.Player.Look.ReadValue<Vector2>();
        return lookDir;
    }
}
