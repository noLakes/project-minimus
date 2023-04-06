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
        Debug.Log("ATTACK STARTED!");
    }
    public void AttackEnd()
    {
        _weapon.OnWeaponAttackAnimationEnd();
    }
}
