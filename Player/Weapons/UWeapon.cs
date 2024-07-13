using Assets.Scripts.Player.Weapons;
using IKPn;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class UWeapon : MonoBehaviour
{
    private Weapon weapon;
    public Transform muzzle;
    public int maxBullets = 20;
    [Range(0.1f, 3)]
    public float shootCycle = 0.3f;
    public int onStartBulletCount = 20;
    public IKPTarget leftHand, rightHand;

    //Follow variables
    public bool isFollowTarget = false;
    public Transform lookAtTarget;
    public Transform characterTransform;
    public float rotationSpeed = 5f;
    private bool isAiming = false;
    private int rotationDivider = 3;
    private float minRelativeYRotation = -10;
    private float maxRelativeYRotation = 10;

    void Start()
    {
        weapon = new Weapon(maxBullets, shootCycle, onStartBulletCount);
    }

    private void Update()
    {
        Debug.DrawRay(muzzle.position, muzzle.forward * 100, Color.blue);
    }

    void FixedUpdate()
    {
        if (isFollowTarget)
        {
            FollowTarget();
        }
    }

    private void FollowTarget()
    {
        Vector3 direction = lookAtTarget.position - transform.position;

        if (direction != Vector3.zero)
        {
            Quaternion targetRotation = Quaternion.LookRotation(direction);

            Quaternion lerpedRotation = Quaternion.Lerp(transform.rotation, targetRotation, rotationSpeed * Time.deltaTime);

            Quaternion relativeToCharacterRotation = Quaternion.Inverse(characterTransform.rotation) * lerpedRotation;
            Vector3 relativeEulerAngles = relativeToCharacterRotation.eulerAngles;

            if (relativeEulerAngles.y > 180f) relativeEulerAngles.y -= 360f;

            if (isAiming)
            {
                minRelativeYRotation /= rotationDivider;
                maxRelativeYRotation /= rotationDivider;
            }

            relativeEulerAngles.y = Mathf.LerpAngle(relativeEulerAngles.y, Mathf.Clamp(relativeEulerAngles.y, 
                minRelativeYRotation, 
                maxRelativeYRotation), rotationSpeed * Time.deltaTime);

            if (relativeEulerAngles.y < 0f) relativeEulerAngles.y += 360f;

            Quaternion clampedRotation = characterTransform.rotation * Quaternion.Euler(relativeEulerAngles);

            transform.rotation = clampedRotation;
        }
    }
}