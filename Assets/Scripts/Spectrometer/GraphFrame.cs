using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(LineRenderer))]
public class GraphFrame : MonoBehaviour
{   
    public LineRenderer lr;
    public float lineWidth = 0.04f;

    private void Reset() {
        lr = GetComponent<LineRenderer>();

        // TODO: fix this resource path
        lr.material = new(Shader.Find("Legacy Shaders/Particles/Alpha Blended Premultiply"));
        lr.useWorldSpace = false;
        lr.startWidth = lineWidth;
    }
}
