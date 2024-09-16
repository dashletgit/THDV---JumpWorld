using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.Rendering;

namespace Fragsurf.Movement {

    [AddComponentMenu ("Fragsurf/Surf Character")]
    public class SurfCharacter : MonoBehaviour, ISurfControllable {

        public enum ColliderType {
            Box
        }

        ///// Fields /////
        [SerializeField] private PlayerInput playerInput;
        [SerializeField] private AudioSource audioSource;

        private GameObject _groundObject;
        private Vector3 _baseVelocity;
        private Collider _collider;
        private Vector3 _angles;
        private GameObject _colliderObject;
        private IInteractable interactable;

        private MoveData _moveData = new MoveData();
        private SurfController _controller = new SurfController();

        private Rigidbody rb;

        private List<Collider> triggers = new List<Collider>();
        private int numberOfTriggers = 0;

        //private bool underwater;
        private bool audioPlaying;
        private bool bigFall;
        private bool inputDisable;

        [Header("Physics Settings")]
        public Vector3 colliderSize = new Vector3 (1f, 2f, 1f);
        public float weight = 75f;
        public float rigidbodyPushForce = 2f;
        public bool solidCollider = false;
        [HideInInspector] public ColliderType collisionType { get { return ColliderType.Box; } }

        [Header("View Settings")]
        public Transform viewTransform;
        public Transform playerRotationTransform;
        [SerializeField] private PlayerAiming playerCamera;

        [Header ("Crouching Setup")]
        public float crouchingHeightMultiplier = 0.5f;
        public float crouchingSpeed = 10f;
        private float defaultHeight;
        private bool allowCrouch = true;

        [Header ("Features")]
        public bool crouchingEnabled = false;
        public bool alwaysRun = false;
        private bool slidingEnabled = false;
        private bool laddersEnabled = false;
        private bool supportAngledLadders = false;

        [Header("Audio")]
        [SerializeField] private AudioClip jumpAudio;
        [SerializeField] private AudioClip fallAudio;

        //Step Offset (Buggy)
        private bool useStepOffset = false;
        private float stepOffset = 0.35f;

        [Header ("Movement Config")]
        [SerializeField] public MovementConfig movementConfig;

        ///// Properties /////

        public static SurfCharacter Instance { get; private set; }
        public MoveType moveType { get { return MoveType.Walk; } }
        public MovementConfig moveConfig { get { return movementConfig; } }
        public MoveData moveData { get { return _moveData; } }
        public new Collider collider { get { return _collider; } }

        public GameObject groundObject {

            get { return _groundObject; }
            set { _groundObject = value; }

        }

        public Vector3 baseVelocity { get { return _baseVelocity; } }

        public Vector3 forward { get { return viewTransform.forward; } }
        public Vector3 right { get { return viewTransform.right; } }
        public Vector3 up { get { return viewTransform.up; } }

        Vector3 prevPosition;

        ///// Methods /////

		private void OnDrawGizmos()
		{
			Gizmos.color = Color.red;
			Gizmos.DrawWireCube( transform.position, colliderSize );
		}
		
        private void Awake () {

            Instance = this;    
            _controller.playerTransform = playerRotationTransform;
            
            if (viewTransform != null) {

                _controller.camera = viewTransform;
                _controller.cameraYPos = viewTransform.localPosition.y;

            }
        }
        private void Start () {
            
            _colliderObject = new GameObject ("PlayerCollider");
            _colliderObject.layer = gameObject.layer;
            _colliderObject.transform.SetParent (transform);
            _colliderObject.transform.rotation = Quaternion.identity;
            _colliderObject.transform.localPosition = Vector3.zero;
            _colliderObject.transform.SetSiblingIndex (0);

            prevPosition = transform.position;

            if (viewTransform == null)
                viewTransform = Camera.main.transform;

            if (playerRotationTransform == null && transform.childCount > 0)
                playerRotationTransform = transform.GetChild (0);

            _collider = gameObject.GetComponent<Collider> ();

            if (_collider != null)
                GameObject.Destroy (_collider);

            // rigidbody is required to collide with triggers
            rb = gameObject.GetComponent<Rigidbody> ();
            if (rb == null)
                rb = gameObject.AddComponent<Rigidbody> ();

            allowCrouch = crouchingEnabled;

            rb.isKinematic = true;
            rb.useGravity = false;
            rb.angularDrag = 0f;
            rb.drag = 0f;
            rb.mass = weight;


            switch (collisionType) {

                // Box collider
                case ColliderType.Box:

                _collider = _colliderObject.AddComponent<BoxCollider> ();

                var boxc = (BoxCollider)_collider;
                boxc.size = colliderSize;

                defaultHeight = boxc.size.y;

                break;
            }
            _moveData.slopeLimit = movementConfig.slopeLimit;

            _moveData.rigidbodyPushForce = rigidbodyPushForce;

            _moveData.slidingEnabled = slidingEnabled;
            _moveData.laddersEnabled = laddersEnabled;
            _moveData.angledLaddersEnabled = supportAngledLadders;

            _moveData.playerTransform = transform;
            _moveData.viewTransform = viewTransform;
            _moveData.viewTransformDefaultLocalPos = viewTransform.localPosition;

            _moveData.defaultHeight = defaultHeight;
            _moveData.crouchingHeight = crouchingHeightMultiplier;
            _moveData.crouchingSpeed = crouchingSpeed;
            
            _collider.isTrigger = !solidCollider;
            _moveData.origin = transform.position;

            _moveData.useStepOffset = useStepOffset;
            _moveData.stepOffset = stepOffset;
        }

        private void Update() {
            //Debug.Log("Speed: " + _moveData.speed);
            //Debug.Log("VelocityY: " + _moveData.velocity.y);
            //Debug.Log("Grounded: " + _moveData.grounded);

            UpdateMoveData();

            // Previous movement code
            Vector3 positionalMovement = transform.position - prevPosition;
            transform.position = prevPosition;
            moveData.origin += positionalMovement;

            // Triggers
            if (numberOfTriggers != triggers.Count) {
                numberOfTriggers = triggers.Count;

                triggers.RemoveAll (item => item == null);
                foreach (Collider trigger in triggers) {

                    if (trigger == null)
                        continue;

                    if (trigger.TryGetComponent(out IInteractable interactable)) { 
                        this.interactable = interactable;
                    }
                }
            }

            if (allowCrouch) {
                _controller.Crouch(this, movementConfig, Time.deltaTime);
            }

            _controller.ProcessMovement (this, movementConfig, Time.deltaTime);
            HandleAudio();
            transform.position = moveData.origin;
            prevPosition = transform.position;
            _colliderObject.transform.rotation = Quaternion.identity;
        }
       
        private void UpdateMoveData() {
            _moveData.viewAngles = _angles;
            _moveData.verticalAxis = playerInput.VerticalInputData();
            _moveData.horizontalAxis = playerInput.HorizontalInputData();
            _moveData.sprinting = playerInput.IsSprinting();
            HandleInteractions();
            playerCamera.FreeLookDisable(moveConfig.freeLook);

            if (playerInput.IsCrouching())
                _moveData.crouching = true;
            if (!playerInput.IsCrouching())
                _moveData.crouching = false;

            if (playerInput.IsJumping())
                _moveData.wishJump = true;
            if (!playerInput.IsJumping())
                _moveData.wishJump = false;

            if (playerInput.AlwaysRun()) { 
                alwaysRun = true;
            }
            else { alwaysRun = false; }

            if (alwaysRun ==  true) {
                _moveData.sprinting = true;
                if (playerInput.IsSprinting()) {
                    _moveData.sprinting = false;
                }
            }
            if (_moveData.sideMove == 0f && _moveData.forwardMove == 0f) {
                _moveData.moving = false;
            } 
            else { _moveData.moving = true; }

            if (_moveData.velocity.y <= -12f) {
                bigFall = true;
            }

            if (_groundObject != null) {
                _moveData.grounded = true;
            } 
            else { _moveData.grounded = false; }

            bool moveLeft = _moveData.horizontalAxis < 0f;
            bool moveRight = _moveData.horizontalAxis > 0f;
            bool moveFwd = _moveData.verticalAxis > 0f;
            bool moveBack = _moveData.verticalAxis < 0f;

            if (!moveLeft && !moveRight)
                _moveData.sideMove = 0f;
            else if (moveLeft)
                _moveData.sideMove = -moveConfig.acceleration;
            else if (moveRight)
                _moveData.sideMove = moveConfig.acceleration;

            if (!moveFwd && !moveBack)
                _moveData.forwardMove = 0f;
            else if (moveFwd)
                _moveData.forwardMove = moveConfig.acceleration;
            else if (moveBack)
                _moveData.forwardMove = -moveConfig.acceleration;

            if (inputDisable) {

                _moveData.verticalAxis = 0f;
                _moveData.horizontalAxis = 0f;
                _moveData.sideMove = 0f;
                _moveData.forwardMove = 0f;
                _moveData.wishJump = false;
            }
        }
        private void HandleInteractions() {
            if (playerInput.Interacted() && interactable != null) {
                interactable.Interaction();
            }
        }
        public void DisableInput() {
            inputDisable = true;
            playerCamera.DisableMouseInput();
        }
        public void EnableInput() {
            inputDisable = false;
            playerCamera.EnableMouseInput();
        }
        private void HandleAudio() {
            if (!_moveData.jumping) {
                audioPlaying = false;
            } 
            if (_moveData.jumping) {
                PlayAudioOnce(jumpAudio);
            }
            if (_moveData.grounded && bigFall && !_moveData.wishJump) {
                PlayAudioOnce(fallAudio);
                bigFall = false;
            }
        }
        private void PlayAudioOnce(AudioClip clip) {
            audioSource.clip = clip;

            if (!audioPlaying) {
                audioSource.Play();
                audioPlaying = true;
            }
        }

        private void OnTriggerEnter (Collider other) {
            
            if (!triggers.Contains (other))
                triggers.Add (other);

        }

        private void OnTriggerExit (Collider other) {

            if (triggers.Contains(other)) {
                triggers.Remove(other);
                interactable = null;
            }

        }

        private void OnCollisionStay (Collision collision) {

            if (collision.rigidbody == null)
                return;

            Vector3 relativeVelocity = collision.relativeVelocity * collision.rigidbody.mass / 50f;
            Vector3 impactVelocity = new Vector3 (relativeVelocity.x * 0.0025f, relativeVelocity.y * 0.00025f, relativeVelocity.z * 0.0025f);

            float maxYVel = Mathf.Max (moveData.velocity.y, 10f);
            Vector3 newVelocity = new Vector3 (moveData.velocity.x + impactVelocity.x, Mathf.Clamp (moveData.velocity.y + Mathf.Clamp (impactVelocity.y, -0.5f, 0.5f), -maxYVel, maxYVel), moveData.velocity.z + impactVelocity.z);

            newVelocity = Vector3.ClampMagnitude (newVelocity, Mathf.Max (moveData.velocity.magnitude, 30f));
            moveData.velocity = newVelocity;

        }
    }
}

