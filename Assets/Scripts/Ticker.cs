using MoonSharp.Interpreter;
using System;
using UnityEngine;
using Yeeter;

public class Ticker : MonoBehaviour
{
    public static Action<int> OnTick { get; set; }

    private static int _tick = 0;
    private static float _timeBetweenTicks = 0.5f;
    private static float _nextTickTime = 0.0f;
    private static bool _timeTickEnabled = false;

    private void Awake()
    {
        LuaManager.OnLuaObjectSetUp += RegisterLuaObjectComponentTick;
        if (OnTick == null) OnTick += LogTick;
        _nextTickTime = _timeBetweenTicks;
    }
    private void Update()
    {
        if (!_timeTickEnabled) return;
        if (Time.time >= _nextTickTime)
        {
            OnTick?.Invoke(++_tick);
            _nextTickTime = Time.time + _timeBetweenTicks;
        }
    }
    private void LogTick(int tick)
    {
        InGameDebug.Log("Tick test: Tick = " + tick);
    }
    public static void Tick()
    {
        OnTick?.Invoke(++_tick);
    }
    public static void RegisterLuaObjectComponentTick(LuaObjectComponent luaObject)
    {
        var function = luaObject.Table.Get("Tick");
        if (function != null)
        {
            OnTick += tick => LuaManager.Call(function, DynValue.NewNumber(tick));
        }
    }
}
