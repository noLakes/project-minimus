using UnityEngine;

public class WeaponAnimationHelper : MonoBehaviour
{
    private Weapon _weapon;

    public void Initialize(Weapon weapon)
    {
        _weapon = weapon;
    }

    public void AttackStart()
    {
        _weapon.OnAttackAnimationStart();
    }
    public void AttackEnd()
    {
        _weapon.OnAttackAnimationEnd();
    }
}
