using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PassiveItem
{
    private readonly PassiveItemData _data;
    
    public PassiveItem(PassiveItemData data)
    {
        _data = data;
    }





    public PassiveItemData Data => _data;
}
