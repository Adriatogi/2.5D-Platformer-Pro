using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class UIManager : MonoBehaviour
{
    [SerializeField]
    private Text _coinCountText, _livesCountText;
    [SerializeField]
    private GameObject _restartPanel;
    [SerializeField]
    private bool _isMainMenu = false;
    [SerializeField]
    private GameObject _pausePanel;
    private bool _isPaused = false;
    [SerializeField]
    private GameObject _winPanel;
    private bool _isWon = false;

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyUp(KeyCode.Escape) && (_isMainMenu == false) && (_isPaused == false) && !_isWon)
        {
            _pausePanel.SetActive(true);
            Time.timeScale = 0.0f;
            _isPaused = true;
        }
        else if (Input.GetKeyUp(KeyCode.Escape) && (_isMainMenu == false) && (_isPaused == true))
        {
            _pausePanel.SetActive(false);
            Time.timeScale = 1.0f;
            _isPaused = false;
        }
        else if (Input.GetKeyUp(KeyCode.Escape) && (_isMainMenu == true))
        {
            Application.Quit();
        }

        if (_isPaused == false)
        {
            Time.timeScale = 1.0f;
        }
    }

    public void UpdateCoinsDisplay(int coins)
    {
        _coinCountText.text = coins.ToString();
    }

    public void updateLivesDisplay(int lives)
    {
        _livesCountText.text = lives.ToString();
    }

    public void activateRestartUI()
    {
        _restartPanel.SetActive(true);
    }

    public void resumeGame()
    {
        _pausePanel.SetActive(false);
        _isPaused = false;
    }
    public void mainMenu()
    {
        SceneManager.LoadScene("Main_Menu");
        _isPaused = false;
    }

    private void gameWon()
    {
        _winPanel.SetActive(true);
        _isPaused = true;
        Time.timeScale = 0.0f;
    }

    private void OnEnable()
    {
        EventBroker.LevelComplete += gameWon; 
    }

    private void OnDisable()
    {
        EventBroker.LevelComplete -= gameWon;
    }
}
