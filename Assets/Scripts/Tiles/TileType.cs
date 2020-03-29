using System.Collections.Generic;
using Tyd;
using UnityEngine;
using UnityEngine.Scripting;
using Yeeter;

[Preserve, MoonSharUserData]
public class TileType
{
    private List<string> _properties = new List<string>();

    public Sprite Sprite { get; set; }
    public string Name { get; set; }
    public string TextureName { get; set; }
    public bool IsTraversable { get => _properties.Contains("Traversable"); }
    public bool IsExit { get => _properties.Contains("Exit"); }

    public static TileType FromTydTable(TydTable table)
    {
        var tileType = new TileType();
        tileType.Name = table.Name;
        foreach (var node in table.Nodes)
        {
            if (node.Name == "Properties")
            {
                foreach (var propertyNode in (node as TydList).Nodes)
                {
                    tileType._properties.Add((propertyNode as TydString).Value);
                }
            }
            else if (node.Name == "texture")
            {
                tileType.TextureName = (node as TydString).Value;
            }
        }
        var texture = StreamingAssetsDatabase.GetTexture(tileType.TextureName);
        tileType.Sprite = Sprite.Create(
            texture, new Rect(0, 0, texture.width, texture.height), new Vector2(0, 0), texture.width
        );
        return tileType;
    }

    public bool HasProperty(string property)
    {
        return _properties.Contains(property);
    }
}