using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;

public class DeadZone : MonoBehaviour
{
    [SerializeField]
    private Transform _respawnPoint;
    private GameObject _camera;
    private bool _damaged = false;
    private bool _isEnemyDeadZone = false;

    private CinemachineVirtualCamera _CMCamera;

    // Start is called before the first frame update
    void Start()
    {
        _respawnPoint = GameObject.Find("Respawn_Point").transform;
        _CMCamera = GameObject.Find("CM_Camera").GetComponent<CinemachineVirtualCamera>();
        _camera = GameObject.Find("Main_Camera");

    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.tag == "Player")
        {
            Player player = other.GetComponent<Player>();
            CinemachineBrain _vCam= _camera.GetComponent<CinemachineBrain>(); 

            if (player != null && _damaged != true)
            {
                player.damage();
                _damaged = true;
               
            }

            int playerLives = player.getLives();
            if (_isEnemyDeadZone == false)
            {
                StartCoroutine(fallPlayerRespawn(_vCam, other, playerLives));
            }
            else
            {
                StartCoroutine(enemyPlayerRespawn(_vCam, other, playerLives));
            }
        }
    }

    IEnumerator fallPlayerRespawn(CinemachineBrain vCam, Collider2D other, int lives)
    {
        //Camera stop and continue following
        vCam.enabled = false;
        Player player = other.GetComponent<Player>();
        yield return new WaitForSeconds(1.5f);

        if (lives != 0)
        {
            vCam.enabled = true;

            _damaged = false;

            //Relocate character
            _CMCamera.enabled = false;

            other.transform.position = _respawnPoint.position;
            player.Respawn();
            //yield return new WaitForSeconds(Mathf.Epsilon);
            _CMCamera.enabled = true;
            //cc.enabled = true;

        }
    }

    IEnumerator enemyPlayerRespawn(CinemachineBrain vCam, Collider2D other, int lives)
    {
        Player player = other.GetComponent<Player>();
        SpriteRenderer _spriteRenderer = other.GetComponent<SpriteRenderer>();
        _spriteRenderer.enabled = false;
        vCam.enabled = false;
        yield return new WaitForSeconds(1.5f);

        if (lives != 0)
        {
            vCam.enabled = true;
            _damaged = false;

            //Relocate character
            _CMCamera.enabled = false;

            other.transform.position = _respawnPoint.position;

            _spriteRenderer.enabled = true;
            player.Respawn();

            _CMCamera.enabled = true;
        }
    }

    public void _enemyDeadZone()
    {
        _isEnemyDeadZone = true;
    }
}
