using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ReloadManager : MonoBehaviour
{
    Weapon _weapon;
    
    bool _reloading;
    bool _refreshing;

    int _reloadCounter;

    public bool Ready { get => !_reloading && !_refreshing; }

    IEnumerator activeAttackRefreshRoutine;
    IEnumerator activeReloadRoutine;

    public void Initialize(Weapon weapon)
    {
        _weapon = weapon;
        _reloading = false;
        _refreshing = false;
        _reloadCounter = 0;
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
        activeReloadRoutine = ReloadCoroutine();
        StartCoroutine(activeReloadRoutine);
    }

    private void AttackRefresh()
    {
        activeAttackRefreshRoutine = AttackRefreshCoroutine();
        StartCoroutine(activeAttackRefreshRoutine);
    }

    private IEnumerator ReloadCoroutine()
    {
        Debug.Log(_weapon.Data.name + " reloading for " + _weapon.Stats.reloadTime + "s");
        _reloading = true;
        yield return new WaitForSeconds(_weapon.Stats.reloadTime);
        _reloading = false;
        _reloadCounter = 0;
        activeReloadRoutine = null;
    }

    private IEnumerator AttackRefreshCoroutine()
    {
        _refreshing = true;
        yield return new WaitForSeconds(_weapon.Stats.attackRate);
        _refreshing = false;
        activeReloadRoutine = null;
    }
}
