using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AIWeaponAimManager : WeaponAimManager
{
    private AICharacterManager _aiCharacterManager;
    
    private void Start()
    {
        _aiCharacterManager = GetComponent<AICharacterManager>();
    }
    
    private void Update()
    {
        // do nothing?
    }

    public void AimTowards(Vector2 point)
    {
        AimDirection = (point - (Vector2)transform.position).normalized;
        var angle = Mathf.Atan2(AimDirection.y, AimDirection.x) * Mathf.Rad2Deg;
        transform.eulerAngles = new Vector3(0, 0, angle);

        // flip weapon with local y scale so it is always upright
        var localScale = Vector3.one;
        if( angle is > 90 or < -90)
        {
            localScale.y = -1f;
        }
        else
        {
            localScale.y = 1f;
        }

        transform.localScale = localScale;
        
        // hide weapon behind character sprite if aiming overhead
        if (transform.eulerAngles.z is > 0 and < 180)
        {
            weaponRenderer.sortingOrder = characterRenderer.sortingOrder - 1;
        }
        else
        {
            weaponRenderer.sortingOrder = characterRenderer.sortingOrder + 1;
        }
    }
}
