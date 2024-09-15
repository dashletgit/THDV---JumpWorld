using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Switch : MonoBehaviour, IInteractable {

    [SerializeField] private GameObject target;
    [SerializeField] private AudioSource audioSource;
    //[SerializeField] private float actionSpeed;
    private Animator animator;
    private bool isPressed;
    private const string TRIGGER = "Increase";

    private void Awake() {
        animator = target.GetComponent<Animator>();
    }
    private void SwitchOn() {
        isPressed = true;
        HandleAudio();
        //animator.playbackTime = actionSpeed;
        animator.SetTrigger(TRIGGER);
    }
    private void HandleAudio() {
        if (!audioSource.isPlaying) {
            audioSource.Play();
        }
    }
    public void Interaction() {
        if (!isPressed) {
            SwitchOn();
        }
    }
}
