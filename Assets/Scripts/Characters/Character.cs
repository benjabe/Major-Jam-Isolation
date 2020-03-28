using MoonSharp.Interpreter;
using UnityEngine;
using UnityEngine.Scripting;
using UnityEngine.UI;
using Yeeter;

[Preserve, MoonSharpUserData]
public class Character : MonoBehaviour
{
    [SerializeField] private Transform _facingIndicator = null;
    private Vector2Int _facing;
    private CharacterType _type;

    public Vector2Int Position { get; set; }

    private void Start()
    {
        if (_facingIndicator == null) InGameDebug.Log("WHAT THE FUCK");
        SetFacing(0, -1);
        Position = new Vector2Int(
            Mathf.RoundToInt(transform.position.x),
            Mathf.RoundToInt(transform.position.y));
    }

    public void SetType(CharacterType type)
    {
        _type = type;
        GetComponentInChildren<Text>().text = _type.Symbol;
    }

    public void Move(int x, int y)
    {
        Position += new Vector2Int(x, y);

        if (!FloorBuilder.IsTileTraversable(Position.x, Position.y))
        {
            Position -= new Vector2Int(x, y);
        }
        transform.position = new Vector3(Position.x, Position.y, 0.0f);
    }

    public void SetFacing(int x, int y)
    {
        _facing = new Vector2Int(x, y);
        _facingIndicator.localPosition = new Vector3(0.5f + x * 0.45f, 0.5f + y * 0.45f);
    }

    public void Attack()
    {
        // TODO: Add move selection.
        var attackPos = Position + _facing;
        var target = Floor.CurrentFloor.FindCharacterAtPosition(attackPos.x, attackPos.y);
        if (target != null)
        {
            Destroy(target.gameObject);
        }
    }
}
