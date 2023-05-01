using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Ping : MonoBehaviour
{
    public void Trigger()
    {
        Debug.Log("Ping!");
    }

    public void Trigger(object data)
    {
        CharacterManager cm = (CharacterManager)data;
        Debug.Log(cm.name + " Ping'd!");
    }
}
