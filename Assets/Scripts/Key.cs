using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Key : MonoBehaviour
{
    public delegate void KeyAction();
    public static event KeyAction OnCollected;
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.tag == "Player")
        {
            if(OnCollected != null)
            {
                OnCollected.Invoke();
            }
            OnCollected?.Invoke();
        }
    }
}
