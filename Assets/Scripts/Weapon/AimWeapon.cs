using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AimWeapon : MonoBehaviour
{
    private Transform weaponTransform;
    private Vector3 aimDirection;

    private void Awake()
    {
        weaponTransform = transform.Find("Weapon");
    }

    void Update()
    {
        aimDirection = (Utility.GetMouseWorldPosition() - transform.position).normalized;
        float angle = Mathf.Atan2(aimDirection.y, aimDirection.x) * Mathf.Rad2Deg;
        weaponTransform.eulerAngles = new Vector3(0, 0, angle);

        // flip weapon with local y scale so it is always upright
        Vector3 localScale = Vector3.one;
        if( angle > 90 || angle < -90)
        {
            localScale.y = -1f;
        }
        else
        {
            localScale.y = 1f;
        }

        weaponTransform.localScale = localScale;
    }
}
