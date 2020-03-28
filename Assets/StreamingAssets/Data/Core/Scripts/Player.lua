local player = { characterId = 0 }

function player:Start(id)
    BBInput.AddOnAxisPressed("MoveUp", function() player:Move(id, 0, 1) end)
    BBInput.AddOnAxisPressed("MoveDown", function() player:Move(id, 0, -1) end)
    BBInput.AddOnAxisPressed("MoveLeft", function() player:Move(id, -1, 0) end)
    BBInput.AddOnAxisPressed("MoveRight", function() player:Move(id, 1, 0) end)

    BBInput.AddOnAxisPressed("FaceUp", function() player:Attack(id, 0, 1) end)
    BBInput.AddOnAxisPressed("FaceDown", function() player:Attack(id, 0, -1) end)
    BBInput.AddOnAxisPressed("FaceLeft", function() player:Attack(id, -1, 0) end)
    BBInput.AddOnAxisPressed("FaceRight", function() player:Atack(id, 1, 0) end)

    BBInput.AddOnAxisPressed("Wait", TickerUtility.Tick)
end

function player:Update(id, dt)
end

function player:Move(id, x, y)
    ObjectBuilder.Translate(id, x, y)
    if not FloorBuilder.IsCharacterAtTraversableTile(id) then
        ObjectBuilder.Translate(id, -x, -y)
    end
    TickerUtility.Tick()
end

function player:Attack(id, x, y)
    
end

return player