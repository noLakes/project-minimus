using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;

public class WeaponAttackManager : MonoBehaviour
{
    private Weapon _weapon;
    protected bool Refreshing;
    protected bool Reloading;
    private int _attackCounter;
    [SerializeField] private Transform hitBoxOrigin;
    [SerializeField] private float hitRadius;
    private IEnumerator _activeAttackRefreshRoutine;
    private IEnumerator _activeReloadRoutine;
    public virtual bool Ready => !Refreshing && !Reloading;

    public void Initialize(Weapon weapon)
    {
        Reset();
        _weapon = weapon;
    }

    public void OnWeaponAttack()
    {
        AttackRefresh();

        if (_weapon.Stats.MagazineSize <= 0) return;
        _attackCounter ++;
        if(_attackCounter >= _weapon.Stats.MagazineSize) Reload();
    }

    public void Reload()
    {
        _activeReloadRoutine = ReloadCoroutine();
        StartCoroutine(_activeReloadRoutine);
    }

    private void AttackRefresh()
    {
        _activeAttackRefreshRoutine = AttackRefreshCoroutine();
        StartCoroutine(_activeAttackRefreshRoutine);
    }

    private IEnumerator ReloadCoroutine()
    {
        Reloading = true;
        yield return new WaitForSeconds(_weapon.Stats.ReloadTime);
        Reloading = false;
        _attackCounter = 0;
        _activeReloadRoutine = null;
    }

    private IEnumerator AttackRefreshCoroutine()
    {
        Refreshing = true;
        yield return new WaitForSeconds(_weapon.Stats.AttackRate);
        Refreshing = false;
        _activeReloadRoutine = null;
    }

    private void Reset()
    {
        if(_activeReloadRoutine != null) StopCoroutine(_activeReloadRoutine);
        if(_activeAttackRefreshRoutine != null) StopCoroutine(_activeAttackRefreshRoutine);
        Reloading = false;
        Refreshing = false;
        _attackCounter = 0;
    }

    // triggered during melee animation
    // some stop-on-hit logic may need to interact with this in future
    public void DetectColliders()
    {
        foreach (Collider2D c in Physics2D.OverlapCircleAll(hitBoxOrigin.position, hitRadius))
        {
            _weapon.ProcessHit(c, c.transform.position, transform.position);
        }
    }
    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.blue;
        var position = hitBoxOrigin == null ? Vector3.zero : hitBoxOrigin.position;
        Gizmos.DrawWireSphere(position, hitRadius);
    }

    public float HitRadius => hitRadius;
}
