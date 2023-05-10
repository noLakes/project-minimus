using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeaponAimManager : MonoBehaviour
{
    public SpriteRenderer weaponRenderer, characterRenderer;
    protected Vector3 AimDirection;
    protected Weapon Weapon;
    protected bool PauseAim = false;

    private void Update()
    {
        if (transform.childCount == 0 || PauseAim ) return;

        AimDirection = (Utility.GetMouseWorldPosition() - transform.position).normalized;
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

    public void UpdateSpriteRenderers(SpriteRenderer weapon, SpriteRenderer character)
    {
        weaponRenderer = weapon;
        characterRenderer = character;
    }

    public void SetWeapon(Weapon weapon)
    {
        Weapon = weapon;
    }

    public void ResetPosition()
    {
        transform.localPosition = Vector3.zero;
        transform.localEulerAngles = Vector3.zero;
        transform.localScale = Vector3.one;
    }

    public void PauseAiming() => PauseAim = true;
    public void ResumeAiming() => PauseAim = false;
}
