using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Weapon : MonoBehaviour
{
    [SerializeField]
    float _cooldown;
    IEnumerator cooldownRoutine;

    public bool ready { get; protected set; }

    public virtual void TryAttack(Vector2 direction)
    {
        if(!ready) return;
        else Attack(direction);
    }

    protected virtual void Attack(Vector2 direction)
    {
        cooldownRoutine = StartCooldownRoutine();
    }

    IEnumerator StartCooldownRoutine()
    {
        SetReady(false);
        yield return new WaitForSeconds(_cooldown);
        SetReady(true);
        cooldownRoutine = null;
    }

    private void SetReady(bool status)
    {
        ready = status;
    }

    private void ResetCooldown()
    {
        if(cooldownRoutine != null)
        {
            StopCoroutine(cooldownRoutine);
            cooldownRoutine = null;
        }

        SetReady(true);
    }

}
