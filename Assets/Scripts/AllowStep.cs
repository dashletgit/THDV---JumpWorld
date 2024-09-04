using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AllowStep : MonoBehaviour
{
    private void OnCollisionEnter(Collision collision) {
        if (collision.gameObject.layer == 7) {
            Debug.Log("Collision With Player Detected");
        }
    }
}
