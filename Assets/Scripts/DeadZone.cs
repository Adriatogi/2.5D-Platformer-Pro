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

            if(player != null && _damaged != true)
            {
                player.damage();
                _damaged = true;
               
            }

            //Character would move too fast for staying in new position (The Velocity)
            CharacterController cc = other.GetComponent<CharacterController>();
            //if (cc != null)
            //{
            //    cc.enabled = false;
            //}

            StartCoroutine(playerRespawn(_vCam, other));
            //StartCoroutine(CCEnableRoutine(cc));



        }
    }

    IEnumerator CCEnableRoutine(CharacterController controller)
    {
        yield return new WaitForSeconds(0.08f);
        controller.enabled = true;
    }

    IEnumerator playerRespawn(CinemachineBrain vCam, Collider other)
    {
        vCam.enabled = false;
        yield return new WaitForSeconds(2.0f);
        vCam.enabled = true;
        _damaged = false;
        other.transform.position = _respawnPoint.position;
    }
}
