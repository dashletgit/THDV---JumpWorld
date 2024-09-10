using Fragsurf.Movement;
using UnityEngine;

public class StepOffset : MonoBehaviour {

    [SerializeField] private float stepHeight = 0.5f;
    private Vector3 prevVel;
    private bool allowStepOffset;
    private SurfCharacter player;

    private void Start() {
        player = SurfCharacter.Instance;
    }
    private void OnCollisionEnter(Collision collision) {
        prevVel = player.moveData.velocity;
        if (player.moveData.grounded && player.moveData.moving && !player.moveData.jumping) {
            if (collision.gameObject.layer == 3) {
                allowStepOffset = true;
            }
        }
    }
    private void OnCollisionStay(Collision collision) {
       if (allowStepOffset) {
           player.moveData.origin.y += stepHeight;
           player.moveData.velocity = prevVel;
           allowStepOffset = false;
                
       }
    }
}
