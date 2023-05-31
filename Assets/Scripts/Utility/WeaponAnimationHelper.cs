using UnityEngine;

public class WeaponAnimationHelper : MonoBehaviour
{
    private WeaponManager _weaponManager;

    public void Initialize(WeaponManager weapon)
    {
        _weaponManager = weapon;
    }

    // triggered by animation events
    public void AttackStart()
    {
        _weaponManager.OnAttackAnimationStart();
    }
    
    // triggered by animation events
    public void AttackEnd()
    {
        _weaponManager.OnAttackAnimationEnd();
    }
}
