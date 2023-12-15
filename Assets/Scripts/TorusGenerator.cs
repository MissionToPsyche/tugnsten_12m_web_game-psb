using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TorusGenerator : MonoBehaviour
{
    private Vector3[] fieldLinePoints;
    private int numPoints = 200;
    public LineRenderer lr;

    void Start()
    {
        calcFieldPoints();
    }

    private void calcFieldPoints()
    {
        Vector3[] points = new Vector3[numPoints];
        // float angleStep = 360f / numPoints;
        Vector3 r_current = new Vector3(0,100,0);
        Vector3 magneticMoment = new Vector3(2 * Mathf.Pow(10f, 14f), 0, 0);
        float vacuumPermeability = 4*Mathf.PI*Mathf.Pow(10f,-7f);

        // Calculates the points along the ellipse
        for (int i = 0; i < numPoints; i++)
        {
            points[i] = r_current;
            Debug.Log("point: " + points[i]);

            // float angle = i * angleStep;

            // vectorPotential = (vacuumPermeability / (4*Mathf.PI)) * (Vector3.Cross(magneticMoment, r_current) / Mathf.Pow(r_current.magnitude, 3));
            Vector3 magneticField = vacuumPermeability/(4*Mathf.PI) * ((Vector3.Cross(3*r_current,(Vector3.Cross(magneticMoment, r_current))) / Mathf.Pow(r_current.magnitude, 5f)) - (magneticMoment / Mathf.Pow(r_current.magnitude, 3f)));
            Debug.Log("magentic field (B): " + magneticField);
            Vector3 magneticUnitVector = magneticField.normalized;
            Debug.Log("magnetic unit vector (B_hat): " + magneticUnitVector);

            /////////
            // Update r_current based on the angle and magnetic unit vector
            // float x = r_current.magnitude * Mathf.Cos(angle * Mathf.Deg2Rad);
            // float y = r_current.magnitude * Mathf.Sin(angle * Mathf.Deg2Rad);
            // float ellipseRadius = r_current.magnitude; // Assuming r_current is the current position vector magnitude

            // Vector3 newPosition = new Vector3(x, y, 0);

            // Calculate the new position in the plane of the ellipse
            // Vector3 newPosition = cosAngle * ellipseRadius * magneticUnitVector + sinAngle * ellipseRadius * Vector3.Cross(Vector3.up, magneticUnitVector);

            // Update r_current
            r_current = r_current + magneticUnitVector * 10f;
            // r_current = r_current + magneticUnitVector;
            // r_current = newPosition;
            Debug.Log("r_current updated: " + r_current);
        }

        fieldLinePoints = points;

        lr.positionCount = fieldLinePoints.Length;
        lr.SetPositions(fieldLinePoints);
    }



}
