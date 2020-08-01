using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Key : MonoBehaviour, ICollectable
{
   public void Collected()
    {
        EventBroker.CallKeyCollected();
    }

}
