using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AimWeapon : MonoBehaviour
{
    public SpriteRenderer weaponRenderer, characterRenderer;
    private Vector3 _aimDirection;
    private Weapon _weapon;
    private bool _pauseAim = false;

    void Update()
    {
        if (transform.childCount == 0 || _pauseAim ) return;

        _aimDirection = (Utility.GetMouseWorldPosition() - transform.position).normalized;
        float angle = Mathf.Atan2(_aimDirection.y, _aimDirection.x) * Mathf.Rad2Deg;
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
        
        // hide weapon behind character sprite if aiming overhead
        if (transform.eulerAngles.z > 0 && transform.eulerAngles.z < 180)
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
        _weapon = weapon;
    }

    public void PauseAiming() => _pauseAim = true;
    public void ResumeAiming() => _pauseAim = false;
}
