using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FloatingText : MonoBehaviour
{
    [Header("Configuration")]
    [SerializeField] private bool allowRotation = false;
    [SerializeField] private float renderDistance = 12f;
    [Header("References")]
    [SerializeField] private TextMesh textMesh;

    private Transform mainCamera;
    private void Start() {
        mainCamera = Camera.main.transform;
    }
    private void Update() {
        if (allowRotation) {
            Billboarding();
        }
        if (Vector3.Distance(transform.position, mainCamera.transform.position) < renderDistance) {
            textMesh.gameObject.SetActive(true);
        } else {
            textMesh.gameObject.SetActive(false);
        } 
    }
    private void Billboarding() {
        transform.rotation = Quaternion.LookRotation(transform.position - mainCamera.position);
    }

}

