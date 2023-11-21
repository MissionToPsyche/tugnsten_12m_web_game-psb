using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameController : MonoBehaviour
{
    public Orbiter spacecraft;
    public Orbit targetOrbit;

    public const float altitudeTolerance = 0.25f;
    public const float rotationTolerance = 2f;

    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void FixedUpdate()
    {
        if (spacecraft.orbit.rotation - targetOrbit.rotation < rotationTolerance
            && spacecraft.orbit.periapsisDistance - targetOrbit.periapsisDistance < altitudeTolerance
            && spacecraft.orbit.apoapsisDistance - targetOrbit.apoapsisDistance < altitudeTolerance)
        {
            // Win state
        }

        if (spacecraft.orbit.hasCrashed || spacecraft.orbit.hasEscaped)
        {
            // Fail state
        }
    }
}
