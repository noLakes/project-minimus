using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AnimatedMeleeWeaponManager : WeaponManager
{
    [SerializeField] private Transform hitBoxOrigin;
    [SerializeField] private float hitRadius;
    
    public override void Attack(Vector2 attackLocation)
    {
        Animator.SetTrigger("Attack");
        //_weaponManager.OnWeaponAttack();
        CharacterWeaponAimer.PauseAiming();
        AttackRefresh();
        CheckReload();
    }
    
    public override void Attack()
    {
        Animator.SetTrigger("Attack");
        //_weaponManager.OnWeaponAttack();
        CharacterWeaponAimer.PauseAiming();
        AttackRefresh();
        CheckReload();
    }
    
    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.blue;
        var position = hitBoxOrigin == null ? Vector3.zero : hitBoxOrigin.position;
        Gizmos.DrawWireSphere(position, hitRadius);
    }
    
    // triggered during melee animation
    // some stop-on-hit logic may need to interact with this in future
    public void DetectColliders()
    {
        foreach (Collider2D c in Physics2D.OverlapCircleAll(hitBoxOrigin.position, hitRadius))
        {
            Weapon.ProcessHit(c, c.transform.position, transform.position);
        }
    }
    
    public float HitRadius => hitRadius;
}
