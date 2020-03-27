using MoonSharp.Interpreter;
using System.Collections.Generic;
using UnityEngine.Scripting;
using Yeeter;
using UnityEngine;
using Tyd;

[Preserve, MoonSharpUserData]
public class CharacterBuilder
{
    private static List<CharacterType> _types;
    private static Dictionary<string, CharacterType> _typeNameToType;

    static CharacterBuilder()
    {
        LoadTypes();
    }

    public static void LoadTypes()
    {
        _types = new List<CharacterType>();
        _typeNameToType = new Dictionary<string, CharacterType>();
        var tydNode = StreamingAssetsDatabase.GetDef("CharacterTypes") as TydCollection;
        foreach (var node in tydNode)
        {
            var type = CharacterType.FromTydTable(node as TydTable);
            _types.Add(type);
            _typeNameToType[type.FullName] = type;

        }
    }

    public static CharacterType RandomType()
    {
        return _types[Random.Range(0, _types.Count)];
    }

    public static int Create(string type)
    {
        var id = (int)ObjectBuilder.Instantiate("Character").Number;
        var go = ObjectBuilder.Get(id);
        var character = go.GetComponent<Character>();
        character.SetType(_typeNameToType[type]);
        return id;
    }
}
