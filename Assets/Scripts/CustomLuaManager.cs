using MoonSharp.Interpreter;
using UnityEngine;
using Yeeter;

public class CustomLuaManager : LuaManager
{
    public override Script CreateScript()
    {
        var script = base.CreateScript();
        UserData.RegisterAssembly(typeof(CustomLuaManager).Assembly);
        UserData.RegisterType<Vector2Int>();
        script.Globals["FloorBuilder"] = new FloorBuilder();
        script.Globals["CharacterBuilder"] = new CharacterBuilder();
        script.Globals["TickerUtility"] = new TickerUtility();
        script.Globals["Player"] = new Player();
        return script;
    }
}
