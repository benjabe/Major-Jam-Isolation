using MoonSharp.Interpreter;
using UnityEngine;
using UnityEngine.Scripting;
using UnityEngine.UI;

[Preserve, MoonSharpUserData]
public class Character : MonoBehaviour
{
    private CharacterType _type;

    public void SetType(CharacterType type)
    {
        _type = type;
        GetComponentInChildren<Text>().text = _type.Symbol;
    }
}
