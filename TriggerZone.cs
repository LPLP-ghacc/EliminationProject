using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TriggerZone : MonoBehaviour
{
    private void OnTriggerEnter(Collider other)
    {
        if(other.GetComponent<PlayerCombatControl>() != null)
        {
            other.GetComponent<PlayerCombatControl>().OnInFight(true);
        }
    }

    //private void OnTriggerExit(Collider other)
    //{
    //    if (other.GetComponent<PlayerAnimatorControl>() != null)
    //    {
    //        var animator = other.GetComponent<PlayerAnimatorControl>().weaponSpawnPos.GetComponent<Animator>();
    //
    //        animator.SetBool("InFight", false);
    //    }
    //}
}
