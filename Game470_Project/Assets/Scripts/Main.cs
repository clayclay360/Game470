using System.Collections;
using System.Collections.Generic;
using UnityEditor.Experimental.RestService;
using UnityEngine;
using UnityEngine.Playables;

public class Main : MonoBehaviour
{
    public Animator backgroundAnimator;
    public PlayableDirector wakeUpDirector;

    private void Start()
    {
        backgroundAnimator.SetTrigger("FadeIn");
    }

    public void CanControlCamera(bool condition)
    {
        GameManager.canPlayer.controlCamera = condition;
    }

    public void FadeOut()
    {
        backgroundAnimator.SetTrigger("FadeOut");
        StartCoroutine(PlayDirector(1.45f));
    }

    IEnumerator PlayDirector(float time)
    {
        yield return new WaitForSeconds(time);
        FindObjectOfType<Witch>().virtualCamera.m_Priority = 9;
        wakeUpDirector.Play();
    }
}
