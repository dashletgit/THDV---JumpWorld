using Fragsurf.Movement;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StepOffset : MonoBehaviour {
    private void OnCollisionStay(Collision collision) {
        if (!SurfCharacter.Instance.moveData.wishJump && SurfCharacter.Instance.moveData.moving) {
            float collisionHeight = collision.gameObject.transform.localScale.y;
            SurfCharacter.Instance.transform.position += new Vector3(0f, collisionHeight, 0f);
            Debug.Log("Move Player Up");
        }
    }
}
