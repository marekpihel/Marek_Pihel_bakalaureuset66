using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    PlayerInputActions inputActions;
    int uiSceneNumber = 1;

    bool menuOpened = false;

    void Awake()
    {
        inputActions = new PlayerInputActions();

        inputActions.Player.ToggleMenu.performed += onCancel;


        SceneManager.LoadScene(uiSceneNumber, LoadSceneMode.Additive);
        DontDestroyOnLoad(this.gameObject);
    }

    void onCancel(InputAction.CallbackContext context)
    {
        if (SceneManager.GetActiveScene() == SceneManager.GetSceneByName("TestScene"))
        {
            menuOpened = !menuOpened;
            if (menuOpened)
            {
                SceneManager.LoadScene(uiSceneNumber, LoadSceneMode.Additive);
            }
            else
            {
                SceneManager.UnloadSceneAsync(uiSceneNumber);
            }
        }
    }

    public void LoadTestScene()
    {
        SceneManager.LoadScene(2);
    }

    public void ExitGame()
    {
        Application.Quit();
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
