using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AimWeapon : MonoBehaviour
{
    private Vector3 aimDirection;

    private void Awake()
    {
        
    }

    void Update()
    {
        aimDirection = (Utility.GetMouseWorldPosition() - transform.position).normalized;
        float angle = Mathf.Atan2(aimDirection.y, aimDirection.x) * Mathf.Rad2Deg;
        transform.eulerAngles = new Vector3(0, 0, angle);

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

        transform.localScale = localScale;
    }
}
