using UnityEngine;

public class WinScreenManager : MonoBehaviour
{
    [SerializeField]
    GameObject WinScreenUI;
    GameManager gameManager;

    private void Start()
    {
        gameManager = FindObjectOfType<GameManager>();
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Player") {
            gameManager.SetMenuStatus(true);
            ActivateWinScreen();
        }
    }

    private void ActivateWinScreen()
    {
        WinScreenUI.SetActive(true);
        Cursor.visible = true;
        Cursor.lockState = CursorLockMode.Confined;
    }

    public void GoToMainMenu() {
        WinScreenUI.SetActive(false);
        gameManager.SetMenuStatus(false);
        gameManager.LoadMainMenu();

    }
}
