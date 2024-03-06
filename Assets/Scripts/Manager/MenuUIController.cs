using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class MenuUIController : MonoBehaviour
{
    [SerializeField] private GameObject _settingsScreen;
    [SerializeField] private GameObject _aboutScreen;
    [SerializeField] private GameObject _exitScreen;

    [SerializeField] private Button _startButton;
    [SerializeField] private Button _settingsButton;
    [SerializeField] private Button _aboutButton;
    [SerializeField] private Button _exitButton;

    private int _gameSceneID = 1;
    
    void Start()
    {
        this.gameObject.SetActive(true);
        _settingsScreen.SetActive(false);
        _exitScreen.SetActive(false);
        _aboutScreen.SetActive(false);
        
        
        _startButton.onClick.AddListener(LoadGameScene);
        _settingsButton.onClick.AddListener(SettingsButtonOnClick);
        _aboutButton.onClick.AddListener(AboutButtonOnClick);
        _exitButton.onClick.AddListener(ExitButtonOnClick);
        
        AudioManager.Instance.Play(AudioEnum.DNB);
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void LoadGameScene()
    {
        SceneManager.LoadScene(_gameSceneID);
    }

    public void BackToMainMenuButton()
    {
        _settingsScreen.SetActive(false);
        _exitScreen.SetActive(false);
        _aboutScreen.SetActive(false);
            
        this.gameObject.SetActive(true);
    }

    public void AboutButtonOnClick()
    {
        this.gameObject.SetActive(false);
        _aboutScreen.SetActive(true);
    }

    public void SettingsButtonOnClick()
    {
        this.gameObject.SetActive(false);
        _settingsScreen.SetActive(true);
    }

    #region Exit

    public void ExitButtonOnClick()
    {
        this.gameObject.SetActive(false);
        _exitScreen.SetActive(true);
    }

    public void ExitTheGame()
    {
        Application.Quit();
    }

    #endregion
    
    
    
}
