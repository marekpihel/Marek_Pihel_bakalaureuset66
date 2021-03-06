using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerMovementController : MonoBehaviour
{
    [SerializeField]
    VirtualFootStepSound virtualFootStepSound;
    
    PlayerInputActions playerInput;
    CharacterController characterController;
    Camera playerView;
    AudioSource playerFootstep;


    Vector2 currentMovementInput;
    Vector3 currentMovement;

    Vector2 currentLookInput;

    float movementSpeed = 5f, stepTimer = 0, stepCooldown = 0.4f, sprintModifier = 1.5f, sneakModifier = 0.5f;


    bool isSprintPressed, isSneakPressed, checkForStandingUp, footstepsDisabled;

    float upDownAngle = 0f;
    float mouseSensitivity = 0.1f;

    void initInputSystem(PlayerInputActions.PlayerActions playerActions)
    {
        #region Subscriptions
        #region Movement
        playerActions.Move.started += onMovementInput;
        playerActions.Move.canceled += onMovementInput;
        playerActions.Move.performed += onMovementInput;
        #endregion
        #region Sprint
        playerActions.Sprint.started += onSprint;
        playerActions.Sprint.canceled += onSprint;
        playerActions.Sprint.performed += onSprint;
        #endregion
        #region Sneak
        playerActions.Sneak.started += onSneak;
        playerActions.Sneak.canceled += onSneak;
        #endregion
        #region Look
        playerActions.Look.started += onLookInput;
        playerActions.Look.canceled += onLookInput;
        playerActions.Look.performed += onLookInput;
        #endregion
        #endregion
    }


    private void Awake()
    {
        playerInput = new PlayerInputActions();
        characterController = GetComponent<CharacterController>();
        playerView = GetComponentInChildren<Camera>();
        playerFootstep = GetComponent<AudioSource>();

        initInputSystem(playerInput.Player);
    }

    #region Input system callbacks
    void onLookInput(InputAction.CallbackContext context)
    {
        if (!GameManager.menuOpened)
        {
            currentLookInput = context.ReadValue<Vector2>();
            upDownAngle -= currentLookInput.y * mouseSensitivity;
            upDownAngle = Mathf.Clamp(upDownAngle, -85, 90);
            playerView.transform.localRotation = Quaternion.Euler(upDownAngle, 0, 0);
            transform.rotation = transform.rotation * Quaternion.Euler(0, currentLookInput.x * mouseSensitivity, 0);
        }
    }

    void onMovementInput(InputAction.CallbackContext context)
    {
        currentMovementInput = context.ReadValue<Vector2>();
    }

    void onSprint(InputAction.CallbackContext context)
    {
        isSprintPressed = context.ReadValueAsButton();
    }

    void onSneak(InputAction.CallbackContext context)
    {
        isSneakPressed = context.ReadValueAsButton();
        if (!GameManager.menuOpened)
        {
            if (isSneakPressed)
            {
                changeStance(1, new Vector3(1, 0.5f, 1), new Vector3(transform.position.x, 0.58f, transform.position.z));
            }
            else
            {
                if (!checkIfAboveClear(transform.position))
                {
                    isSneakPressed = true;
                    checkForStandingUp = true;
                }
                else
                {
                    changeStance(2, Vector3.one, new Vector3(transform.position.x, 1.08f, transform.position.z));
                }
            }
        }
    }
    #endregion

    #region Check for above clear
    private bool checkIfAboveClear(Vector3 position)
    {
        RaycastHit hit;
        if (Physics.Raycast(transform.position, Vector3.up, out hit, 1f))
        {
            return false;
        }
        else
        {
            return true;
        }
    }
    #endregion

    #region Change stance
    void changeStance(int height, Vector3 scale, Vector3 position)
    {
        characterController.height = height;
        transform.localScale = scale;
        transform.position = position;
    }
    #endregion

    void updateCurrentMovement()
    {
        Vector3 forwardMovement = transform.forward * currentMovementInput.y;
        Vector3 sideToSideMovement = transform.right * currentMovementInput.x;
        Vector3 gravity = transform.up * 3f * -1;
        currentMovement = (forwardMovement + sideToSideMovement) * movementSpeed * Time.deltaTime + gravity;
    }


    // Update is called once per frame
    void Update()
    {
        if (!GameManager.menuOpened)
        {
            updateCurrentMovement();
            if (checkForStandingUp)
            {
                if (checkIfAboveClear(transform.position))
                {
                    changeStance(2, Vector3.one, new Vector3(transform.position.x, 1.08f, transform.position.z));
                    checkForStandingUp = false;
                    isSneakPressed = false;
                }
            }
            if (currentMovementInput.magnitude == 0)
            {
                ChangeFootstepRange(0);
            }
            else
            {
                if (isSprintPressed)
                {
                    characterController.Move(currentMovement * sprintModifier);
                    ChangeFootstepRange(sprintModifier);
                    playerFootstep.volume = 0.4f;
                    stepCooldown = 0.3f;
                }
                else if (isSneakPressed)
                {
                    characterController.Move(currentMovement * sneakModifier);
                    ChangeFootstepRange(sneakModifier);
                    playerFootstep.volume = 0.1f;
                    stepCooldown = 0.5f;

                }
                else
                {
                    characterController.Move(currentMovement);
                    ChangeFootstepRange(1);
                    playerFootstep.volume = 0.25f;
                    stepCooldown = 0.35f;
                }
                if (stepTimer <= 0)
                {
                    playerFootstep.Play();
                    stepTimer = stepCooldown;
                }
                else {
                    stepTimer -= Time.deltaTime;
                }

                
            }
        }
    }

    private void ChangeFootstepRange(float modifier)
    {
        if (!footstepsDisabled)
        {
            virtualFootStepSound.ChangeFootstepHeardRange(modifier);
        }
        else {
            virtualFootStepSound.ChangeFootstepHeardRange(0);
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

    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Cell") {
            footstepsDisabled = true;
        }
    }

    private void OnTriggerStay(Collider other)
    {
        if(other.tag == "Cell")
        {
            footstepsDisabled = true;
        }
    }
    private void OnTriggerExit(Collider other)
    {
        if(other.tag == "Cell")
        {
            footstepsDisabled = false;
        }
    }

}
