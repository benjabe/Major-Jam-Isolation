using MoonSharp.Interpreter;
using System;
using UnityEngine;
using UnityEngine.Scripting;
using UnityEngine.UI;
using Yeeter;

[Preserve, MoonSharpUserData]
public class Character : MonoBehaviour
{
    [SerializeField] private Transform _facingIndicator = null;
    private int _health = 10;
    private CharacterType _type;
    private bool _isDead = false;

    public int Health { get => _health; private set { _health = value; OnHealthSet?.Invoke(_health); } }
    public Vector2Int Facing { get; private set; }
    public Vector2Int Position { get; set; }
    public Action<Character> OnDeath { get; set; }
    public Action<Vector2Int> OnMove { get; set; }
    public Action<int> OnHealthSet { get; set; }

    private void Start()
    {
        if (_facingIndicator == null) InGameDebug.Log("WHAT THE FUCK");
        SetFacing(0, -1);
        Position = new Vector2Int(
            Mathf.RoundToInt(transform.position.x),
            Mathf.RoundToInt(transform.position.y));
        Health = _health;
    }

    public void SetType(CharacterType type)
    {
        _type = type;
        GetComponentInChildren<Text>().text = _type.Symbol;
    }

    public bool Move(int x, int y)
    {
        if (_isDead) return false;
        bool movementWasNotBlocked = true;

        if (!FloorBuilder.IsTileTraversable(Position.x + x, Position.y + y))
        {
            movementWasNotBlocked = false;
        }
        else
        {
            Position += new Vector2Int(x, y);
        }
        transform.position = new Vector3(Position.x, Position.y, 0.0f);
        OnMove?.Invoke(Position);
        return movementWasNotBlocked;
    }
    public void SetPosition(int x, int y)
    {
        Position = new Vector2Int(x, y);
        transform.position = new Vector3(Position.x, Position.y, 0.0f);
    }

    public void SetFacing(int x, int y)
    {
        if (_isDead) return;
        Facing = new Vector2Int(x, y);
        _facingIndicator.localPosition = new Vector3(0.5f + x * 0.45f, 0.5f + y * 0.45f);
    }

    public void HealOrTakeDamage(int healthDelta)
    {
        if (_isDead) return;
        var damageIndicatorGameObject =
        ObjectBuilder.Get((int)ObjectBuilder.Instantiate("AttackDamageIndicator").Number);
        var attackDamageIndicator = damageIndicatorGameObject.GetComponent<AttackDamageIndicator>();
        attackDamageIndicator.Target = transform;
        attackDamageIndicator.HealthDelta = healthDelta;

        Health += healthDelta;
        if (Health <= 0)
        {
            Health = 0;
            // TODO: Play a death animation/sound maybe
            OnDeath?.Invoke(this);
            _isDead = true;
            Destroy(gameObject);
        }
    }

    public void Attack()
    {
        if (_isDead) return;
        // TODO: Add move selection.
        var attackPos = Position + Facing;
        var target = Floor.CurrentFloor.FindCharacterAtPosition(attackPos.x, attackPos.y);
        if (target != null)
        {
            int healthDelta = UnityEngine.Random.Range(0, -10);
            target.HealOrTakeDamage(healthDelta);
        }
    }

    public void AddOnHealthSetListener(DynValue function)
    {
        OnHealthSet += health => LuaManager.Call(function, DynValue.NewNumber(health));
    }
}
