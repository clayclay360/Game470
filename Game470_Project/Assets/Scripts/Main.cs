using System.Collections;
using UnityEngine;
using UnityEngine.Playables;

public class Main : MonoBehaviour
{
    [Header("CutScenes")]
    public Animator backgroundAnimator;
    public PlayableDirector wakeUpDirector;
    public PlayableDirector endDirector;

    [Header("Witch")]
    public GameObject witch;

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
        RestartGame();
        wakeUpDirector.Play();
    }

    public void RestartGame()
    {
        GameManager.playerCaptured = false; // player is not captured
        FindObjectOfType<PlayerController>().isCaptured = false; // player is free
        FindObjectOfType<CameraController>().ResetValue(); // reset camera
        FindObjectOfType<Witch>().ResetVariables();
        FindObjectOfType<Witch>().gameObject.SetActive(false);
    }

    public void WitchDefeated()
    {
        endDirector.Play();
        FindObjectOfType<Witch>().gameObject.SetActive(false);
    }
}
