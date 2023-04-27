using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class GameParameters : JSONSerializableScriptableObject
{
    public abstract string GetParametersName();

    [SerializeField]
    protected List<string> _fieldsToShowInGame;

    private bool ShowsField(string fieldName)
    {
        return _fieldsToShowInGame != null && _fieldsToShowInGame.Contains(fieldName);
    }

    public void ToggleShowField(string fieldName)
    {
        // assigns if null
        _fieldsToShowInGame ??= new List<string>();

        if (ShowsField(fieldName))
            _fieldsToShowInGame.Remove(fieldName);
        else
            _fieldsToShowInGame.Add(fieldName);
    }

    public List<string> FieldsToShowInGame => _fieldsToShowInGame;
}
