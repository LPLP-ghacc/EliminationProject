using Assets.Scripts.Core;
using UnityEngine;
using UnityEngine.InputSystem;

namespace Assets.Scripts.Player
{
    [RequireComponent(typeof(Assets.Scripts.Player.PlayerInput))]
    public class PlayerMovement : MonoBehaviour
    {
        [Header("Player Camera Prefab/GameObject")]
        // default is false
        public bool isFPV = false;

        public GameObject TPV_playerCamera;
        public GameObject cameraPivot;
        [Space(15f)]
        public GameObject FPV_playerCamera;

        [Space]
        public PlayerGroundControl groundControl;
        public GameObject armature;

        // default 9.46
        public float moveForce = 1f;
        public ForceMode forceMode = ForceMode.VelocityChange;
        public float crouchSpeedReduce = 2f;
        // default 4.5f
        public float maxVelocity = 4.5f;
        public float maxVelocityMultiplier = 2f;
        public float sprintForce = 1f;

        private float defaultJumpForce = 200f;
        private float maxJumpForce = 300;
        public float actualJumpForce = 1f;
        public float jumpCooldown = 1f;
        private float jumpTimer = 0f;

        public float mouseSensitivity = 1f;
        private float yaw = 0.0f; 
        private float pitch = 0.0f; 
        public float pitchMin = -40f;
        public float pitchMax = 85f;
        public float modelRotationSpeed = 5.0f;

        [Header("Player Variables")]
        public bool isPause = false;
        public bool isGrounded = false;
        public bool canJump = true;
        public bool canMoveRotate = true;

        private Rigidbody rb;
        private PlayerInput playerInput;

        private void Awake()
        {
            
        }

        private void Start()
        {
            playerInput = GetComponent<PlayerInput>();
            rb = GetComponent<Rigidbody>();

            playerInput.SetCursorLocked(true);
        }

        private void FixedUpdate()
        {
            if (!isPause && canMoveRotate)
            {
                Move();
                Jump();
                ModelRotation();
            }
        }

        private void Update()
        {
            if (!isPause && canMoveRotate)
            {
                Look();        
            }

            if (!isPause)
            {
                if (Input.GetKeyUp(GlobalInputVariables.viewMode))
                {
                    ChangeCamera(isFPV);
                    isFPV = !isFPV;
                }
            }

            isGrounded = groundControl.isGrounded;
        }

        private void Move()
        {
            if (!isGrounded)
                return;

            var _maxVelocity = playerInput.sprint ? maxVelocity * maxVelocityMultiplier : maxVelocity;
            _maxVelocity = Input.GetKey(GlobalInputVariables.crouchKey) ? _maxVelocity / crouchSpeedReduce : _maxVelocity;
            var velocity = rb.velocity.magnitude < _maxVelocity ? new Vector3(playerInput.move.y, 0, playerInput.move.x) * moveForce : Vector3.zero;
            
            rb.AddRelativeForce(velocity, forceMode);
        }

        private void Jump()
        {
            actualJumpForce = rb.velocity.magnitude < 1f ? defaultJumpForce : defaultJumpForce * rb.velocity.magnitude / 2f;
            actualJumpForce = actualJumpForce > maxJumpForce ? maxJumpForce : actualJumpForce;

            if (isGrounded && playerInput.jump && canJump)
            {
                var velocity = new Vector3(0, actualJumpForce, 0);
                rb.AddRelativeForce(velocity, ForceMode.Impulse);
                canJump = false;
                jumpTimer = jumpCooldown;
            }

            if (!canJump)
            {
                jumpTimer -= Time.deltaTime;
                if (jumpTimer <= 0)
                {
                    canJump = true;
                }
            }
        }

        private void Look()
        {
            float mouseX = playerInput.look.x * mouseSensitivity;
            float mouseY = playerInput.look.y * mouseSensitivity;
            yaw += mouseX;
            pitch -= mouseY;
            pitch = Mathf.Clamp(pitch, pitchMin, pitchMax);

            cameraPivot.transform.eulerAngles = new Vector3(pitch, yaw, 0.0f);
        }

        private void ModelRotation()
        {
            if (Input.GetKey(GlobalInputVariables.freeLook))
                return;

            Vector3 targetDirection = cameraPivot.transform.forward;
            targetDirection.y = 0;

            if (targetDirection.sqrMagnitude > 0.01f)
            {
                Quaternion targetRotation = Quaternion.LookRotation(targetDirection);
                transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, Time.deltaTime * modelRotationSpeed);
            }
        }

        public void ChangeCamera(bool isFpv)
        {
            if (isFpv)
            {
                TPV_playerCamera.SetActive(false);
                FPV_playerCamera.SetActive(true);
            }
            else
            {
                TPV_playerCamera.SetActive(true);
                FPV_playerCamera.SetActive(false);
            }
        } 
    }
}
