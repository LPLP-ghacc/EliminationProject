using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class PlayerGroundControl : MonoBehaviour
{
    public bool isGrounded = false;

    private void OnTriggerStay(Collider other)
    {
        if (other.gameObject.layer == LayerMask.NameToLayer("Default"))
        {
            isGrounded = true;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        isGrounded = false;
    }
}
