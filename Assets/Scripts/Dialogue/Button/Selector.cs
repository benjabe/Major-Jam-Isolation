using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Selector : MonoBehaviour
{
    private Button[] _buttons;

    private void Awake()
    {
        print(transform.childCount);
    }
}
