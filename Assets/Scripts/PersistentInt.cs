using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu]
public class PersistentInt : ScriptableObject
{
    [SerializeField]
    private int _number = -1;

    private void OnEnable() {
        _number = -1;
    }

    public int Num
    {
        get { return _number; }
        set { _number = value; }
    }
}
