using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameController : MonoBehaviour
{
    private bool gameRunning = true;
    public Orbiter spacecraft;
    public Orbit targetOrbit;

    public const float altitudeTolerance = 0.1f;
    public const float rotationTolerance = 4f;
    public const float winTimeRequired = 2f;
    private float winTimer = 0f;

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

    void Start()
    {
        InitializeGame();
    }

    void FixedUpdate()
    {
        if (gameRunning)
        {
            // Checks if the current orbit's rotation and apsis distances are
            // within tolerance of the target orbit.
            bool winState = Mathf.Abs(spacecraft.orbit.rotation - targetOrbit.rotation) < rotationTolerance
                            && Mathf.Abs(spacecraft.orbit.periapsisDistance - targetOrbit.periapsisDistance) < altitudeTolerance
                            && Mathf.Abs(spacecraft.orbit.apoapsisDistance - targetOrbit.apoapsisDistance) < altitudeTolerance;

            // If the game is in the win state, starts counting up to
            // winTimeRequired. If the game leaves the win state, the timer is
            // reset.
            if (winState)
            {
                winTimer += Time.fixedDeltaTime;
            }
            else
            {
                winTimer = 0f;
            }

            if (spacecraft.orbit.hasCrashed || spacecraft.orbit.hasEscaped)
            {
                gameRunning = false;
                Debug.Log("Fail");
            }

            // If the game has remained in the win state for winTimeRequired,
            // the player has actually won.
            if (winTimer >= winTimeRequired)
            {
                gameRunning = false;
                Debug.Log("Win");
            }
        }
    }

    void InitializeGame()
    {
        SpacecraftState initialState = startStates[Random.Range(0, startStates.Length - 1)];

        spacecraft.transform.position = new Vector3(initialState.position.x, initialState.position.y, 0);
        spacecraft.initialVelocity = new Vector3(initialState.velocity.x, initialState.velocity.y, 0);
    }
}
