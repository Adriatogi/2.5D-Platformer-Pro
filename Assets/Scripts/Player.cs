using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using Cinemachine;

public class Player : MonoBehaviour
{
    private int _collectedCoins = 0;
    private UIManager _UIManager;
    private GameManager _gameManager;
    private PlayerController _playerController;
    private SpriteRenderer _spriteRenderer;
    private LevelLoader _levelLoader;
    private CinemachineVirtualCamera _CMCamera;
    private Camera _camera;
    private CinemachineBrain _vCam;

    [SerializeField]
    private int _lives = 3;
    [SerializeField]
    private Transform _respawnPoint;
    private Vector3 _respawnPointPosition;
    [SerializeField]
    private Transform _keyRespawnPoint;
    private Vector3 _keyRespawnPointPosition;
    private bool _damaged = false;

    [SerializeField]
    private float _respawnTime = 0.833f;


    private void OnEnable()
    {
        EventBroker.CoinCollected += collectedCoin;
        EventBroker.KeyCollected += ChangeSpawn;
    }

    private void OnDisable()
    {
        EventBroker.CoinCollected -= collectedCoin;
        EventBroker.KeyCollected -= ChangeSpawn;
    }
    //Called before start and update
    private void Awake()
    {
        _camera = Camera.main;
        _UIManager = GameObject.Find("Canvas").GetComponent<UIManager>();
        _gameManager = GameObject.Find("Game_Manager").GetComponent<GameManager>();
        _CMCamera = GameObject.Find("CM_Camera").GetComponent<CinemachineVirtualCamera>();
        _levelLoader = GameObject.Find("LevelLoader").GetComponent<LevelLoader>();
        _vCam = _camera.GetComponent<CinemachineBrain>();
        _playerController = GetComponent<PlayerController>();
        _spriteRenderer = GetComponent<SpriteRenderer>();

        _respawnPointPosition = _respawnPoint.position;
        _keyRespawnPointPosition = _keyRespawnPoint.position;

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
        _UIManager.UpdateCoinsDisplay(_collectedCoins);
        _UIManager.updateLivesDisplay(_lives);
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.X))
        {
            //RaycastHit2D hit = Physics2D.Raycast(_rigidbody2D.position + Vector2.up * 0.2f, lookDirection, 0.5f, LayerMask.GetMask("Default"));
            Collider2D[] hits = Physics2D.OverlapBoxAll(transform.position, _spriteRenderer.bounds.size, 0);

            foreach (Collider2D hit in hits)
            {
                //TODO Refactor so it only creates IInteractable if there is one
                Debug.Log("Hit");
                //Debug.DrawRay(transform.position, lookDirection, Color.red, 1.0f, false);

                IInteractable interactable = hit.GetComponent<IInteractable>();
                //if it isnt nul, run the interactable property of interecated 
                if (interactable != null)
                {
                    interactable.Interact();
                }
            }
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("DeadZone"))
        {
            if (!_damaged)
            {
                damage();
                _damaged = true;

            }

            StartCoroutine(FallPlayerRespawn(_vCam, _lives));

        }
        else if(collision.CompareTag("Enemy"))
        {
            if (!_damaged)
            {
                damage();
                _damaged = true;

            }

            StartCoroutine(enemyPlayerRespawn(_vCam, _lives));
        }
    }

    public void collectedCoin()
    {
        _collectedCoins++;
        _UIManager.UpdateCoinsDisplay(_collectedCoins);
    }

    private void damage()
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

    private void Respawn()
    {
        _playerController.setVelocity(0, 0);
        _playerController.setDirection(true);
    }

    public void FadeCharacter()
    {
        StartCoroutine(FadeOut());
    }

    public void ChangeSpawn()
    {
        _respawnPointPosition = _keyRespawnPointPosition;
    }

    private IEnumerator FadeOut()
    {
        Color tmp = _spriteRenderer.color;
        float _progress = 0.0f;

        while (_progress < 1)
        {
            Color _tmpColor = GetComponent<SpriteRenderer>().color;

            GetComponent<SpriteRenderer>().color = new Color(_tmpColor.r, _tmpColor.g, _tmpColor.b, Mathf.Lerp(tmp.a, 0, _progress)); //startAlpha = 0 <-- value is in tmp.a
            _progress += Time.deltaTime * 1.5f;

            yield return null;
        }
    }

    #region Respawn IEnumerators
    IEnumerator FallPlayerRespawn(CinemachineBrain vCam, int lives)
    {
        //Camera stop and continue following
        vCam.enabled = false;



        if (lives != 0)
        {
            _levelLoader.LoadTransition(_respawnTime);
            yield return new WaitForSeconds(_respawnTime);
            vCam.enabled = true;

            _damaged = false;

            //Relocate character
            _CMCamera.enabled = false;

            transform.position = _respawnPointPosition;
            Respawn();
            //yield return new WaitForSeconds(Mathf.Epsilon);
            _CMCamera.enabled = true;
            //cc.enabled = true;
            _levelLoader.EndTransition(_respawnTime);

        }
    }

    IEnumerator enemyPlayerRespawn(CinemachineBrain vCam, int lives)
    {
        //Player player = other.GetComponent<Player>();
        //SpriteRenderer _spriteRenderer = other.GetComponent<SpriteRenderer>();
        _spriteRenderer.enabled = false;
        vCam.enabled = false;

        if (lives != 0)
        {
            _levelLoader.LoadTransition(_respawnTime);
            yield return new WaitForSeconds(_respawnTime);
            vCam.enabled = true;
            _damaged = false;

            //Relocate character
            _CMCamera.enabled = false;

            transform.position = _respawnPointPosition;

            _spriteRenderer.enabled = true;
            Respawn();

            _CMCamera.enabled = true;
            _levelLoader.EndTransition(_respawnTime);
        }
    }
    #endregion
}
