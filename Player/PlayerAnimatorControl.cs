using Assets.Scripts.Core;
using Assets.Scripts.Player;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAnimatorControl : MonoBehaviour
{
    public Animator animator;
    public Transform weaponSpawnPos;
    [SerializeField] private Camera cameraToLookFrom;
    public bool isEnableFollowHead = true;
    public Transform headTarget;
    private float previousLookX = 0f;
    public float smoothingSpeed = 2f;
    [SerializeField] private float maxLookDistance = 100f;
    [SerializeField] private LayerMask layerMask;

    private float currentCrouchLayerWeight = 0f;
    public float crouchSmoothingSpeed = 2f;

    private PlayerInput playerInput;
    private Rigidbody _rigidbody;
    private PlayerMovement movement;

    private void Start()
    {
        playerInput = GetComponent<PlayerInput>();
        _rigidbody = GetComponent<Rigidbody>();
        movement = GetComponent<PlayerMovement>();
    }

    public void Update()
    {
        var moveX = playerInput.move.x;
        var moveY = playerInput.move.y;

        var lookX = playerInput.look.x;

        animator.SetFloat("MoveX", moveX);
        animator.SetFloat("MoveY", moveY);

        if (playerInput.move.magnitude == 0)
        {
            float smoothedLookX = Mathf.Lerp(previousLookX, lookX, Time.deltaTime * smoothingSpeed);
            animator.SetFloat("MoveY", Mathf.Clamp(smoothedLookX, -0.5f, 0.5f));

            previousLookX = smoothedLookX;
        }

        animator.SetBool("IsSprinting", playerInput.sprint);
        animator.SetFloat("Velocity", _rigidbody.velocity.magnitude);

        animator.SetBool("Grounded", movement.isGrounded);

        if (Input.GetKeyDown(GlobalInputVariables.jumpKey) && movement.canJump)
            animator.SetTrigger("Jump");

        float targetCrouchLayerWeight = Input.GetKey(GlobalInputVariables.crouchKey) ? 1f : 0f;
        currentCrouchLayerWeight = Mathf.Lerp(currentCrouchLayerWeight, targetCrouchLayerWeight, Time.deltaTime * crouchSmoothingSpeed);
        animator.SetLayerWeight(1, currentCrouchLayerWeight);
    }

    public void LateUpdate()
    {
        if(isEnableFollowHead)
            FollowHead();
    }

    private void FollowHead()
    {
        Vector3 rayOrigin = cameraToLookFrom.transform.position;
        Vector3 rayDirection = cameraToLookFrom.transform.forward;

        Debug.DrawRay(rayOrigin, rayDirection * maxLookDistance, Color.red);

        if (Physics.Raycast(rayOrigin, rayDirection, out RaycastHit hitInfo, maxLookDistance, layerMask))
        {
            Vector3 targetPoint = hitInfo.point;

            Debug.DrawLine(rayOrigin, targetPoint, Color.green);

            headTarget.position = targetPoint;
        }
    }

}
