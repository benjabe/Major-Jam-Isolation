local floor = {}

function floor:Start(id)
    -- hotkey to generate a new floor
    BBInput.AddOnAxisPressed("RemakeFloor", function()
        floor:Generate(id)
    end)
    -- generate the floor
    floor:Generate(id)

end

function floor:Update(id, dt)
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
    FloorBuilder.PositionCameraAtEntry()

    -- place a couple of characters around the level on random floor tile positions
    -- todo: select a random type of character from a level-floor def/.tyd file
    local characterCount = math.random(1, 4)
    for i = 1, characterCount do
        local pos = floorTilePositions[math.random(#floorTilePositions)]
        local character = FloorBuilder.PlaceCharacter("At", pos.x, pos.y)
    end
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