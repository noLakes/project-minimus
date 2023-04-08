using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;

public class WeaponAttackManager : MonoBehaviour
{
    Weapon _weapon;
    
    private bool _reloading;
    private bool _refreshing;

    private int _reloadCounter;

    [SerializeField]
    private Transform hitBoxOrigin;
    [SerializeField]
    private float hitRadius;
    

    public bool Ready { get => !_reloading && !_refreshing; }

    IEnumerator _activeAttackRefreshRoutine;
    IEnumerator _activeReloadRoutine;

    public void Initialize(Weapon weapon)
    {
        Reset();
        _weapon = weapon;
    }

    public void OnWeaponAttack()
    {
        AttackRefresh();

        if(_weapon.Stats.magazineSize > 0)
        {
            _reloadCounter ++;

            if(_reloadCounter >= _weapon.Stats.magazineSize) Reload();
        }

        // add reload logic later... might be best to handle this with an event trigger?
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
        Debug.Log(_weapon.Data.name + " reloading for " + _weapon.Stats.reloadTime + "s");
        _reloading = true;
        yield return new WaitForSeconds(_weapon.Stats.reloadTime);
        _reloading = false;
        _reloadCounter = 0;
        _activeReloadRoutine = null;
    }

    private IEnumerator AttackRefreshCoroutine()
    {
        _refreshing = true;
        yield return new WaitForSeconds(_weapon.Stats.attackRate);
        _refreshing = false;
        _activeReloadRoutine = null;
    }

    private void Reset()
    {
        if(_activeReloadRoutine != null) StopCoroutine(_activeReloadRoutine);
        if(_activeAttackRefreshRoutine != null) StopCoroutine(_activeAttackRefreshRoutine);
        _reloading = false;
        _refreshing = false;
        _reloadCounter = 0;
    }

    public void DetectColliders()
    {
        foreach (Collider2D collider in Physics2D.OverlapCircleAll(hitBoxOrigin.position, hitRadius))
        {
            _weapon.ValidateHit(collider, collider.transform.position);
        }
    }
    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.blue;
        Vector3 position = hitBoxOrigin == null ? Vector3.zero : hitBoxOrigin.position;
        Gizmos.DrawWireSphere(position, hitRadius);
    }
}
