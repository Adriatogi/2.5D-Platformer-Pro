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
    private float transitionTime = 1f;

    public void LoadTransition()
    {
        StartCoroutine(LoadTransitionRoutine());
    }
    public void EndTransition()
    {
        StartCoroutine(EndTransitionRoutine());
    }

    IEnumerator LoadTransitionRoutine()
    {
        transition.SetBool("Start", true);

        yield return new WaitForSeconds(transitionTime);

        transition.SetBool("Start", false);
    }

    IEnumerator EndTransitionRoutine()
    {
        transition.SetBool("End", true);

        yield return new WaitForSeconds(transitionTime);

        transition.SetBool("End", false);
    }
}
