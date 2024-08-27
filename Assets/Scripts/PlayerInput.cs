using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class PlayerInput : MonoBehaviour {
    [SerializeField] private KeyCode moveFoward = KeyCode.W;
    [SerializeField] private KeyCode moveBack = KeyCode.S;
    [SerializeField] private KeyCode moveLeft = KeyCode.A;
    [SerializeField] private KeyCode moveRight = KeyCode.D;
    [SerializeField] private KeyCode jump = KeyCode.Space;
    [SerializeField] private KeyCode crouch = KeyCode.LeftControl;
    [SerializeField] private KeyCode sprintWalk = KeyCode.LeftShift;
    [SerializeField] private KeyCode movementToggle = KeyCode.T;
    private bool alwaysRun;

    public float HorizontalInputData() {

        if (Input.GetKey(moveLeft)) {
            return -1f;
        }
        if (Input.GetKey(moveRight)) {
            return 1f;
        }
        return 0f;
    }
    public float VerticalInputData() {
        if (Input.GetKey(moveFoward)) {
            return 1f;
        }
        if (Input.GetKey(moveBack)) {
            return -1f;
        }
        return 0f;
    }
    public bool IsJumping() {
        if (Input.GetKey(jump)) {
            return true;
        }
        return false;
    }
    public bool IsCrouching() {
        if (Input.GetKey(crouch)) {
            return true;
        }
        return false;
    }
    public bool IsSprinting() {
        if (Input.GetKey(sprintWalk)) {
            return true;
        }
        return false;
    }
    public bool AlwaysRun() {
        if (!alwaysRun && Input.GetKeyDown(movementToggle)) {
            alwaysRun = true;
        } else if (alwaysRun == true && Input.GetKeyDown(movementToggle)) {
            alwaysRun = false;
        }
        Debug.Log(alwaysRun);
        return alwaysRun;
    }
}
