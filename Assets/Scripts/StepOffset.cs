using Fragsurf.Movement;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StepOffset : MonoBehaviour {
    private void OnCollisionEnter(Collision collision) {
       if (collision.gameObject.layer == 7 && !SurfCharacter.Instance.moveData.wishJump) {
            float collisionHeight = collision.gameObject.transform.localScale.y;
            Vector3 newPosition = new Vector3(0f, collisionHeight, 0f);
            SurfCharacter.Instance.transform.position += newPosition;
        } 
    }
}
