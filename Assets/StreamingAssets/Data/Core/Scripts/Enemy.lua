local Enemy = {}

function Enemy:Start(id)
end

function Enemy:Update(id, dt)
end

function Enemy:Tick(id, tick)
    -- TODO: Have the enemies do something after killing the player
    if Player.IsDead then return end
    
    local playerPos = Player.Character.Position
    local character = FloorBuilder.FindCharacterWithId(id)
    local pos = character.Position

    -- if in attack range
    if math.abs(pos.y - playerPos.y) < 2 and
       math.abs(pos.x - playerPos.x) < 2 then
        -- make sure we're facing the right direction
        local facing = { x = 0, y = 0}
        if playerPos.x - pos.x < 0 then facing.x = -1
        elseif playerPos.x - pos.x > 0 then facing.x = 1 end
        if playerPos.y - pos.y < 0 then facing.y = -1
        elseif playerPos.y - pos.y > 0 then facing.y = 1 end
        character.SetFacing(facing.x, facing.y)
        -- attack
        character.Attack()
    -- if not in attack range
    else
        -- move towards the player
        local path = FloorBuilder.FindPathPositions(pos.x, pos.y, playerPos.x, playerPos.y)
        local nextPos = path[2]
        character.Move(nextPos.x - pos.x, nextPos.y - pos.y)
    end
end

function Enemy:OldSchoolFindPathToPlayer(id)
    local playerPos = Player.Character.Position
    local character = FloorBuilder.FindCharacterWithId(id)
    local pos = character.Position
    if math.abs(pos.x - playerPos.x) > 1 then
        local moved = false
        if pos.x < playerPos.x then moved = character.Move(1, 0)
        elseif pos.x > playerPos.x then moved = character.Move(-1, 0) end
        if moved then return end
    end
    if math.abs(pos.y - playerPos.y) > 1 then
        local moved = false
        if pos.y < playerPos.y then moved = character.Move(0, 1)
        elseif pos.y > playerPos.y then moved = character.Move(0, -1) end
        if moved then return end
    end
end

return Enemy