using System.Collections;
using System.Collections.Generic;

public struct WeaponStats
{
    public int Damage;
    public float AttackRate;
    public int MagazineSize;
    public float ReloadTime;
    public float Range;
    public List<Effect> OnHitEffects;

    public WeaponStats(WeaponData data)
    {
        Damage = data.damage;
        AttackRate = data.attackRate;
        MagazineSize = data.magazineSize;
        ReloadTime = data.reloadTime;
        Range = data.range;
        OnHitEffects = data.onHitEffects;
    }
}
