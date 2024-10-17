using Fragsurf.Movement;
using UnityEngine;

public class Collectable : MonoBehaviour
{
    [SerializeField] private AudioSource audioSource;
    [SerializeField] private SpriteRenderer sprite;
    [SerializeField] private int collectableValue;
    private void Update() {
        Billboarding();
    }
    private void Billboarding() {
        Vector3 targetDir = SurfCharacter.Instance.transform.position - transform.position;
        Vector3 rotation = Quaternion.LookRotation(targetDir).eulerAngles;
        transform.rotation = Quaternion.Euler(0, rotation.y, 0);
    }
    private void CollectableLogic() {
        audioSource.Play();
        sprite.enabled = false;
        Destroy(gameObject, 3f);
    }
    private void OnTriggerEnter(Collider other) {
        if (sprite.isVisible) {
            CollectableLogic();
            GameManager.Instance.IncreaseGemCounter(collectableValue);
        }
    }
}

