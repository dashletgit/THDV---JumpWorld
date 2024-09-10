using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TeleportDestination : MonoBehaviour
{
    private Vector3 objectSize = new Vector3(1f, 2f, 1f);
    [SerializeField] private AudioSource audioSource;
    private void OnDrawGizmos() {
        Gizmos.color = Color.blue;
        Gizmos.DrawWireCube(transform.position, objectSize);
        Gizmos.DrawRay(transform.position, transform.forward * 2f);
    }
    public void PlayAudio() { 
        audioSource.Play();
    }
}
