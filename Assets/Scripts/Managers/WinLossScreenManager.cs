using UnityEngine;
using UnityEngine.UI;

public class WinLossScreenManager : MonoBehaviour
{
    [SerializeField]
    GameObject WinLossScreenUI;
    [SerializeField]
    Image WinScreenUI;
    [SerializeField]
    Image LossScreenUI;
    GameManager gameManager;

    private void Awake()
    {
        WinLossScreenUI.SetActive(false);
    }

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
        ActiveUIViewWithName("WinScreenView");
    }
    public void ActivateLossScreen()
    {
        ActiveUIViewWithName("LostScreenView");
    }

    private void ActiveUIViewWithName(string name)
    {
        WinLossScreenUI.SetActive(true);
        if (name == "WinScreenView")
        {
            WinScreenUI.gameObject.SetActive(true);
        }
        else if (name == "LostScreenView") {
            LossScreenUI.gameObject.SetActive(true);
        }
        gameManager.SetMenuStatus(true);
        gameManager.SetWinLossState(true);
        Cursor.visible = true;
        Cursor.lockState = CursorLockMode.Confined;
    }

    public void GoToMainMenu() {
        WinLossScreenUI.SetActive(false);
        gameManager.SetMenuStatus(false);
        gameManager.SetWinLossState(false);
        gameManager.LoadMainMenu();

    }
}
