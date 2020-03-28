using MoonSharp.Interpreter;
using Yeeter;

public class CustomLuaManager : LuaManager
{
    public override Script CreateScript()
    {
        var script = base.CreateScript();
        UserData.RegisterAssembly(typeof(CustomLuaManager).Assembly);
        script.Globals["FloorBuilder"] = new FloorBuilder();
        script.Globals["CharacterBuilder"] = new CharacterBuilder();
        script.Globals["TickerUtility"] = new TickerUtility();
        script.Globals["Player"] = new Player();
        return script;
    }
}
