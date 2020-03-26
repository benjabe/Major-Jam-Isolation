local mainMenu = {}

function mainMenu:Start(id)
    local font = "Kenney Blocks"
    local buttonFontSize = 22

    -- title
    local title = UI.Create("Text")
    UI.SetParent(title, UIBottom)
    UI.SetAnchors(title, 0.5, 0.5, 1, 1)
    UI.SetPivot(title, 0.5, 1)
    UI.SetSizeDelta(title, 400, 72)
    UI.SetText(title, "|$ØL@t1öñ")
    UI.SetFontSize(title, 48)
    UI.SetTextAlignment(title, "MiddleCenter")
    UI.SetFont(title, font)
    UI.SetColor(title, 255, 255, 255, 255)
    -- credit
    local credit = UI.Create("Text")
    UI.SetParent(credit, UIBottom)
    UI.SetAnchors(credit, 0.5, 0.5, 1, 1)
    UI.SetPivot(credit, 0.5, 1)
    UI.SetSizeDelta(credit, 400, 100)
    UI.SetPosition(credit, 0, -60)
    UI.SetText(credit, "A GAME BY\nBENJAMIN BERGSETH\nAND\nMATHIAS HAREIDE")
    UI.SetFontSize(credit, 16)
    UI.SetTextAlignment(credit, "MiddleCenter")
    UI.SetFont(credit, font)
    UI.SetColor(credit, 255, 255, 255, 255)
    -- buttons --
    local buttonParent = UI.Create("SpacedVerticalLayoutGroup")
    UI.SetParent(buttonParent, UIMiddle)
    UI.SetAnchors(buttonParent, 0.5, 0.5, 0.5, 0.5)
    UI.SetPosition(buttonParent, 0, 0)
    UI.SetPivot(buttonParent, 0.5, 0.5)
    UI.SetSizeDelta(buttonParent, 150, 175)
    -- new game
    local newGameButton = UI.Create("Button")
    UI.SetParent(newGameButton, buttonParent)
    UI.SetText(newGameButton, "NEW GAME")
    UI.SetFont(newGameButton, font)
    UI.SetFontSize(newGameButton, buttonFontSize)
    UI.AddOnClick(newGameButton, function()
        SoundManager.PlayEffect("Effects.ButtonClick")
        SceneManager.LoadScene(2)
        Console.Log("New game started!")
    end)
    -- settings
    local settingsButton = UI.Create("Button")
    UI.SetParent(settingsButton, buttonParent)
    UI.SetText(settingsButton, "SETTINGS")
    UI.SetFont(settingsButton, font)
    UI.SetFontSize(settingsButton, buttonFontSize)
    UI.AddOnClick(settingsButton, function()
        -- todo: open settings
        SoundManager.PlayEffect("Effects.ButtonClick")
        Console.Log("Settings menu opened.")
    end)
    -- quit
    local quitButton = UI.Create("Button")
    UI.SetParent(quitButton, buttonParent)
    UI.SetText(quitButton, "QUIT")
    UI.SetFont(quitButton, font)
    UI.SetFontSize(quitButton, buttonFontSize)
    UI.AddOnClick(quitButton, function()
        SoundManager.PlayEffect("Effects.ButtonClick")
        Console.Log("Bye bye ^^!")
        Application.Quit()
    end)

end

function mainMenu:Update(id, dt)
end

return mainMenu