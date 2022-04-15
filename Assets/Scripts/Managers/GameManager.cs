using System;
using System.Collections;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    PlayerInputActions inputActions;

    const int mainMenuSceneNumber = 0, uiSceneNumber = 1, testSceneNumber = 2, playGameSceneNumber = 3;

    public static bool menuOpened = false, winLossState = false;

    public static GameManager gameManager;


    void Awake()
    {
        inputActions = new PlayerInputActions();

        inputActions.Player.ToggleMenu.started += onCancel;


        SceneManager.LoadSceneAsync(uiSceneNumber, LoadSceneMode.Additive);
    }

    void onCancel(InputAction.CallbackContext context)
    {
        if (!winLossState)
        {
            menuOpened = !menuOpened;
            if (SceneManager.GetSceneByBuildIndex(mainMenuSceneNumber).isLoaded)
            {
                menuOpened = true;
            }
            else
            {
                if (menuOpened)
                {
                    if (!SceneManager.GetSceneByBuildIndex(uiSceneNumber).isLoaded) SceneManager.LoadSceneAsync(uiSceneNumber, LoadSceneMode.Additive);
                    Cursor.visible = true;
                    Cursor.lockState = CursorLockMode.Confined;
                }
                else
                {
                    CloseUi();
                }
            }
        }
    }

    public void LoadPlayScene()
    {
        if (FindObjectOfType<DroneSuspicionManager>() != null)
        {
            FindObjectOfType<DroneSuspicionManager>().ResetDroneSuspicionParams();
        }
        menuOpened = false;
        SceneManager.LoadScene(playGameSceneNumber);
        Cursor.visible = false;
        Cursor.lockState = CursorLockMode.Locked;
    }

    public void CloseUi()
    {
        menuOpened = false;
        Cursor.visible = false;
        Cursor.lockState = CursorLockMode.Locked;
        SceneManager.UnloadSceneAsync(uiSceneNumber);
    }

    public void ExitGame()
    {
        Application.Quit();
    }

    public PlayerInputActions GetInputActions()
    {
        return inputActions;
    }

    void OnEnable()
    {
        inputActions.Player.Enable();
        if (gameManager == null)
        {
            gameManager = this;
            DontDestroyOnLoad(base.gameObject);
        }
        else
        {
            Destroy(base.gameObject);
        }
    }

    void OnDisable()
    {
        inputActions.Player.Disable();
    }

    internal void LoadMainMenu()
    {
        SceneManager.LoadScene(mainMenuSceneNumber);
        Cursor.visible = true;
        Cursor.lockState = CursorLockMode.Confined;
    }

    public String GetActiveSceneName()
    {
        return SceneManager.GetActiveScene().name;
    }

    internal void SetMenuStatus(bool menuStatus)
    {
        menuOpened = menuStatus;
    }

    public void SetWinLossState(bool state) {
        winLossState = state;
    }
}
