using MoonSharp.Interpreter;
using System;
using System.Collections.Generic;
using UnityEngine;
using Yeeter;

public class Ticker : MonoBehaviour
{
    private static Dictionary<LuaObjectComponent, DynValue> _luaObjectTickCallbacks
        = new Dictionary<LuaObjectComponent, DynValue>();
    public static Action<int> OnTick { get; set; }

    private static int _tick = 0;
    private static float _timeBetweenTicks = 0.5f;
    private static float _nextTickTime = 0.0f;
    private static bool _timeTickEnabled = false;

    private void Awake()
    {
        LuaManager.OnLuaObjectSetUp += RegisterLuaObjectComponentTick;
        if (OnTick == null)  OnTick += CallLuaObjectCallbacks;
        //if (OnTick == null) OnTick += LogTick;
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
        if (function.IsNotNil())
        {
            _luaObjectTickCallbacks[luaObject] = function;
            luaObject.OnDestroyed += OnLuaObjectDestroyed;
        }
    }
    private static void OnLuaObjectDestroyed(LuaObjectComponent luaObject)
    {
        _luaObjectTickCallbacks.Remove(luaObject);
        luaObject.OnDestroyed -= OnLuaObjectDestroyed;
    }
    private static void CallLuaObjectCallbacks(int tick)
    {
        foreach (var luaObject in _luaObjectTickCallbacks.Keys)
        {
            LuaManager.Call(
                _luaObjectTickCallbacks[luaObject],
                DynValue.NewNumber(0),  // I literally don't know why I need this
                DynValue.NewNumber(luaObject.Id),
                DynValue.NewNumber(tick)
            );
        }
    }
}
