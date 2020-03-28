using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Yeeter;

public class Floor : MonoBehaviour
{
    public static Floor CurrentFloor { get; private set; }
    public Tile Entry { get; private set; }
    private Dictionary<Vector2Int, Tile> _tiles = new Dictionary<Vector2Int, Tile>();
    private List<Character> _characters = new List<Character>();

    private void OnDestroy()
    {
        if (CurrentFloor == this) CurrentFloor = null;
    }

    private void Awake()
    {
        CurrentFloor = this;
        GetComponent<LuaObjectComponent>().Load("Floor");
    }

    public void PlaceTile(TileType type, int x, int y)
    {
        var vec = new Vector2Int(x, y);
        if (_tiles.ContainsKey(vec))
        {
            Destroy(_tiles[vec].gameObject);
        }
        int tileId = (int)ObjectBuilder.Create(x, y).Number;
        var go = ObjectBuilder.Get(tileId);
        go.transform.parent = transform;
        go.AddComponent<SpriteRenderer>().sprite = type.Sprite;
        go.name = x + "_" + y + "_" + type.Name;
        _tiles[vec] = go.AddComponent<Tile>();
        _tiles[vec].Type = type;
        if (type.Name == "Entry") Entry = go.GetComponent<Tile>();
    }

    public void AddCharacter(Character character)
    {
        _characters.Add(character);
    }

    public void Clear()
    {
        foreach (var tile in _tiles.Values)
        {
            DestroyImmediate(tile.gameObject);
        }
        foreach (var character in _characters)
        {
            DestroyImmediate(character.gameObject);
        }
        _tiles = new Dictionary<Vector2Int, Tile>();
        _characters = new List<Character>();
    }

    public bool IsTileTraversable(int x, int y)
    {
        return _tiles[new Vector2Int(x, y)].Type.Traversable;
    }
}
