using System;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    PlayerInputActions inputActions;
    const int mainMenuSceneNumber = 0, uiSceneNumber = 1, testSceneNumber = 2;

    bool menuOpened = false;

    void Awake()
    {
        inputActions = new PlayerInputActions();

        inputActions.Player.ToggleMenu.started += onCancel;


        SceneManager.LoadSceneAsync(uiSceneNumber, LoadSceneMode.Additive);
    }

    void onCancel(InputAction.CallbackContext context)
    {
        menuOpened = !menuOpened;
        if (SceneManager.GetSceneByBuildIndex(mainMenuSceneNumber).isLoaded)
        {
            menuOpened = true;
        }
        else {
            if (menuOpened)
            {
                if(!SceneManager.GetSceneByBuildIndex(uiSceneNumber).isLoaded) SceneManager.LoadSceneAsync(uiSceneNumber, LoadSceneMode.Additive);
            }
            else {
                SceneManager.UnloadSceneAsync(uiSceneNumber);
            }
        }
    }

    public void LoadTestScene()
    {
        SceneManager.LoadSceneAsync(testSceneNumber);
    }

    public void CloseUi() {
        menuOpened = false;
        SceneManager.UnloadSceneAsync(uiSceneNumber);
    }

    public void ExitGame()
    {
        Application.Quit();
    }

    public PlayerInputActions GetInputActions() {
        return inputActions;
    }

    void OnEnable()
    {
        inputActions.Player.Enable();
        if(!SceneManager.GetSceneByName("DontDestroyOnLoad").isLoaded)DontDestroyOnLoad(this.gameObject);
    }

    void OnDisable()
    {
        inputActions.Player.Disable();
    }

    internal void LoadMainMenu()
    {
        SceneManager.LoadSceneAsync(mainMenuSceneNumber, LoadSceneMode.Single);
    }

    public String GetActiveSceneName() {
        print(SceneManager.GetActiveScene().name);
        return SceneManager.GetActiveScene().name;
    }
}
