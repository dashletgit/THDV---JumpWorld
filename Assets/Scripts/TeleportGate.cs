using Fragsurf.Movement;
using System.Collections;
using UnityEngine;

public class TeleportGate : MonoBehaviour
{
    private SurfCharacter player;
    private Transform destination;
    [SerializeField] private TeleportDestination teleportDestination;

    private void Start() {
        player = SurfCharacter.Instance;
        if (teleportDestination == null) {
            Debug.LogError("Teleporter Destination Not Assigned!");
        } else {
            destination = teleportDestination.transform;
        }
    }
    private void TeleportLogic() {
        Vector3 destinationPos = destination.transform.position;
        player.transform.position = destinationPos;
        player.moveData.velocity = Vector3.zero;
        SetPlayerRotation();
        StartCoroutine(TemporaryDisableInput());
        StopCoroutine(TemporaryDisableInput());
    }
    private void SetPlayerRotation() {
        player.transform.rotation = Quaternion.Euler(0f, 0f, 0f);
        player.transform.Rotate(destination.transform.rotation.eulerAngles);
    }
    IEnumerator TemporaryDisableInput() {
        player.DisableInput();
        teleportDestination.PlayAudio();
        yield return new WaitForSeconds(0.5f);
        player.EnableInput();
        yield return null;
    }
    private void OnTriggerEnter(Collider other) {
        if (other.gameObject.layer == 7) {
            TeleportLogic();
        }
    }

}
