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

        inputActions.Player.ToggleMenu.performed += onCancel;


        SceneManager.LoadSceneAsync(uiSceneNumber, LoadSceneMode.Additive);
        DontDestroyOnLoad(this.gameObject);
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
                SceneManager.LoadSceneAsync(uiSceneNumber, LoadSceneMode.Additive);
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

    public void ExitGame()
    {
        Application.Quit();
    }

    public void OpenUI() {
        int i = 1 + 1;
    }

    void OnEnable()
    {
        inputActions.Player.Enable();
    }

    void OnDisable()
    {
        inputActions.Player.Disable();
    }


}
