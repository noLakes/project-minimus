using UnityEngine;

public class WeaponAnimationHelper : MonoBehaviour
{
    private Weapon _weapon;

    public void Initialize(Weapon weapon)
    {
        _weapon = weapon;
    }

    // triggered by animation events
    public void AttackStart()
    {
        _weapon.OnAttackAnimationStart();
    }
    
    // triggered by animation events
    public void AttackEnd()
    {
        _weapon.OnAttackAnimationEnd();
    }
}
