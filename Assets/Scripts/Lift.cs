using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Lift : MonoBehaviour, IInteractable
{
    [SerializeField] private Transform lift;
    [SerializeField] private AudioSource audioSource;
    [SerializeField] private AudioClip[] clip;
    [SerializeField] private float speed;
    [SerializeField] private float secondsLowered;
    private bool isLowered;
    private bool isRaised;
    private bool coroutineRunning;
    private void Start() {
        isRaised = true;
    }
    private void Update() {
        if (isLowered) {
            LowerLift();
        }
        if (!isRaised && !isLowered) {
            RaiseLift();
        }
    }
    private void LowerLift() {
        float lowerPoint = -1.025f;
        float unitPerSecond = -0.1f;

        if (!coroutineRunning) {
            MoveLift(unitPerSecond);
            if (lift.localPosition.y < lowerPoint) {
                lift.localPosition = new Vector3(0f, lowerPoint, 0f);
                StartCoroutine(WaitBeforeRaising());
            }
        }
    }
    private void RaiseLift() {
        float upperPoint = 0f;
        float unitPerSecond = 0.1f;
        MoveLift(unitPerSecond);

        if (lift.localPosition.y > upperPoint) {
            lift.localPosition = new Vector3(0f, upperPoint, 0f);
            HandleAudio(clip[1]);
            isRaised = true;
        }
    }
    private void MoveLift(float unitPerSecond) {
        float speed = this.speed * Time.deltaTime;
        lift.localPosition += new Vector3(0f, unitPerSecond, 0f) * speed;
    }
    private void HandleAudio(AudioClip clip) {
        audioSource.clip = clip;
        if (!audioSource.isPlaying) {
            audioSource.Play();
        }
    }
    private IEnumerator WaitBeforeRaising() {
        coroutineRunning = true;
        HandleAudio(clip[1]);
        yield return new WaitForSeconds(secondsLowered);
        isLowered = false;
        HandleAudio(clip[0]);
        coroutineRunning = false;
        yield break;
    }
    public void Interaction() {
        if (isRaised) {
            isRaised = false;
            isLowered = true;
            HandleAudio(clip[0]);
        }
    }
}
