using UnityEngine;
using Yeeter;

public class PlayerController : MonoBehaviour
{
    // Start is called before the first frame update
    void Awake()
    {
        ObjectBuilder.AddLuaObjectComponent(ObjectBuilder.GetId(gameObject), "Player");
    }
}
