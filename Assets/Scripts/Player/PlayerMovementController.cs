using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerMovementController : MonoBehaviour
{
    PlayerInputActions playerInput;
    CharacterController characterController;
    Camera playerView;

    Vector2 currentMovementInput;
    Vector3 currentMovement;
    Vector3 currentSprintMovement;

    Vector2 currentLookInput;
    Vector3 currentLook;
    

    float sprintModifier = 3f;
    float movementSpeed = 5f;
    bool isRunPressed;

    private void Awake()
    {
        playerInput = new PlayerInputActions();
        characterController = GetComponent<CharacterController>();
        playerView = GetComponentInChildren<Camera>();

        playerInput.Player.Move.started += onMovementInput;
        playerInput.Player.Move.canceled += onMovementInput;
        playerInput.Player.Move.performed += onMovementInput;
        playerInput.Player.Sprint.started += onSprint;
        playerInput.Player.Sprint.canceled += onSprint;
        playerInput.Player.Sprint.performed += onSprint;
        playerInput.Player.Look.started += onLookInput;
        playerInput.Player.Look.canceled += onLookInput;
        playerInput.Player.Look.performed += onLookInput;
        
    }

    void onLookInput(InputAction.CallbackContext context) {
        currentLookInput = context.ReadValue<Vector2>();

        playerView.transform.rotation = playerView.transform.rotation * Quaternion.Euler(currentLookInput.y, 0, 0);
        transform.rotation = transform.rotation * Quaternion.Euler(0, currentLookInput.x, 0);
    }

    void onSprint(InputAction.CallbackContext context) {
        isRunPressed = context.ReadValueAsButton();
    }

    void onMovementInput(InputAction.CallbackContext context) {
        currentMovementInput = context.ReadValue<Vector2>();
        currentMovement.x = currentMovementInput.x * movementSpeed;
        currentMovement.z = currentMovementInput.y * movementSpeed;
        currentSprintMovement.x = currentMovementInput.x * movementSpeed * sprintModifier;
        currentSprintMovement.z = currentMovementInput.y * movementSpeed * sprintModifier;
    }



    // Update is called once per frame
    void Update()
    {
        if (isRunPressed) {
            characterController.Move(currentSprintMovement * Time.deltaTime);
        } else {
            characterController.Move(currentMovement * Time.deltaTime);
        }

    }

    void OnEnable()
    {
        playerInput.Player.Enable();
    }

    void OnDisable()
    {
        playerInput.Player.Disable();
    }
}
