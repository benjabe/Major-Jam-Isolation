local floor = {}

function floor:Start(id)
    -- set up ui
    floor:CreateUI(id)

    -- hotkey to generate a new floor
    BBInput.AddOnAxisPressed("RemakeFloor", function()
        floor:Generate(id)
    end)
    -- generate the floor
    floor:Generate(id)
    -- add a tick to the floor so that we can have ticked behaviours
    ObjectBuilder.Instantiate("Tick")
end

function floor:Update(id, dt)
end

function floor:CreateUI(id)
    local font = "Kenney Blocks"
    local fontSize = 16
    --local uiBottom = UI.Create("EmptyStretchAll")
    local uiMiddle = UI.Create("EmptyStretchAll")

    -- create console so we can cheat
    local uiTop = UI.Create("EmptyStretchAll")
    local console = UI.Create("Console")
    UI.SetParent(console, uiTop)

    -- display floor name
    local floorName = "B1F"
    local floorNameText = UI.Create("Text")
    UI.SetText(floorNameText, floorName)
    UI.SetAnchors(floorNameText, 0, 1, 0, 1)
    UI.SetPivot(floorNameText, 0, 1)
    UI.SetPosition(floorNameText, 10, 0)
    UI.SetFont(floorNameText, font)
    UI.SetFontSize(floorNameText, fontSize)
    UI.SetParent(floorNameText, uiMiddle)
    UI.SetTextColor(floorNameText, 255, 255, 255, 255)
    
    local levelText = UI.Create("Text")
    UI.SetText(levelText, "Lv1")
    UI.SetAnchors(levelText, 0, 1, 0, 1)
    UI.SetPivot(levelText, 0, 1)
    UI.SetPosition(levelText, 70, 0)
    UI.SetFont(levelText, font)
    UI.SetFontSize(levelText, fontSize)
    UI.SetParent(levelText, uiMiddle)
    UI.SetTextColor(levelText, 255, 255, 255, 255)

    -- display player level
    local hpText = UI.Create("Text")
    local onHealthSet = 
    Player.AddOnHealthSetListener(function(health)
        -- todo: remove this
        if ObjectBuilder.Get(hpText) ~= nil then
            UI.SetText(hpText, "HP"..health.."/1000")
        end
    end)
    UI.SetAnchors(hpText, 0, 1, 0, 1)
    UI.SetPivot(hpText, 0, 1)
    UI.SetPosition(hpText, 130, 0)
    UI.SetFont(hpText, font)
    UI.SetFontSize(hpText, fontSize)
    UI.SetParent(hpText, uiMiddle)
    UI.SetTextColor(hpText, 255, 255, 255, 255)

    -- display player hp
    --   numbers
    --   bar
end

function floor:Generate(id)
    FloorBuilder.ClearFloor()
    local size = { x = 25, y = 25 }
    -- make a bunch of walls
    for x = 0, size.x - 1, 1 do
        for y = 0, size.y - 1, 1 do
            FloorBuilder.PlaceTile("Wall", x, y)
        end
    end
    -- randomly assign entry and exit
    -- make sure they're a decent distance away
    local entry = { x = 0,  y = 0}
    local exit = { x = 0, y = 0}
    while math.abs(entry.x - exit.x) + math.abs(entry.x - exit.y) < 15 do
        entry.x = math.random(1, size.x - 2)
        entry.y = math.random(1, size.y - 2)
        exit.x = math.random(1, size.x - 2)
        exit.y = math.random(1, size.y - 2)
    end
    -- create a path from the entry to the exit
    local floorTilePositions = {}
    local previousDirection = 1
    local position = { x = entry.x, y = entry.y}
    while position.x ~= exit.x or position.y ~= exit.y do
        -- add the valid directions, i.e. ones that won't lead out of bounds
        local directions = {} -- 1 up, 2 left, 3 down, 4 right
        if position.y < size.y - 2  then table.insert(directions, 1) end
        if position.x > 1           then table.insert(directions, 2) end
        if position.y > 1           then table.insert(directions, 3) end
        if position.x < size.x - 2  then table.insert(directions, 4) end
        local direction = 0
        -- give a lot of weight to the previous direction so we don't get a snake.
        if math.random() < 0.5 and table.contains(directions, previousDirection) then
            direction = previousDirection
        else
            -- choose a random direction.
            direction = directions[math.random(#directions)]
            -- give some weight to the direction to the exit by rerolling if we're
            -- going the wrong way
            local distance = { x = exit.x - position.x, y = exit.y - position.y }
            if (direction == 1 and distance.y < 0) or
               (direction == 2 and distance.x > 0) or
               (direction == 3 and distance.y > 0) or
               (direction == 4 and distance.x < 0) and
               math.random() < 0.4 then
                direction = directions[math.random(#directions)]
            end
            -- we allow going back for now. it doesn't really hurt.
        end
        -- move in the chosen direction
        if direction == 1       then position.y = position.y + 1
        elseif direction == 2   then position.x = position.x - 1
        elseif direction == 3   then position.y = position.y - 1
        elseif direction == 4   then position.x = position.x + 1
        end
        -- place a floor tile
        FloorBuilder.PlaceTile("Floor", position.x, position.y)
        floorTilePositions[#floorTilePositions + 1] = { x = position.x, y = position.y}
        previousDirection = direction
    end
    -- place the exit and entry
    FloorBuilder.PlaceTile("Entry", entry.x, entry.y)
    FloorBuilder.PlaceTile("Exit", exit.x, exit.y)

    -- place a couple of characters around the level on random floor tile positions
    -- todo: select a random type of character from a level-floor def/.tyd file
    local characterCount = math.random(1, 4)
    for i = 1, characterCount do
        local pos = floorTilePositions[math.random(#floorTilePositions)]
        FloorBuilder.PlaceEnemy("At", pos.x, pos.y)
    end
    -- place him player boi at entrance
    FloorBuilder.PlacePlayer()
    -- enable dungeon control scheme
    BBInput.SetActiveProfile("Dungeon")
    ObjectBuilder.Instantiate("PlayerFollowCamera")
    FloorBuilder.BuildTileGraph()
end

function table.contains(table, element)
    for key, value in pairs(table) do
        if value == element then
            return true
        end
    end
    return false
end

return floor