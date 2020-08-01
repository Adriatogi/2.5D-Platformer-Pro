using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    [SerializeField]
    private bool _isGameOver = false;
    [SerializeField]
    private bool _levelComplete = false;
    [SerializeField]
    private float _fadeTime = 0.65f;

    private Player _player;

    private void Awake()
    {
        _player = GameObject.Find("Player").GetComponent<Player>();
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space) && _isGameOver)
        {
            restartGame();  //Current game scene
        }
    }

    public void gameOver()
    {
        _isGameOver = true;
    }

    public void restartGame()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }

    public void levelCompleted()
    {
        StartCoroutine(EndLevel());
    }

    private IEnumerator EndLevel()
    {
        _player.FadeCharacter();

        yield return new WaitForSeconds(_fadeTime);

        EventBroker.CallLevelComplete();
        _levelComplete = true;
        Debug.Log("Level Complete");
    }
}
