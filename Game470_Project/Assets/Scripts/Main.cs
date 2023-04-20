using System.Collections;
using System.Collections.Generic;
using UnityEditor.Experimental.RestService;
using UnityEngine;
using UnityEngine.Playables;

public class Main : MonoBehaviour
{
    public Animator backgroundAnimator;
    public PlayableDirector wakeUpDirector;
    public PlayableDirector endDirector;

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
        RestartGame();
        FindObjectOfType<Witch>().virtualCamera.m_Priority = 9;
        wakeUpDirector.Play();
    }

    public void RestartGame()
    {
        GameManager.playerCaptured = false; // player is not captured
        FindObjectOfType<PlayerController>().isCaptured = false; // player is free
        FindObjectOfType<CameraController>().ResetValue(); // reset camera
        FindObjectOfType<Witch>().ResetVariables();
    }

    public void WitchDefeated()
    {
        endDirector.Play();
    }
}
