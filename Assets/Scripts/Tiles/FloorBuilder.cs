using MoonSharp.Interpreter;
using System.Collections.Generic;
using Tyd;
using UnityEngine;
using UnityEngine.SceneManagement;
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

    public static void NextFloor()
    {
        CurrentFloor.Clear();
        BBInput.ClearAllAxesInProfile("Dungeon");
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
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

    public Tile GetTile(int x, int y)
    {
        return CurrentFloor.GetTile(x, y);
    }

    public static void PlaceEnemy(string typeName, int x, int y)
    {
        int id = CharacterBuilder.Create(typeName);
        var go = ObjectBuilder.Get(id);
        ObjectBuilder.SetPosition(id, x, y);
        Character character = go.GetComponent<Character>();
        ObjectBuilder.AddLuaObjectComponent(id, "Enemy");
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
        var entryPosition = Floor.CurrentFloor.Entry.transform.position;
        if (Player.Character == null)
        {
            int id = CharacterBuilder.Create(CharacterBuilder.RandomType().FullName);
            var go = ObjectBuilder.Get(id);
            ObjectBuilder.SetPosition(id, entryPosition.x, entryPosition.y);
            Character character = go.GetComponent<Character>();
            go.AddComponent<PlayerController>();
            Floor.CurrentFloor.AddCharacter(character);
            Player.Character = character;
        }
        else
        {
            Player.Character.SetPosition(Mathf.RoundToInt(entryPosition.x), Mathf.RoundToInt(entryPosition.y));
        }
    }

    public static bool IsCharacterAtTraversableTile(int id)
    {
        var position = ObjectBuilder.Get(id).transform.position;
        return IsTileTraversable((int)position.x, (int)position.y);
    }

    public static bool IsTileTraversable(int x, int y)
    {
        return CurrentFloor.IsTileTraversable(x, y);
    }

    public static Character FindCharacterWithId(int id)
    {
        return CurrentFloor.FindCharacterWithId(id);
    }

    public static void BuildTileGraph()
    {
        CurrentFloor.BuildTileGraph();
    }
    public static void VisualizeTileGraph()
    {
        CurrentFloor.VisualizeTileGraph();
    }

    public List<Node<Tile>> FindPath(int startX, int startY, int goalX, int goalY)
    {
        return CurrentFloor.FindPath(startX, startY, goalX, goalY);
    }
    public List<Vector2Int> FindPathPositions(int startX, int startY, int goalX, int goalY)
    {
        return CurrentFloor.FindPathPositions(startX, startY, goalX, goalY);
    }
}
