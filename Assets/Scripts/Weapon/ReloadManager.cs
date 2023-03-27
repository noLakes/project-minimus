using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ReloadManager : MonoBehaviour
{
    Weapon _weapon;
    
    bool _ready;
    public bool Ready { get => _ready; }

    IEnumerator activeReloadRoutine;


    public void Initialize(Weapon weapon)
    {
        _weapon = weapon;
    }

    public void Reload()
    {
        activeReloadRoutine = ReloadCoroutine();
        StartCoroutine(activeReloadRoutine);
    }

    private IEnumerator ReloadCoroutine()
    {
        _ready = false;
        yield return new WaitForSeconds(_weapon.Data.reloadTime);
        _ready = true;
        activeReloadRoutine = null;
    }
}
