using MoonSharp.Interpreter;
using UnityEngine;
using UnityEngine.Scripting;

[Preserve, MoonSharpUserData]
public class Tile : MonoBehaviour
{
    public TileType Type { get; set; }
    public Vector2Int Position { get; set; }

    public bool HasProperty(string property)
    {
        return Type.HasProperty(property);
    }
}
