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

    private readonly SpacecraftState[] startStates = {
        new() {position = new Vector2(0, -2.2f), velocity = new Vector2(1.55f, 0)},
        new() {position = new Vector2(0, -2.2f), velocity = new Vector2(1.1f, 0)},
        new() {position = new Vector2(0, -2.2f), velocity = new Vector2(1.1f, 1.1f)},
        new() {position = new Vector2(0, -2.2f), velocity = new Vector2(1.5f, 0.75f)},
    };

    // Returns (position, velocity)
    public (Vector3, Vector3) GetInitialState()
    {
        // note: int random generator excludes upper bound
        SpacecraftState initialState = startStates[Random.Range(0, startStates.Length)];

        return (
            new(initialState.position.x, initialState.position.y, 0),
            new(initialState.velocity.x, initialState.velocity.y, 0)
        );
    }
}
