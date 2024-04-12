using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu]
public class Scorecard : ScriptableObject
{
    [SerializeField]
    private int _magnetometerScore = 0;
    [SerializeField]
    private int _imagerScore = 0;
    [SerializeField]
    private int _gravityScore = 0;
    [SerializeField]
    private int _spectrometerScore = 0;
    [SerializeField]
    private int[] _orbitScore = {0, 0, 0, 0};

    private void OnEnable() {
        _magnetometerScore = 0;
        _imagerScore = 0;
        _gravityScore = 0;
        _spectrometerScore = 0;
        for (int i = 0; i < _orbitScore.Length; i++)
        {
            _orbitScore[i] = 0;
        }
    }

    public int MagnetometerScore
    {
        get { return _magnetometerScore; }
        set { _magnetometerScore = value; }
    }

    public int ImagerScore
    {
        get { return _imagerScore; }
        set { _imagerScore = value; }
    }

    public int GravityScore
    {
        get { return _gravityScore; }
        set { _gravityScore = value; }
    }

    public int SpectrometerScore
    {
        get { return _spectrometerScore; }
        set { _spectrometerScore = value; }
    }

    public int[] OrbitScore
    {
        get { return _orbitScore; }
        set { _orbitScore = value; }
    }
}
