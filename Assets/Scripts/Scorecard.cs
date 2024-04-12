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
    private int _orbitScore1 = 0;
    [SerializeField]
    private int _orbitScore2 = 0;
    [SerializeField]
    private int _orbitScore3 = 0;
    [SerializeField]
    private int _orbitScore4 = 0;

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

    public int OrbitScore1
    {
        get { return _orbitScore1; }
        set { _orbitScore1 = value; }
    }

    public int OrbitScore2
    {
        get { return _orbitScore2; }
        set { _orbitScore2 = value; }
    }

    public int OrbitScore3
    {
        get { return _orbitScore3; }
        set { _orbitScore3 = value; }
    }

    public int OrbitScore4
    {
        get { return _orbitScore4; }
        set { _orbitScore4 = value; }
    }
}
