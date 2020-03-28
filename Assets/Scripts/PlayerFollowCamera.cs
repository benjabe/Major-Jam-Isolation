using UnityEngine;

public class PlayerFollowCamera : MonoBehaviour
{
    void Awake()
    {
        transform.parent = Player.Character.transform;
        transform.localPosition = Vector3.zero + Vector3.forward * -10.0f;
    }
}
