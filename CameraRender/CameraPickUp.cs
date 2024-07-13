using Assets.Scripts.Core;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraPickUp : MonoBehaviour
{
    private Camera _camera;
    public LayerMask collisionMask;

    private void Start()
    {
        _camera = GetComponent<Camera>();
    }

    void LateUpdate()
    {
        RaycastHit hit;
        if (Physics.Raycast(_camera.transform.position, _camera.transform.forward, out hit, collisionMask))
        {
            if(Input.GetKeyDown(GlobalInputVariables.interactKey))
                if(hit.collider.gameObject.GetComponent<Pickupable>() != null)
                {
                    var item = hit.collider.gameObject.GetComponent<Pickupable>();
                    string name = item.visibleItemName;
                    item.OnPickUp();
                }
        }
    }
}
