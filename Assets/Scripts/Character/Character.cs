using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class Character
{
    protected string uid;
    public CharacterData data { get; protected set; }
    public string code { get; protected set; }
    public Transform transform;
    public int health;
    public int maxHealth;
    public bool isActive { get; protected set; }


    public Character(CharacterData initialData)
    {
        uid = System.Guid.NewGuid().ToString();
        data = initialData;
        code = data.code;
        health = data.health;
        maxHealth = data.health;

        GameObject g = GameObject.Instantiate(data.prefab) as GameObject;
        transform = g.transform;
        transform.parent = Game.Instance.CHARACTER_CONTAINER;
        g.GetComponent<CharacterManager>().Initialize(this);
    }

    public virtual void SetPosition(Vector3 position)
    {
        transform.position = position;
    }

    public override string ToString()
    {
        return "{ code: " + code + ", uid: " + uid + " }";
    }

    public void Activate() => isActive = true;

    public void Deactivate(bool remove = true)
    {
        isActive = false;
        if (remove) Game.Instance.CHARACTERS.Remove(this);
    }
}
