using System.Collections;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using UnityEngine;
using UnityEngine.AI;

public static class Utility
{
    public static Vector2 GetMouseWorldPosition2D()
    {
        return (Vector2)Camera.main.ScreenToWorldPoint(Input.mousePosition);
    }

    public static Vector3 GetMouseWorldPosition()
    {
        return Camera.main.ScreenToWorldPoint(Input.mousePosition);
    }

    public static int GetAlphaKeyValue(string keyString)
    {
        if (keyString == "0" || keyString == "à") return 0;
        if (keyString == "1" || keyString == "&") return 1;
        if (keyString == "2" || keyString == "é") return 2;
        if (keyString == "3" || keyString == "\"") return 3;
        if (keyString == "4" || keyString == "'") return 4;
        if (keyString == "5" || keyString == "(") return 5;
        if (keyString == "6" || keyString == "§") return 6;
        if (keyString == "7" || keyString == "è") return 7;
        if (keyString == "8" || keyString == "!") return 8;
        if (keyString == "9" || keyString == "ç") return 9;
        return -1;
    }


    static Regex camelCaseRegex = new Regex(@"(?:[a-z]+|[A-Z]+|^)([a-z]|\d)*", RegexOptions.Compiled);
    
    public static string CapitalizeWords(string str)
    {
        List<string> words = new List<string>();
        MatchCollection matches = camelCaseRegex.Matches(str);
        string word;
        foreach (Match match in matches)
        {
            word = match.Groups[0].Value;
            word = word[0].ToString().ToUpper() + word.Substring(1);
            words.Add(word);
        }
        return string.Join(" ", words);
    }
}
