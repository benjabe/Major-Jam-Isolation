local player = { characterId = 0 }

function player:Start(id)
    BBInput.AddOnAxisPressed("MoveUp", function() player:Move(id, 0, 1) end)
    BBInput.AddOnAxisPressed("MoveDown", function() player:Move(id, 0, -1) end)
    BBInput.AddOnAxisPressed("MoveLeft", function() player:Move(id, -1, 0) end)
    BBInput.AddOnAxisPressed("MoveRight", function() player:Move(id, 1, 0) end)

    BBInput.AddOnAxisPressed("FaceUp", function() player:Face(id, 0, 1) end)
    BBInput.AddOnAxisPressed("FaceDown", function() player:Face(id, 0, -1) end)
    BBInput.AddOnAxisPressed("FaceLeft", function() player:Face(id, -1, 0) end)
    BBInput.AddOnAxisPressed("FaceRight", function() player:Face(id, 1, 0) end)

    BBInput.AddOnAxisPressed("Attack", function() player:Attack(id) end)

    BBInput.AddOnAxisPressed("Wait", TickerUtility.Tick)
end

function player:Update(id, dt)
end

function player:Move(id, x, y)
    Player.Character.Move(x, y)
    player:Face(id, x, y)
    TickerUtility.Tick()
end

function player:Face(id, x, y)
    Player.Character.SetFacing(x, y)
end

function player:Attack(id)
    Player.Character.Attack()
    TickerUtility.Tick()
end

return player