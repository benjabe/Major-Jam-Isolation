local player = { characterId = 0 }

function player:Start(id)
    BBInput.AddOnAxisPressed("MoveUp", function() player:Move(id, 0, 1) end)
    BBInput.AddOnAxisPressed("MoveDown", function() player:Move(id, 0, -1) end)
    BBInput.AddOnAxisPressed("MoveLeft", function() player:Move(id, -1, 0) end)
    BBInput.AddOnAxisPressed("MoveRight", function() player:Move(id, 1, 0) end)

    BBInput.AddOnAxisPressed("MoveInFacingDirection", function()
        player:Move(id, Player.Facing.x, Player.Facing.y)
    end)

    BBInput.AddOnAxisPressed("FaceUp", function() Player.IsHoldingFaceUp = true end)
    BBInput.AddOnAxisPressed("FaceDown", function() Player.IsHoldingFaceDown = true end)
    BBInput.AddOnAxisPressed("FaceLeft", function() Player.IsHoldingFaceLeft = true end)
    BBInput.AddOnAxisPressed("FaceRight", function() Player.IsHoldingFaceRight = true end)

    BBInput.AddOnAxisReleased("FaceUp", function() Player.IsHoldingFaceUp = false end)
    BBInput.AddOnAxisReleased("FaceDown", function() Player.IsHoldingFaceDown = false end)
    BBInput.AddOnAxisReleased("FaceLeft", function() Player.IsHoldingFaceLeft = false end)
    BBInput.AddOnAxisReleased("FaceRight", function() Player.IsHoldingFaceRight = false end)

    BBInput.AddOnAxisPressed("FaceUp", function()
        if Player.IsHoldingFaceLeft then player:Face(id, -1, 1)
        elseif Player.IsHoldingFaceRight then player:Face(id, 1, 1)
        else player:Face(id, 0, 1) end
    end)
    BBInput.AddOnAxisPressed("FaceDown", function()
        if Player.IsHoldingFaceLeft then player:Face(id, -1, -1)
        elseif Player.IsHoldingFaceRight then player:Face(id, 1, -1)
        else player:Face(id, 0, -1) end
    end)
    BBInput.AddOnAxisPressed("FaceLeft", function()
        if Player.IsHoldingFaceUp then player:Face(id, -1, 1)
        elseif Player.IsHoldingFaceDown then player:Face(id, -1, -1)
        else player:Face(id, -1, 0) end
    end)
    BBInput.AddOnAxisPressed("FaceRight", function()
        if Player.IsHoldingFaceUp then player:Face(id, 1, 1)
        elseif Player.IsHoldingFaceDown then player:Face(id, 1, -1)
        else player:Face(id, 1, 0) end
    end)

    BBInput.AddOnAxisPressed("Attack", function() player:Attack(id) end)
    BBInput.AddOnAxisPressed("Wait", TickerUtility.Tick)

    Player.Character.HealOrTakeDamage(10000)
end

function player:Update(id, dt)
end

function player:Move(id, x, y)
    if Player.IsDead then return end
    Player.Character.Move(x, y)
    local facing = {}
    if player:IsHoldingAnyFaceButton() then
        facing.x = 0
        facing.y = 0
        if Player.IsHoldingFaceUp then facing.y = 1 end
        if Player.IsHoldingFaceDown then facing.y = -1 end
        if Player.IsHoldingFaceLeft then facing.x = -1 end
        if Player.IsHoldingFaceRight then facing.x = 1 end
    else
        facing.x = x
        facing.y = y
    end
    player:Face(id, facing.x, facing.y)
    -- go to next floor if exit
    local pos = Player.Character.Position
    if FloorBuilder.GetTile(pos.x, pos.y).HasProperty("Exit") then
        Console.Log("Exit da floor?")
        FloorBuilder.NextFloor()
    end
    TickerUtility.Tick()
end

function player:Face(id, x, y)
    if Player.IsDead then return end
    Player.Character.SetFacing(x, y)
end

function player:IsHoldingAnyFaceButton()
    return Player.IsHoldingFaceUp or Player.IsHoldingFaceDown or
           Player.IsHoldingFaceLeft or Player.IsHoldingFaceRight
end

function player:Attack(id)
    if Player.IsDead then return end
    Player.Character.Attack()
    TickerUtility.Tick()
end

return player