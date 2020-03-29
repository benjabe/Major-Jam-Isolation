using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Yeeter;

public class Floor : MonoBehaviour
{
    private static Dictionary<Tile, Node<Tile>> _tileGraph;
    public static Floor CurrentFloor { get; private set; }
    public Tile Entry { get; private set; }
    private Dictionary<Vector2Int, Tile> _tiles = new Dictionary<Vector2Int, Tile>();
    private List<Character> _characters = new List<Character>();

    public static int Index { get; private set; } = 0;

    private void OnDestroy()
    {
        if (CurrentFloor == this) CurrentFloor = null;
    }

    private void Awake()
    {
        CurrentFloor = this;
        Index++;
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
        _tiles[vec].Position = vec;
    }

    public Tile GetTile(int x, int y)
    {
        return _tiles[new Vector2Int(x, y)];
    }

    public void AddCharacter(Character character)
    {
        _characters.Add(character);
        character.GetComponent<LuaObjectComponent>().OnDestroyed += c => _characters.Remove(character);
    }

    public void Clear()
    {
        foreach (var tile in _tiles.Values.ToList())
        {
            DestroyImmediate(tile.gameObject);
        }
        foreach (var character in _characters.ToList())
        {
            DestroyImmediate(character.gameObject);
        }
        _tileGraph = null;
        Entry = null;
        _tiles = new Dictionary<Vector2Int, Tile>();
        _characters = new List<Character>();
    }

    public bool IsTileTraversable(int x, int y)
    {
        foreach (var character in _characters)
        {
            if (character.Position.x == x && character.Position.y == y)
            {
                return false;
            }
        }
        return _tiles[new Vector2Int(x, y)].Type.IsTraversable;
    }

    public Character FindCharacterAtPosition(int x, int y)
    {
        foreach (var character in _characters)
        {
            if (character.Position.x == x && character.Position.y == y)
            {
                return character;
            }
        }
        return null;
    }

    public Character FindCharacterWithId(int id)
    {
        // TODO: Optimise this probably.
        foreach (var character in _characters)
        {
            if (ObjectBuilder.GetId(character.gameObject) == id)
            {
                return character;
            }
        }
        return null;
    }

    public void BuildTileGraph()
    {
        _tileGraph = new Dictionary<Tile, Node<Tile>>();
        foreach (var pair in _tiles)
        {
            var position = pair.Key;
            var tile = pair.Value;
            Node<Tile> node = null;
            if (_tileGraph.ContainsKey(tile))
            {
                node = _tileGraph[tile];
            }
            else
            {
                node = new Node<Tile>();
                _tileGraph[tile] = node;
            }
            node.Value = tile;
            for (int x = -1; x <= 1; x++)
            {
                for (int y = -1; y <= 1; y++)
                {
                    var neighborPosition = position + new Vector2Int(x, y);
                    if (_tiles.ContainsKey(neighborPosition))
                    {
                        var neighbor = _tiles[neighborPosition];
                        var edge = new Edge<Tile>();
                        edge.Cost = neighbor.Type.IsTraversable ? 1 : Mathf.Infinity;
                        edge.From = node;
                        if (_tileGraph.ContainsKey(neighbor))
                        {
                            edge.To = _tileGraph[neighbor];
                        }
                        else
                        {
                            edge.To = new Node<Tile>();
                            _tileGraph[neighbor] = edge.To;
                        }
                        edge.To.Value = _tiles[neighborPosition];
                        node.Edges.Add(edge);
                    }
                }
            }
        }
    }

    public void VisualizeTileGraph()
    {
        foreach (var node in _tileGraph.Values)
        {
            foreach (var edge in node.Edges)
            {
                var neighbor = edge.To;
                var go = Instantiate(new GameObject(), transform);
                var arrow = go.AddComponent<TileGraphArrow>();
                arrow.From = node.Value;
                arrow.To = neighbor.Value;
            }
        }
    }

    public List<Node<Tile>> FindPath(int startX, int startY, int goalX, int goalY)
    {
        var startPos = new Vector2Int(startX, startY);
        var goalPos = new Vector2Int(goalX, goalY);
        var start = _tileGraph[_tiles[startPos]];
        var goal = _tileGraph[_tiles[goalPos]];
        return AStar.FindPath(
            start,
            goal,
            node =>
            {
                return Mathf.Abs(node.Value.Position.x - goalPos.x) + Mathf.Abs(node.Value.Position.y - goalPos.y);
            }
        );
    }
    public List<Vector2Int> FindPathPositions(int startX, int startY, int goalX, int goalY)
    {
        var startPos = new Vector2Int(startX, startY);
        var goalPos = new Vector2Int(goalX, goalY);
        var start = _tileGraph[_tiles[startPos]];
        var goal = _tileGraph[_tiles[goalPos]];
        var nodes = AStar.FindPath(
            start,
            goal,
            node =>
            {
                return Mathf.Abs(node.Value.Position.x - goalPos.x) + Mathf.Abs(node.Value.Position.y - goalPos.y);
            }
        );
        var positions = new List<Vector2Int>();
        foreach (var node in nodes)
        {
            positions.Add(node.Value.Position);
        }
        return positions;
    }
}
