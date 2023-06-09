using System.Collections;
using System.Collections.Generic;
using UnityEditor.SceneManagement;
using UnityEngine;

public class PHYMeleeWeaponManager : WeaponManager
{
    private HingeJoint2D _hinge;

    public override void Initialize(Weapon weapon)
    {
        base.Initialize(weapon);
        _hinge = transform.GetComponent<HingeJoint2D>();
        // link hinge joint
        _hinge.connectedBody = Weapon.Owner.Transform.GetComponent<Rigidbody2D>();
    }
    
    public override void Attack(Vector2 attackLocation)
    {
        _hinge.useMotor = true;
        TriggerOnAttackEffects();
        AttackRefresh();
        CheckReload();
    }
    
    public override void Attack()
    {
        _hinge.useMotor = true;
        TriggerOnAttackEffects();
        AttackRefresh();
        CheckReload();
    }
    
    public override void ConvertToPickup()
    {
        base.ConvertToPickup();
        Destroy(_hinge);
        _hinge = null;
        Destroy(GetComponent<Rigidbody2D>());
    }
}
