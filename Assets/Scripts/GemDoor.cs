using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[SelectionBase]
public class GemDoor : MonoBehaviour {
    [Header("Configuration")]
    [SerializeField] private int gemsNeeded = 1;
    [Header("References")]
    [SerializeField] private GameObject doorModel;
    [SerializeField] private TextMesh textValue;
    [SerializeField] private AudioSource audioSource;
    private bool unlocked;

    private void Start() {
        textValue.text = gemsNeeded.ToString();
    }
    private void Unlock() { 
        unlocked = true;
        doorModel.SetActive(false);
        audioSource.Play();
        Destroy(gameObject, 5f);
    }
    private void OnTriggerEnter(Collider other) {
        if (other.gameObject.layer == 7 && !unlocked) {
            int currentGems = GameManager.Instance.GetGemCounter();
            if (currentGems >= gemsNeeded) {
                Unlock();
            }
        }
    }
}
