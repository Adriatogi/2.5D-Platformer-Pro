using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LevelLoader : MonoBehaviour
{
    [SerializeField]
    private Animator transition;
    [SerializeField]
    //private float transitionTime = 1f;

    public void LoadTransition(float transitionTime)
    {
        StartCoroutine(LoadTransitionRoutine(transitionTime));
    }
    public void EndTransition(float transitionTime)
        {
        StartCoroutine(EndTransitionRoutine(transitionTime));
    }

    IEnumerator LoadTransitionRoutine(float transitionTime)
    {
        transition.SetBool("Start", true);

        yield return new WaitForSeconds(transitionTime);

        transition.SetBool("Start", false);
    }

    IEnumerator EndTransitionRoutine(float transitionTime)
    {
        yield return new WaitForSeconds(transitionTime);

        transition.SetBool("End", true);
        yield return new WaitForSeconds(0.1f);
        transition.SetBool("End", false);
    }
}
