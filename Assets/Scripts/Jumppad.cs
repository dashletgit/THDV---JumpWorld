using Fragsurf.Movement;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting.FullSerializer;
using UnityEngine;

public class Jumppad : MonoBehaviour
{
    [Header("Configuration")]
    [SerializeField] private float force;
    [Header("References")]
    [SerializeField] private AudioSource audioSource;

    private void ApplyForce() {
        audioSource.Play();
        SurfCharacter.Instance.moveData.velocity.y = 0;
        SurfCharacter.Instance.moveData.velocity.y += SurfCharacter.Instance.moveConfig.jumpForce * force;
    }
    private void OnTriggerEnter(Collider other) {
       if (other.gameObject.layer == 7) {
            ApplyForce();
        }
    }
}
