using Tyd;

public class CharacterType
{
    public string Symbol { get; private set; }
    public string FullName { get; private set; }

    public static CharacterType FromTydTable(TydTable table)
    {
        var type = new CharacterType();
        foreach (var node in table.Nodes)
        {
            switch (node.Name)
            {
                case "symbol":
                    type.Symbol = (node as TydString).Value;
                    break;
                case "fullName":
                    type.FullName = (node as TydString).Value;
                    break;
            }
        }
        return type;
    }
}
