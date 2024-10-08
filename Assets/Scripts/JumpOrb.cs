using Fragsurf.Movement;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class JumpOrb : MonoBehaviour
{
    [Header("Configuration")]
    [SerializeField] private float respawnSeconds = 3f;
    [SerializeField] private float jumpForceMultiplier = 1.2f;
    [Header("References")]
    [SerializeField] private SpriteRenderer sprite;
    [SerializeField] private AudioSource audioSource;
    [SerializeField] private AudioClip[] audioClips;
    private bool exploded;
    private const int EXPLODE = 0;
    private const int RESPAWN = 1;
    private void Update() {
        Billboarding();
    }
    private void Billboarding() {
        Vector3 targetDir = SurfCharacter.Instance.transform.position - transform.position;
        Vector3 rotation = Quaternion.LookRotation(targetDir).eulerAngles;
        transform.rotation = Quaternion.Euler(0, rotation.y, 0);
    }
    private void Explode() { 
        exploded = true;
        sprite.enabled = false;
        SurfCharacter.Instance.moveData.velocity.y = 0;
        SurfCharacter.Instance.moveData.velocity.y += SurfCharacter.Instance.moveConfig.jumpForce * jumpForceMultiplier;
        StartCoroutine(Respawn());
    }
    private IEnumerator Respawn() {
        audioSource.clip = audioClips[EXPLODE];
        audioSource.Play();
        yield return new WaitForSeconds(respawnSeconds);
        sprite.enabled = true;
        exploded = false;
        audioSource.clip = audioClips[RESPAWN];
        audioSource.Play();
        yield break;
    }
    private void OnTriggerEnter(Collider other) {
       if (other.gameObject.layer == 7) {
            if (!exploded) {
                Explode();
            }
        }
    }

}
