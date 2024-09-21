using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Switch : MonoBehaviour, IInteractable {

    [SerializeField] private GameObject target;
    [SerializeField] private AudioSource audioSource;
    [SerializeField] private SpriteRenderer spriteRenderer;
    [SerializeField] private Sprite[] switchTextures;
    [SerializeField] private AudioClip[] targetSounds;
    [Header("Repetable Parameters")]
    [SerializeField] private bool isRepeatable;
    [SerializeField] private float secondsToUseAgain;
    private Animator targetAnimator;
    private AudioSource targetAudioSource;
    private bool isPressed;
    private const string TRIGGER = "Action";
    private const string BOOL = "Action";
    private const int ON = 1;
    private const int OFF = 0;

    private void Awake() {
        if (target != null) {
            targetAnimator = target.GetComponent<Animator>();
            if (target.TryGetComponent(out AudioSource audioSource)) {
                targetAudioSource = audioSource;
            }
        } else { 
            Debug.LogWarning("Error: Switch is missing target or target does not contain an Animator"); 
        }
    }
    public void Interaction() {
        if (!isPressed) {
            SwitchOn();
        }
    }
    private void SwitchOn() {
        isPressed = true;
        SwitchSFX();
        spriteRenderer.sprite = switchTextures[ON];
        TargetAction();
    }
    private void TargetAction() {
        if (isRepeatable) {
            StartCoroutine(RepeatableAction());
        } else {
            StartCoroutine(SingleAction());
        }
    }
    IEnumerator RepeatableAction() {
        targetAnimator.SetBool(BOOL, true);
        yield return new WaitForSeconds(secondsToUseAgain);
        isPressed = false;
        spriteRenderer.sprite = switchTextures[OFF];
        targetAnimator.SetBool(BOOL, false);
        yield break;
    }
    IEnumerator SingleAction() {
        targetAnimator.SetTrigger(TRIGGER);
        yield return new WaitForSeconds(targetAnimator.GetCurrentAnimatorStateInfo(0).length);
        if (targetAudioSource != null) {
            targetAudioSource.clip = targetSounds[1];
            targetAudioSource.Play();
        }
        yield break;
    }
    private void SwitchSFX() {
        if (!audioSource.isPlaying) {
            audioSource.Play();
        }
    }
}
