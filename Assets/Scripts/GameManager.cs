using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    [SerializeField]
    private bool _isGameOver = false;
    [SerializeField]
    private bool _isMainMenu = false;
    [SerializeField]
    private GameObject _pausePanel;
    private bool _isPaused = false;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space) && _isGameOver)
        {
            restartGame();  //Current game scene
        }

        if (Input.GetKeyUp(KeyCode.Escape) && (_isMainMenu == false) && (_isPaused == false))
        {
            _pausePanel.SetActive(true);
            Time.timeScale = 0.0f;
            _isPaused = true;
        }
        else if(Input.GetKeyUp(KeyCode.Escape) && (_isMainMenu == false) && (_isPaused == true))
        {
            _pausePanel.SetActive(false);
            Time.timeScale = 1.0f;
            _isPaused = false;
        }
        else if (Input.GetKeyUp(KeyCode.Escape) && (_isMainMenu == true))
        {
            Application.Quit();
        }

        if(_isPaused == false)
        {
            Time.timeScale = 1.0f;
        }

    }

    public void gameOver()
    {
        _isGameOver = true;
    }

    public void resumeGame()
    {
        _pausePanel.SetActive(false);
        _isPaused = false;
    }

    public void restartGame()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }

    public void mainMenu()
    {
        SceneManager.LoadScene("Main_Menu");
        _isPaused = false;
    }

}
