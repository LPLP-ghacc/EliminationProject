using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerCombatControl : MonoBehaviour
{
    public UWeapon currentWeapon;
    private Animator weaponPivotAminator;
    public bool isAiming = false;
    public bool isCombat = false;

    private void Start()
    {
        var weaponPivot = GetComponent<PlayerAnimatorControl>().weaponSpawnPos;
        weaponPivotAminator = weaponPivot.GetComponent<Animator>();
    }

    private void Update()
    {
        if(currentWeapon != null)
        {
            isAiming = Input.GetMouseButton(1) && isCombat ? true : false;
            weaponPivotAminator.SetBool("Aiming", isAiming);
        }
    }

    public void OnInFight(bool infight)
    {
        if(currentWeapon != null)
        {
            var animcomp = GetComponent<PlayerAnimatorControl>();
            isCombat = infight;
            animcomp.weaponSpawnPos.GetComponent<Animator>().SetBool("InFight", isCombat);

            currentWeapon.isFollowTarget = isCombat;
        }
    }
}
