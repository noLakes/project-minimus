using UnityEngine;
using UnityEngine.Serialization;

[CreateAssetMenu(fileName = "Parameters", menuName = "Scriptable Objects/Game Parameters", order = 10)]
public class GameGlobalParameters : GameParameters
{
    public override string GetParametersName() => "Global";

    [Header("Player Starting Data")] 
    public CharacterData startingCharacter;

    [Header("Item Settings")] 
    [Range(0.25f, 5f)] public float itemPickupRange;
}