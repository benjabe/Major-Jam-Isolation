local init = {}
local background = 0
function init:Start(id)
    Console.LogObjectCreated = false
    LuaManager.Set(id, "colourMin", 43)
    LuaManager.Set(id, "colourMax", 171)
    LuaManager.Set(id, "r", 43)
    LuaManager.Set(id, "g", 171)
    LuaManager.Set(id, "b", 89)
    LuaManager.Set(id, "colourChangeRate", 5)
    -- background image
    background = UI.Create("EmptyStretchAll")
    UI.SetImage(background)
    -- set up ui
    UIBottom = UI.Create("EmptyStretchAll")
    UIMiddle = UI.Create("EmptyStretchAll")
    UITop = UI.Create("EmptyStretchAll")
    UI.SetParent(UI.Create("BBInputDebugger"), UITop)
    local console = UI.Create("Console")
    UI.SetParent(console, UITop)

    -- set up controls
    BBInput.AddOnAxisPressed("ToggleConsole", function()
        ObjectBuilder.ToggleEnabled(console)
    end)

    -- create main menu
    UI.CreateLuaObject("MainMenu")

    -- music TODO: Change this to something more main menu-y
    SoundManager.PlayMusic("Music.Music")
end

function init:Update(id, dt)
    local min = LuaManager.Get(id, "colourMin")
    local max = LuaManager.Get(id, "colourMax")
    local r = LuaManager.Get(id, "r")
    local g = LuaManager.Get(id, "g")
    local b = LuaManager.Get(id, "b")
    local d = LuaManager.Get(id, "colourChangeRate")
    -- change background colour over time
    if r == min and b == max and g ~= max then
        g = g + dt * d
    elseif r == min and g == max and b ~= min then
        b = b - dt * d
    elseif g == max and b == min and r ~= max then
        r = r + dt * d
    elseif r == max and b == min and g ~= min then
        g = g - dt * d
    elseif r == max and g == min and b ~= max then
        b = b + dt * d
    elseif g == min and b == max and r ~= min then
        r = r - dt * d
    end

    if g < min then g = min end
    if r < min then r = min end
    if b < min then b = min end
    if g > max then g = max end
    if r > max then r = max end
    if b > max then b = max end

    UI.SetColor(background, r, g, b)
    LuaManager.Set(id, "r", r)
    LuaManager.Set(id, "g", g)
    LuaManager.Set(id, "b", b)
end

return init