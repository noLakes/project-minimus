using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "PassiveItem", menuName = "Scriptable Objects/PassiveItem", order = 1)]
public class PassiveItemData : ScriptableObject
{
    public string code;
    public string itemName;
    public GameObject prefab;
    public Sprite uiSprite;
    public List<Effect> passiveEffects;
    public List<Effect> onHitEffects;
}
