using MoonSharp.Interpreter;
using UnityEngine.Scripting;
using Yeeter;

[Preserve, MoonSharpUserData]
public class TickerUtility
{
    public static void Tick()
    {
        Ticker.Tick();
    }
    public static void AddOnTickListener(DynValue function)
    {
        Ticker.OnTick += n => LuaManager.Call(function, DynValue.NewNumber(n));
    }
}
