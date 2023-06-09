using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class DataHandler
{
    public static void LoadGameData()
    {
        Game.CHARACTER_DATA = Resources.LoadAll<CharacterData>("Scriptable Objects/Characters");
        Game.WEAPON_DATA = Resources.LoadAll<WeaponData>("Scriptable Objects/Weapons");
        Game.ITEM_DATA = Resources.LoadAll<ItemData>("Scriptable Objects/Items");

        var gameParametersList = Resources.LoadAll<GameParameters>("Scriptable Objects/Parameters");
        foreach (var parameters in gameParametersList)
            parameters.LoadFromFile();
    }

    public static void SaveGameData()
    {
        // save game parameters
        var gameParametersList = Resources.LoadAll<GameParameters>("Scriptable Objects/Parameters");
        foreach (var parameters in gameParametersList)
            parameters.SaveToFile();
    }

    public static CharacterData LoadCharacter(string name)
    {
        return Resources.Load<CharacterData>($"Scriptable Objects/Characters/{name}");
    }

    public static WeaponData LoadWeapon(string name)
    {
        return Resources.Load<WeaponData>($"Scriptable Objects/Weapons/{name}");
    }
    
    public static ItemData LoadItem(string name)
    {
        return Resources.Load<ItemData>($"Scriptable Objects/Items/{name}");
    }
}

