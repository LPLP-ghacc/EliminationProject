using Assets.Scripts.Player;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RagdollController : MonoBehaviour
{
    public GameObject player;
    public bool isRagdoll = false;
    private Animator animator;
    private Rigidbody[] ragdollRigidbodies;
    private Collider[] ragdollColliders;
    private Vector3 savedVelocity;
    private Vector3 savedAngularVelocity;

    void Start()
    {
        animator = GetComponent<Animator>();
        ragdollRigidbodies = GetComponentsInChildren<Rigidbody>();
        ragdollColliders = GetComponentsInChildren<Collider>();

        DeactivateRagdoll();
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.X))
        {
            isRagdoll = !isRagdoll;

            if (isRagdoll)
            {
                ActivateRagdoll();
                return;
            }
            else
            {
                DeactivateRagdoll();
            }
        }
    }

    public void ActivateRagdoll()
    {
        // Сохранение текущей скорости и угловой скорости
        var playerRigidbody = player.GetComponent<Rigidbody>();
        savedVelocity = playerRigidbody.velocity;
        savedAngularVelocity = playerRigidbody.angularVelocity;

        // Отключение основного Rigidbody
        playerRigidbody.isKinematic = true;
        player.GetComponent<Collider>().enabled = false;
        player.GetComponent<PlayerMovement>().canMoveRotate = false;

        animator.enabled = false;

        foreach (var rb in ragdollRigidbodies)
        {
            rb.isKinematic = false;
            // Передача сохраненной скорости каждому Rigidbody в Ragdoll
            rb.velocity = savedVelocity;
            rb.angularVelocity = savedAngularVelocity;
        }
        foreach (var col in ragdollColliders)
        {
            col.enabled = true;
        }

        transform.SetParent(transform.parent);
    }

    public void DeactivateRagdoll()
    {
        // Включение основного Rigidbody
        var playerRigidbody = player.GetComponent<Rigidbody>();
        playerRigidbody.isKinematic = false;
        player.GetComponent<Collider>().enabled = true;
        player.GetComponent<PlayerMovement>().canMoveRotate = true;

        animator.enabled = true;

        foreach (var rb in ragdollRigidbodies)
        {
            rb.isKinematic = true;
        }
        foreach (var col in ragdollColliders)
        {
            col.enabled = false;
        }
    }
}
