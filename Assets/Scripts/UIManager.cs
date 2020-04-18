using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIManager : MonoBehaviour
{
    [SerializeField]
    private Text _coinCountText, _livesCountText;
    [SerializeField]
    private GameObject _restartPanel;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void updateCoinsDisplay(int coins)
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
}
