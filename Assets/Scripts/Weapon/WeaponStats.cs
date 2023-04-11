using System.Collections;
using System.Collections.Generic;

public struct WeaponStats
{
    public int damage;
    public float attackRate;
    public int magazineSize;
    public float reloadTime;
    public float range;
    public List<Effect> onHitEffects;

    public WeaponStats(WeaponData data)
    {
        damage = data.damage;
        attackRate = data.attackRate;
        magazineSize = data.magazineSize;
        reloadTime = data.reloadTime;
        range = data.range;
        onHitEffects = data.onHitEffects;
    }
}
