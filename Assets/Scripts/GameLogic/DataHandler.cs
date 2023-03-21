using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class DataHandler
{
    public static void LoadGameData()
    {
        Game.CHARACTER_DATA = Resources.LoadAll<CharacterData>("Scriptable Objects/Characters");

        GameParameters[] gameParametersList = Resources.LoadAll<GameParameters>("Scriptable Objects/Parameters");
        foreach (GameParameters parameters in gameParametersList)
            parameters.LoadFromFile();
    }

    public static void SaveGameData()
    {
        // save game parameters
        GameParameters[] gameParametersList = Resources.LoadAll<GameParameters>("Scriptable Objects/Parameters");
        foreach (GameParameters parameters in gameParametersList)
            parameters.SaveToFile();
    }

    public static CharacterData LoadCharacter(string name)
    {
        return Resources.Load<CharacterData>($"Scriptable Objects/Characters/{name}");
    }
}

