using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnPin : MonoBehaviour
{
    public CharacterData characterData;
    
    // Start is called before the first frame update
    void Start()
    {
        Character character = new Character(characterData);
        character.SetPosition(transform.position);
        
        Destroy(gameObject);
    }
}
