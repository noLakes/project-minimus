using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerCharacter : Character
{
    private Dictionary<string, PassiveItem> _passiveItemInventory;
    
    public PlayerCharacter(CharacterData initialData) : base(initialData)
    {
    }
    
}
