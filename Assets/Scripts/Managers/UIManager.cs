using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIManager : MonoBehaviour
{
    GameManager gameManager;

    [SerializeField]
    List<String> viewNames;
    [SerializeField]
    List<GameObject> viewGameObjects;

    String lastUiView;
    Dictionary<String, GameObject> views = new Dictionary<string, GameObject>();


    Dropdown resolutionDropdown;
    Slider volumeSlider;


    private void Start()
    {
        gameManager = FindObjectOfType<GameManager>();
        LoadLastUIView();
        resolutionDropdown = FindObjectOfType<Dropdown>();
        volumeSlider = FindObjectOfType<Slider>();
        LoadViews();
        DeactivateAllUIViews();
        views[lastUiView].SetActive(true);
    }

    private void LoadLastUIView()
    {
        if (gameManager.GetActiveSceneName() == "StartGameScene")
        {
            lastUiView = "mainMenu";
        }
        else
        {
            lastUiView = "pause";
        }
    }

    private void LoadViews()
    {
        for (int viewIndex = 0; viewIndex < viewGameObjects.Count; viewIndex++)
        {
            views.Add(viewNames[viewIndex], viewGameObjects[viewIndex]);
        }
    }

    private void DeactivateAllUIViews()
    {
        foreach (String key in views.Keys)
        {
            views[key].SetActive(false);
        }
    }

    public void LoadTestScene()
    {
        lastUiView = "pause";
        gameManager.LoadPlayScene();
    }

    public void ExitGame()
    {
        gameManager.ExitGame();
    }

    public void OpenSettingsMenu()
    {
        lastUiView = FindCurrentlyActiveUI();
        DeactivateAllUIViews();
        views["settings"].SetActive(true);
        SetupSettings();
    }

    private String FindCurrentlyActiveUI()
    {
        foreach (String key in views.Keys)
        {
            if (views[key].activeSelf)
            {
                return key;
            }
        }
        return null;
    }

    public void OpenLastMenu()
    {
        DeactivateAllUIViews();
        views[lastUiView].SetActive(true);
    }

    public void OpenPauseView()
    {
        DeactivateAllUIViews();
        views["pause"].SetActive(true);
    }

    public void SetupSettings()
    {
        resolutionDropdown.captionText.text = Screen.currentResolution.ToString();
        resolutionDropdown.ClearOptions();
        List<string> resolutions = new List<string>();
        resolutions.Add(Screen.currentResolution.ToString());
        foreach (Resolution resolution in Screen.resolutions)
        {
            if (resolution.Equals(Screen.currentResolution)) continue;

            string resolutionString = resolution.ToString().Split('@')[0].Trim();
            if (!resolutions.Contains(resolutionString)) resolutions.Add(resolutionString);
        }
        resolutionDropdown.AddOptions(resolutions);
        volumeSlider.value = PlayerPrefs.GetFloat("Volume");
    }

    public void ChangeResolution(Text resolution)
    {
        int width = int.Parse(resolution.text.Split(Char.Parse(" "))[0]);
        int height = int.Parse(resolution.text.Split(Char.Parse(" "))[2]);
        Screen.SetResolution(width, height, FullScreenMode.ExclusiveFullScreen);
    }

    public void ChangeVolume(Slider volumeSlider)
    {
        PlayerPrefs.SetFloat("Volume", volumeSlider.value);
        AudioListener.volume = volumeSlider.value;
    }

    public void CloseUI()
    {
        gameManager.CloseUi();
        Cursor.visible = false;
        Cursor.lockState = CursorLockMode.Locked;
    }

    public void LoadMainMenu()
    {
        CloseUI();
        gameManager.LoadMainMenu();
    }
}
