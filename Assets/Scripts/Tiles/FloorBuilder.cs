using MoonSharp.Interpreter;
using System.Collections.Generic;
using Tyd;
using UnityEngine;
using UnityEngine.Scripting;
using Yeeter;

[Preserve, MoonSharpUserData]
public class FloorBuilder
{
    private static Camera _mainCamera;
    private static Dictionary<string, TileType> _tileTypes;

    public static Floor CurrentFloor { get => Floor.CurrentFloor; }

    public static void LoadTileTypes()
    {
        _tileTypes = new Dictionary<string, TileType>();
        var tileNodes = StreamingAssetsDatabase.GetDef("TileTypes") as TydTable;
        foreach (var node in tileNodes.Nodes)
        {
            var tileType = TileType.FromTydTable(node as TydTable);
            _tileTypes.Add(node.Name, tileType);
        }
    }

    public static void SetTileTypeToPlaceOnMouseClick(string typeName)
    {
        var type = GetTileTypeWithName(typeName);
    }

    public static TileType GetTileTypeWithName(string name)
    {
        return _tileTypes[name];
    }

    public static void PlaceTile(string typeName, int x, int y)
    {
        Floor.CurrentFloor.PlaceTile(_tileTypes[typeName], x, y);
    }
    public static void PlaceCharacter(string typeName, int x, int y)
    {
        int id = CharacterBuilder.Create(typeName);
        var go = ObjectBuilder.Get(id);
        ObjectBuilder.SetPosition(id, x, y);
        Character character = go.GetComponent<Character>();
        Floor.CurrentFloor.AddCharacter(character);
    }

    public static void PositionCameraAtEntry()
    {
        if (Floor.CurrentFloor.Entry == null)
        {
            InGameDebug.Log(
                "<color=red>" + "FloorBuilder.PositionCameraAtEntry(): Current floor has no entry.</color>");
        }
        if (_mainCamera == null) _mainCamera = Camera.main;
        _mainCamera.transform.position =
            Floor.CurrentFloor.Entry.transform.position + Vector3.forward * _mainCamera.transform.position.z;
    }

    public static void ClearFloor()
    {
        Floor.CurrentFloor.Clear();
    }

    public static void PlacePlayer()
    {
        int id = CharacterBuilder.Create(CharacterBuilder.RandomType().FullName);
        var go = ObjectBuilder.Get(id);
        var entryPosition = Floor.CurrentFloor.Entry.transform.position;
        ObjectBuilder.SetPosition(id, entryPosition.x, entryPosition.y);
        Character character = go.GetComponent<Character>();
        go.AddComponent<PlayerController>();
        Floor.CurrentFloor.AddCharacter(character);
        Player.Character = character;
    }

    public static bool IsCharacterAtTraversableTile(int id)
    {
        var position = ObjectBuilder.Get(id).transform.position;
        return IsTileTraversable((int)position.x, (int)position.y);
    }

    public static bool IsTileTraversable(int x, int y)
    {
        return Floor.CurrentFloor.IsTileTraversable(x, y);
    }
}
