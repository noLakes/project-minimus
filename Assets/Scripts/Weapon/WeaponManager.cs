using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;

public class WeaponManager : MonoBehaviour
{
    protected Weapon Weapon;
    
    protected CharacterWeaponAimer CharacterWeaponAimer;
    protected Animator Animator;
    private SpriteRenderer _spriteRenderer;
    
    protected bool Refreshing;
    protected bool Reloading;
    private int _attackCounter;
    private IEnumerator _activeAttackRefreshRoutine;
    private IEnumerator _activeReloadRoutine;
    public float ComputedRange { get; protected set; }
    public virtual bool Ready => !Refreshing && !Reloading;

    public virtual void Initialize(Weapon weapon)
    {
        Reset();
        Weapon = weapon;
        CharacterWeaponAimer = Weapon.Owner.Transform.GetComponentInChildren<CharacterWeaponAimer>();
        CharacterWeaponAimer.ResetPosition();
        CharacterWeaponAimer.ResumeAiming(); // prevents weapon aim staying stuck in previous equipped weapon attack dir
        
        if (transform.TryGetComponent(out Animator))
        {
            transform.GetComponent<WeaponAnimationHelper>()?.Initialize(this);
        }
        
        _spriteRenderer = Weapon.Transform.GetComponent<SpriteRenderer>();

        ComputeRange();
    }

    public virtual void Attack()
    {
        // leave implementation to child classes
    }

    public virtual void Attack(Vector2 location)
    {
        // leave implementation to child classes
    }
    
    protected void CheckReload()
    {
        if (Weapon.Stats.magazineSize <= 0) return;
        _attackCounter ++;
        if(_attackCounter >= Weapon.Stats.magazineSize) Reload();
    }

    protected void Reload()
    {
        _activeReloadRoutine = ReloadCoroutine();
        StartCoroutine(_activeReloadRoutine);
    }

    protected void AttackRefresh()
    {
        _activeAttackRefreshRoutine = AttackRefreshCoroutine();
        StartCoroutine(_activeAttackRefreshRoutine);
    }

    protected IEnumerator ReloadCoroutine()
    {
        Reloading = true;
        yield return new WaitForSeconds(Weapon.Stats.reloadTime);
        Reloading = false;
        _attackCounter = 0;
        _activeReloadRoutine = null;
    }

    protected IEnumerator AttackRefreshCoroutine()
    {
        Refreshing = true;
        yield return new WaitForSeconds(Weapon.Stats.attackRate);
        Refreshing = false;
        _activeReloadRoutine = null;
    }

    protected void TriggerOnAttackEffects()
    {
        Transform ownerTransform = Weapon.Owner.Transform;
        var effectArgs = new EffectArgs(ownerTransform, ownerTransform, transform.position);
        Effect.ApplyEffectList(Weapon.Stats.onAttackEffects, effectArgs);
    }

    protected void Reset()
    {
        if(_activeReloadRoutine != null) StopCoroutine(_activeReloadRoutine);
        if(_activeAttackRefreshRoutine != null) StopCoroutine(_activeAttackRefreshRoutine);
        Reloading = false;
        Refreshing = false;
        _attackCounter = 0;
    }
    
    public void OnAttackAnimationStart()
    {
        // do something
    }
    
    public void OnAttackAnimationEnd()
    {
        CharacterWeaponAimer.ResumeAiming();
    }
    
    protected void ComputeRange()
    {
        ComputedRange = Weapon.Stats.range;
        // needs to be re-implemented
        /*
        switch (Weapon.Data.type)
        {
            case WeaponType.AnimationMelee:
            {
                float ownerHalfSize = Weapon.Owner.Transform.GetComponent<CharacterManager>().Size / 2;
                Debug.Log("ComputeRange: " + ownerHalfSize + " / " + _weaponManager.HitRadius * 1.9f);
                _computedRange = (_weaponManager.HitRadius * 1.95f) + ownerHalfSize;
                break;
            }
            case WeaponType.Ranged:
            {
                // not implemented yet
                _computedRange = _activeStats.Range; // replace this
                break;
            }
        }
        Debug.Log("ComputedRange: " + _computedRange);
        */
        
    }
    
    public virtual void ConvertToPickup()
    {
        if (Animator != null)
        {
            Destroy(Animator);
            Animator = null;
            
            if(TryGetComponent<WeaponAnimationHelper>(out var animHelper)) Destroy(animHelper);
        }
    }

    public SpriteRenderer SpriteRenderer => _spriteRenderer;
}
