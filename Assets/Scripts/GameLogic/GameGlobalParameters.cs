using UnityEngine;

[CreateAssetMenu(fileName = "Parameters", menuName = "Scriptable Objects/Game Parameters", order = 10)]
public class GameGlobalParameters : GameParameters
{
    public override string GetParametersName() => "Global";

    [Header("Difficulty Settings")]
    public int tempSetting;
}