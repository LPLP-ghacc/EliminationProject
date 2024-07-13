using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ThirdPersonCameraCollision : MonoBehaviour
{
    public Transform player;
    public Transform cameraPivot;
    public Transform cameraTransform; 
    public float cameraDistance = 3.0f; 
    public float cameraSpeed = 10f; 
    public float minDistanceToPivot = 0.5f;
    public LayerMask collisionMask;

    private Vector3 dollyDir;
    private float currentDistance; 

    void Start()
    {
        dollyDir = cameraTransform.localPosition.normalized;
        currentDistance = cameraTransform.localPosition.magnitude;
    }

    void LateUpdate()
    {
        Vector3 desiredCameraPos = cameraPivot.TransformPoint(dollyDir * cameraDistance);

        RaycastHit hit;
        if (Physics.Linecast(cameraPivot.position, desiredCameraPos, out hit, collisionMask))
        {
            currentDistance = Mathf.Clamp(hit.distance * 0.8f, minDistanceToPivot, cameraDistance);
        }
        else
        {
            currentDistance = cameraDistance;
        }

        cameraTransform.localPosition = Vector3.Lerp(cameraTransform.localPosition, dollyDir * currentDistance, Time.deltaTime * cameraSpeed);
    }
}
