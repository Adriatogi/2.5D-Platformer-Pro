using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EventBroker : MonoBehaviour
{
    public static event Action KeyCollected;
    public static event Action LevelComplete;

    public static void CallKeyCollected()
    {
        KeyCollected?.Invoke();
    }

    public static void CallLevelComplete()
    {
        LevelComplete?.Invoke();
    }
}
