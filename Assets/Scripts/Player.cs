using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Player : MonoBehaviour
{
    private int _collectedCoins = 0;
    private UIManager _UIManager;
    private GameManager _gameManager;
    private PlayerController _playerController;

    [SerializeField]
    private int _lives = 3;

    //Called before start and update
    private void Awake()
    {
        _UIManager = GameObject.Find("Canvas").GetComponent<UIManager>();
        _gameManager = GameObject.Find("Game_Manager").GetComponent<GameManager>();
        _playerController = GetComponent<PlayerController>();

        if (_UIManager == null)
        {
            Debug.LogError("UIManager is null");
        }
        if (_gameManager == null)
        {
            Debug.LogError("GameManager is null");
        }
        if(_playerController == null)
        {
            Debug.LogError("PlayerController is null");
        }
    }
    // Start is called before the first frame update
    void Start()
    {
        _UIManager.updateCoinsDisplay(_collectedCoins);
        _UIManager.updateLivesDisplay(_lives);
    }
    
    public void collectedCoin()
    {
        _collectedCoins++;
        _UIManager.updateCoinsDisplay(_collectedCoins);
    }

    public void damage()
    {
        _lives--;
        //Detach in case you die from moving platform
        transform.parent = null;
        _UIManager.updateLivesDisplay(_lives);
        if (_lives == 0)
        {
            _UIManager.activateRestartUI();
            _gameManager.gameOver();
        }
    }

    public int getLives()
    {
        return _lives;
    }

    public void Respawn()
    {
        _playerController.setVelocity(0, 0);
        _playerController.setDirection(1);
    }
}
