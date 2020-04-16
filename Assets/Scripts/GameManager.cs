using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    [SerializeField]
    private bool _isGameOver = false;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space) && _isGameOver)
        {
            SceneManager.LoadScene("Level_1"); //Current game scene
        }
    }

    public void gameOver()
    {
        _isGameOver = true;
    }
}
