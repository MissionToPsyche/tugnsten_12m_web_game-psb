using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OrbitDataGenerator : MonoBehaviour
{
    private struct SpacecraftState
    {
        public Vector2 position;
        public Vector2 velocity;
    }

    private struct SpacecraftStateRange
    {
        public Vector2 position;
        public Vector2 minVelocity;
        public Vector2 maxVelocity;
    }

    private struct Orbit
    {
        public float periapsisDistance;
        public float apoapsisDistance;
        public float rotation;
    }

    private readonly Orbit[] missionOrbits = {
        new() {periapsisDistance = 6f, apoapsisDistance = 6.01f, rotation = 0f},    // Orbit A
        new() {periapsisDistance = 3.4f, apoapsisDistance = 3.41f, rotation = 0f},  // Orbit B
        new() {periapsisDistance = 1.3f, apoapsisDistance = 1.31f, rotation = 0f},  // Orbit C
        new() {periapsisDistance = 0.95f, apoapsisDistance = 2.1f, rotation = 90f}, // Orbit D
    };

    // Spacecraft states that place it in each mission orbit
    private readonly SpacecraftState[] missionStates = {
        new() {position = new Vector2(6, 5.5f), velocity = new Vector2(-1.4f, 0f)},  // Capture
        new() {position = new Vector2(6.01f, 0), velocity = new Vector2(0, 0.91f)},  // Orbit A
        new() {position = new Vector2(3.41f, 0), velocity = new Vector2(0, 1.21f)},  // Orbit B
        new() {position = new Vector2(1.31f, 0), velocity = new Vector2(0, 1.95f)},  // Orbit C
        new() {position = new Vector2(0, 2.1f), velocity = new Vector2(-1.23f, 0)},  // Orbit D 
    };

    private readonly SpacecraftStateRange[] randomStates = {
        // Low start
        new() {
            position = new Vector2(1.5f, 0),
            minVelocity = new Vector2(0, 1.6f),
            maxVelocity = new Vector2(0, 2.31f)
        },
        new() {
            position = new Vector2(-1.5f, 0),
            minVelocity = new Vector2(0, -1.6f),
            maxVelocity = new Vector2(0, -2.31f)
        },
        new() {
            position = new Vector2(0, 1.5f),
            minVelocity = new Vector2(1.6f, 0),
            maxVelocity = new Vector2(2.31f, 0)
        },
        new() {
            position = new Vector2(0, -1.5f),
            minVelocity = new Vector2(-1.6f, 0),
            maxVelocity = new Vector2(-2.31f, 0)
        },
        // Medium start
        new() {
            position = new Vector2(3f, 0),
            minVelocity = new Vector2(0, 0.9f),
            maxVelocity = new Vector2(0, 1.5f)
        },
        new() {
            position = new Vector2(-3f, 0),
            minVelocity = new Vector2(0, -0.9f),
            maxVelocity = new Vector2(0, -1.5f)
        },
        new() {
            position = new Vector2(0, 3f),
            minVelocity = new Vector2(0.9f, 0),
            maxVelocity = new Vector2(1.5f, 0)
        },
        new() {
            position = new Vector2(0, -3f),
            minVelocity = new Vector2(-0.9f, 0),
            maxVelocity = new Vector2(-1.5f, 0)
        },
        // High start
        new() {
            position = new Vector2(5f, 0),
            minVelocity = new Vector2(0, 0.6f),
            maxVelocity = new Vector2(0, 1.05f)
        },
        new() {
            position = new Vector2(-5f, 0),
            minVelocity = new Vector2(0, -0.6f),
            maxVelocity = new Vector2(0, -1.05f)
        },
        new() {
            position = new Vector2(0, 5f),
            minVelocity = new Vector2(0.6f, 0),
            maxVelocity = new Vector2(1.05f, 0)
        },
        new() {
            position = new Vector2(0, -5f),
            minVelocity = new Vector2(-0.6f, 0),
            maxVelocity = new Vector2(-1.05f, 0)
        },
    };

    // 0 for capture state, 1 - 4 for mission orbits, -1 for random
    public (Vector2, Vector2) GetInitialState(int index)
    {
        SpacecraftState state;

        if (index >= 0 && index < missionOrbits.Length)
        {
            state = missionStates[index];
        }
        else
        {
            SpacecraftStateRange parameters = randomStates[Random.Range(0, randomStates.Length)];
            
            float xVel = Random.Range(parameters.minVelocity.x, parameters.maxVelocity.x);
            float yVel = Random.Range(parameters.minVelocity.y, parameters.maxVelocity.y);

            state = new()
            {
                position = parameters.position,
                velocity = new(xVel, yVel)
            };
        }

        return(state.position, state.velocity);
    }

    // 0 - 3 for mission orbits, -1 for random
    public (float, float, float) GetTargetOrbit(int index)
    {
        Orbit orbit;

        if (index >= 0 && index < missionOrbits.Length)
        {
            orbit = missionOrbits[index];
        }
        else
        {
            orbit = new()
            {
                periapsisDistance = Random.Range(0.8f, 5f)
            };

            orbit.apoapsisDistance = Random.Range(orbit.periapsisDistance + 0.01f, 6f);

            orbit.rotation = Random.Range(0, 360);
        }

        return (
            orbit.periapsisDistance,
            orbit.apoapsisDistance,
            orbit.rotation
        );
    }
}
