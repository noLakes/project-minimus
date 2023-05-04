using System.Collections;
using System.Collections.Generic;

public struct CharacterStats
{
    public int maxHealth;
    public int maxWeaponCount;

    public CharacterStats(CharacterData data)
    {
        maxHealth = data.maxHealth;
        maxWeaponCount = data.maxWeaponCount;
    }
}