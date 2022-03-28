using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIManager : MonoBehaviour
{
    GameManager gameManager;

    private void Awake()
    {
        gameManager = FindObjectOfType<GameManager>();
    }

    public void LoadTestScene() {
        gameManager.LoadTestScene();
    }

    public void ExitGame() {
        gameManager.ExitGame();
    }
}
