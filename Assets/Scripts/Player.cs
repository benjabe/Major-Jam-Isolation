using MoonSharp.Interpreter;
using System;
using UnityEngine;
using UnityEngine.Scripting;

[Preserve, MoonSharpUserData]
public class Player
{
    private static Character _character = null;

    public static bool IsHoldingFaceUp = false;
    public static bool IsHoldingFaceDown = false;
    public static bool IsHoldingFaceLeft = false;
    public static bool IsHoldingFaceRight = false;
    public static bool IsDead = false;
    public static Action OnSetPlayerCharacter;

    public static Vector2Int Facing { get => Character.Facing; }
    public static Character Character
    {
        get => _character;
        set
        {
            _character = value;
            IsDead = false;
            _character.OnDeath += _ => { IsDead = true; _character = null; };
            OnSetPlayerCharacter?.Invoke();
        }
    }

    public static void AddOnHealthSetListener(DynValue function)
    {
        if (Character != null)
        {
            Character.AddOnHealthSetListener(function);
        }
        else
        {
            OnSetPlayerCharacter += () => Character.AddOnHealthSetListener(function);
        }
    }
}
