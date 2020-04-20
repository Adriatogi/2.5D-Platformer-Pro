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



    // Start is called before the first frame update
    void Start()
    {
        _respawnPoint = GameObject.Find("Respawn_Point").transform;
        _camera = GameObject.Find("Main_Camera");

    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Player")
        {
            Player player = other.GetComponent<Player>();
            CinemachineBrain _vCam= _camera.GetComponent<CinemachineBrain>();

            //Character would move too fast for staying in new position (The Velocity)
            CharacterController cc = other.GetComponent<CharacterController>();

            if (player != null && _damaged != true)
            {
                player.damage();
                _damaged = true;
               
            }

            int playerLives = player.getLives();
            if (_isEnemyDeadZone == false)
            {
                StartCoroutine(fallPlayerRespawn(_vCam, other, cc, playerLives));
            }
            else
            {
                StartCoroutine(enemyPlayerRespawn(_vCam, other, playerLives));
            }
        }
    }

    IEnumerator fallPlayerRespawn(CinemachineBrain vCam, Collider other, CharacterController cc, int lives)
    {
        //Camera stop and continue following
        vCam.enabled = false;
        Player player = other.GetComponent<Player>();
        yield return new WaitForSeconds(1.5f);

        if (lives != 0)
        {
            vCam.enabled = true;

            _damaged = false;
            //Disable to reset speed
            if (cc != null)
            {
                cc.enabled = false;
            }

            //Relocate character
            other.transform.position = _respawnPoint.position;
            player.respawn();
            yield return new WaitForSeconds(0.08f);
            cc.enabled = true;

        }
    }

    IEnumerator enemyPlayerRespawn(CinemachineBrain vCam, Collider other, int lives)
    {
        Player player = other.GetComponent<Player>();
        other.gameObject.SetActive(false);
        vCam.enabled = false;
        yield return new WaitForSeconds(1.5f);

        if (lives != 0)
        {
            vCam.enabled = true;
            _damaged = false;
            other.transform.position = _respawnPoint.position;
            other.gameObject.SetActive(true);
            player.respawn();
        }
    }

    public void _enemyDeadZone()
    {
        _isEnemyDeadZone = true;
    }
}
