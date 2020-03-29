using UnityEngine;

public class PlayerFollowCamera : MonoBehaviour
{
    private Vector3 _targetPosition;

    private void Start()
    {
        transform.position = transform.position;
        Player.Character.OnMove += SetTargetPosition;
        _targetPosition = Player.Character.transform.position;
    }

    private void Update()
    {
        MoveTowardTargetPosition();
    }

    private void SetTargetPosition(Vector2Int targetPosition)
    {
        _targetPosition = new Vector3(targetPosition.x, targetPosition.y);
    }

    private void MoveTowardTargetPosition()
    {
        var distance = transform.position - _targetPosition;
        transform.position = new Vector3(
            Mathf.Lerp(
                transform.position.x,
                _targetPosition.x,
                Mathf.Max (1.0f, Mathf.Abs(distance.x)) * 20.0f * Time.deltaTime),
            Mathf.Lerp(
                transform.position.y,
                _targetPosition.y,
                Mathf.Max(1.0f, Mathf.Abs(distance.y)) * 20.0f * Time.deltaTime),
            transform.position.z
        );
    }
}
