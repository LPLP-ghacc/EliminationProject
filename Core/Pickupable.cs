using Assets.Scripts.Player.Enums;
using Assets.Scripts.Player.Weapons;
using IKPn;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using static IKPn.IKPModule_UpperBody;

public class Pickupable : MonoBehaviour
{
    public string visibleItemName = "sample text";
    public PickupableItemType Type;
    public GameObject initPrefab;

    public void OnPickUp()
    {
        if(Type == PickupableItemType.Rifle)
        {
            var player = GameObject.FindGameObjectsWithTag("Player");
            
            if(player == null)
            {
                Debug.Log("No gameobjects with tag 'Player' finded in Scene!");
                return;
            }

            if(player.Length > 1)
            {
                Debug.Log("Here is two or more objs with tag 'Player'. Return.");
                return;
            }

            var playerAnimComp = player[0].GetComponent<PlayerAnimatorControl>();
            if(playerAnimComp.weaponSpawnPos != null)
            {
                var spawnTransform = playerAnimComp.weaponSpawnPos;

                var weapon = GameObject.Instantiate(initPrefab, spawnTransform.position, spawnTransform.rotation, spawnTransform.transform);

                // recode it later, ok??? we need to make custom system for ikp logic
                if(playerAnimComp.animator.gameObject.GetComponent<IKP>() != null)
                {
                    var ikp = playerAnimComp.animator.gameObject.GetComponent<IKP>();

                    var weaponComp = weapon.GetComponent<UWeapon>();

                    weaponComp.lookAtTarget = playerAnimComp.headTarget;
                    weaponComp.characterTransform = playerAnimComp.gameObject.transform;
                    playerAnimComp.GetComponent<PlayerCombatControl>().currentWeapon = weaponComp;

                    if (weaponComp.leftHand != null && weaponComp.rightHand != null)
                    {
                        ikp.SetHandTarget(Side.Left, weaponComp.leftHand);
                        ikp.SetHandTarget(Side.Right, weaponComp.rightHand);

                        ikp.SetProperty((int)IKPModule_UpperBody.Property.GeneralWeight, 1, ModuleSignatures.UPPER_HUMANOID);
                        Debug.Log($"Property[1] value = 1 {ikp.GetProperty((int)IKPModule_UpperBody.Property.GeneralWeight, ModuleSignatures.UPPER_HUMANOID)}");
                    }
                    else
                    {
                        Debug.Log("No Target Points!!!");
                    }
                }

                Debug.Log($"Weapon '{initPrefab.name}' was created in {spawnTransform.name} obj. Destroy {this.gameObject.name}");
                Destroy(this);
            }
        }
        if(Type == PickupableItemType.Pistol)
        {

        }
    }
}
