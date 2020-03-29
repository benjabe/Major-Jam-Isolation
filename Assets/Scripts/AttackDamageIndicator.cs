using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class AttackDamageIndicator : MonoBehaviour
{
    private static Dictionary<Transform, List<AttackDamageIndicator>> _targetToIndicators
        = new Dictionary<Transform, List<AttackDamageIndicator>>();

    [SerializeField] private Text _text = null;
    private float _timeSinceAwake = 0.0f;

    public Transform Target { get; set; }
    public int HealthDelta { get; set; }

    private void Start()
    {
        AddTargetToIndicatorsEntry(Target, this);
        // TODO: Order this with the newest entry first.
        // Probably make a vertical layout group to act as a parent for the indicators with the same target.
        transform.position = Target.position + new Vector3(0.0f, 0.37f) * _targetToIndicators[Target].Count;
        _text.text = "";
        if (HealthDelta > 0)
        {
            _text.text += "+";
            _text.color = Color.green;
        }
        else if (HealthDelta < 0)
        {
            _text.color = Color.red;
        }
        else
        {
            _text.color = Color.white;
        }
        _text.text += HealthDelta;
    }

    private void Update()
    {
        _timeSinceAwake += Time.deltaTime;
        if (_timeSinceAwake > 0.8f)
        {
            if (Target != null && _targetToIndicators.ContainsKey(Target))
            {
                RemoveTargetToIndicatorsEntry(Target, this);
            }
            Destroy(gameObject);
        }
    }

    private static void AddTargetToIndicatorsEntry(Transform target, AttackDamageIndicator indicator)
    {
        if (!_targetToIndicators.ContainsKey(target))
        {
            _targetToIndicators[target] = new List<AttackDamageIndicator>();
            var character = target.GetComponent<Character>();
            if (character != null)
            {
                character.OnDeath += RemoveCharacterTarget;
            }
        }
        _targetToIndicators[target].Add(indicator);
    }
    private static void RemoveTargetToIndicatorsEntry(Transform target, AttackDamageIndicator indicator)
    {
        _targetToIndicators[target].Remove(indicator);
        if (_targetToIndicators[target].Count == 0)
        {
            var character = target.GetComponent<Character>();
            if (character != null)
            {
                RemoveCharacterTarget(character);
            }

        }
    }

    private static void RemoveCharacterTarget(Character character)
    {
        _targetToIndicators.Remove(character.transform);
        character.OnDeath -= RemoveCharacterTarget;
    }
}
