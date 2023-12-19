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
        calcFieldPoints2();
    }

    private void calcFieldPoints()
    {
        Vector3[] points = new Vector3[numPoints];
        float angleStep = 360f / numPoints;
        // Vector3 r_current = new Vector3(0,6371f*Mathf.Pow(10f,3f),0);
        Vector3 r_current = new Vector3(6371f*Mathf.Pow(10f,3f),0,0);
        Vector3 magneticMoment = new Vector3(0, 8 * Mathf.Pow(10f, 22f), 0);
        float vacuumPermeability = 4f*Mathf.PI*Mathf.Pow(10f,-7f);


        // float r_current = 6371f*Mathf.Pow(10f,3f);
        // float magneticMoment = 8f * Mathf.Pow(10f, 22f); 
        // float angle = 90f;


        // float magneticPotential = -magneticMoment * Mathf.Cos(angle * Mathf.Deg2Rad) / (4f*Mathf.PI * Mathf.Pow(r_current, 2f));
        // Debug.Log("magnetic potential: " + magneticPotential);
        // float radial = vacuumPermeability * magneticMoment.magnitude * 2f * Mathf.Cos(angle * Mathf.Deg2Rad) / (4f * Mathf.PI * Mathf.Pow(r_current.magnitude, 3f));
        // Debug.Log("radial component: " + radial);
        // float tangential = vacuumPermeability * magneticMoment.magnitude * Mathf.Sin(angle * Mathf.Deg2Rad) / (4f * Mathf.PI * Mathf.Pow(r_current.magnitude, 3f));
        // Debug.Log("tangential component: " + tangential);

        // Calculates the points along the ellipse
        for (int i = 0; i < numPoints; i++)
        {


            points[i] = new Vector3(r_current.x/1000000, r_current.y/1000000, 0);
            Debug.Log("point: " + points[i]);

            float angle = i * angleStep;
            Debug.Log("angle: " + angle);

            // vectorPotential = (vacuumPermeability / (4*Mathf.PI)) * (Vector3.Cross(magneticMoment, r_current) / Mathf.Pow(r_current.magnitude, 3));
            // Vector3 magneticField = vacuumPermeability/(4*Mathf.PI) * ((Vector3.Cross(3*r_current,(Vector3.Cross(magneticMoment, r_current))) / Mathf.Pow(r_current.magnitude, 5f)) - (magneticMoment / Mathf.Pow(r_current.magnitude, 3f)));
            
            // float magneticPotential = -magneticMoment * Mathf.Cos(angle * Mathf.Deg2Rad) / (4f*Mathf.PI * Mathf.Pow(r_current, 2f));
            // Debug.Log("magnetic potential: " + magneticPotential);
            float radial = vacuumPermeability * magneticMoment.magnitude * 2f * Mathf.Cos(angle * Mathf.Deg2Rad) / (4f * Mathf.PI * Mathf.Pow(r_current.magnitude, 3f));
            Debug.Log("radial component: " + radial);
            float tangential = -vacuumPermeability * magneticMoment.magnitude * Mathf.Sin(angle * Mathf.Deg2Rad) / (4f * Mathf.PI * Mathf.Pow(r_current.magnitude, 3f));
            Debug.Log("tangential component: " + tangential);
           
            // Vector3 magneticField = vacuumPermeability * (3*Vector3.Dot(magneticMoment, r_current) * r_current - magneticMoment) / Mathf.Pow(r_current.magnitude, 3);
            // Debug.Log("magentic field (B): " + magneticField);
            // Vector3 magneticUnitVector = magneticField.normalized;
            // Debug.Log("magnetic unit vector (B_hat): " + magneticUnitVector);

            /////////
            // Update r_current based on the angle and magnetic unit vector
            // float x = r_current.magnitude * Mathf.Cos(angle * Mathf.Deg2Rad);
            // float y = r_current.magnitude * Mathf.Sin(angle * Mathf.Deg2Rad);
            // float ellipseRadius = r_current.magnitude; // Assuming r_current is the current position vector magnitude

            // Vector3 newPosition = new Vector3(x, y, 0);

            // Calculate the new position in the plane of the ellipse
            // Vector3 newPosition = cosAngle * ellipseRadius * magneticUnitVector + sinAngle * ellipseRadius * Vector3.Cross(Vector3.up, magneticUnitVector);


            // Vector3 dir = new Vector3(radial, tangential, 0);
            Vector3 dir = new Vector3(tangential, radial, 0);
            dir = dir.normalized;
            Debug.Log("dir: " + dir);
            // Update r_current
            r_current = r_current + dir * 100000f;
            // r_current = r_current + magneticUnitVector * 1f;
            // r_current = r_current + magneticUnitVector;
            // r_current = newPosition;
            // Debug.Log("r_current updated: " + r_current);
        }

        fieldLinePoints = points;

        createLine1(points);
        // lr.positionCount = fieldLinePoints.Length;
        // lr.SetPositions(fieldLinePoints);
    }

    private void calcFieldPoints2()
    {
        Vector3[] points = new Vector3[numPoints];
        float angleStep = 360f / numPoints;
        // Vector3 r_current = new Vector3(0,0.5f*6371f*Mathf.Pow(10f,3f),0);
        Vector3 r_current = new Vector3(0.5f*6371f*Mathf.Pow(10f,3f),0,0);
        Vector3 magneticMoment = new Vector3(0, 8 * Mathf.Pow(10f, 22f), 0);
        float vacuumPermeability = 4f*Mathf.PI*Mathf.Pow(10f,-7f);


        // float r_current = 6371f*Mathf.Pow(10f,3f);
        // float magneticMoment = 8f * Mathf.Pow(10f, 22f); 
        // float angle = 90f;


        // float magneticPotential = -magneticMoment * Mathf.Cos(angle * Mathf.Deg2Rad) / (4f*Mathf.PI * Mathf.Pow(r_current, 2f));
        // Debug.Log("magnetic potential: " + magneticPotential);
        // float radial = vacuumPermeability * magneticMoment.magnitude * 2f * Mathf.Cos(angle * Mathf.Deg2Rad) / (4f * Mathf.PI * Mathf.Pow(r_current.magnitude, 3f));
        // Debug.Log("radial component: " + radial);
        // float tangential = vacuumPermeability * magneticMoment.magnitude * Mathf.Sin(angle * Mathf.Deg2Rad) / (4f * Mathf.PI * Mathf.Pow(r_current.magnitude, 3f));
        // Debug.Log("tangential component: " + tangential);

        // Calculates the points along the ellipse
        for (int i = 0; i < numPoints; i++)
        {


            points[i] = new Vector3(r_current.x/1000000, r_current.y/1000000, 0);
            Debug.Log("point: " + points[i]);

            float angle = i * angleStep;
            Debug.Log("angle: " + angle);

            // vectorPotential = (vacuumPermeability / (4*Mathf.PI)) * (Vector3.Cross(magneticMoment, r_current) / Mathf.Pow(r_current.magnitude, 3));
            // Vector3 magneticField = vacuumPermeability/(4*Mathf.PI) * ((Vector3.Cross(3*r_current,(Vector3.Cross(magneticMoment, r_current))) / Mathf.Pow(r_current.magnitude, 5f)) - (magneticMoment / Mathf.Pow(r_current.magnitude, 3f)));
            
            // float magneticPotential = -magneticMoment * Mathf.Cos(angle * Mathf.Deg2Rad) / (4f*Mathf.PI * Mathf.Pow(r_current, 2f));
            // Debug.Log("magnetic potential: " + magneticPotential);
            float radial = vacuumPermeability * magneticMoment.magnitude * 2f * Mathf.Cos(angle * Mathf.Deg2Rad) / (4f * Mathf.PI * Mathf.Pow(r_current.magnitude, 3f));
            Debug.Log("radial component: " + radial);
            float tangential = -vacuumPermeability * magneticMoment.magnitude * Mathf.Sin(angle * Mathf.Deg2Rad) / (4f * Mathf.PI * Mathf.Pow(r_current.magnitude, 3f));
            Debug.Log("tangential component: " + tangential);
           
            // Vector3 magneticField = vacuumPermeability * (3*Vector3.Dot(magneticMoment, r_current) * r_current - magneticMoment) / Mathf.Pow(r_current.magnitude, 3);
            // Debug.Log("magentic field (B): " + magneticField);
            // Vector3 magneticUnitVector = magneticField.normalized;
            // Debug.Log("magnetic unit vector (B_hat): " + magneticUnitVector);

            /////////
            // Update r_current based on the angle and magnetic unit vector
            // float x = r_current.magnitude * Mathf.Cos(angle * Mathf.Deg2Rad);
            // float y = r_current.magnitude * Mathf.Sin(angle * Mathf.Deg2Rad);
            // float ellipseRadius = r_current.magnitude; // Assuming r_current is the current position vector magnitude

            // Vector3 newPosition = new Vector3(x, y, 0);

            // Calculate the new position in the plane of the ellipse
            // Vector3 newPosition = cosAngle * ellipseRadius * magneticUnitVector + sinAngle * ellipseRadius * Vector3.Cross(Vector3.up, magneticUnitVector);


            // Vector3 dir = new Vector3(radial, tangential, 0);
            Vector3 dir = new Vector3(tangential, radial, 0);
            dir = dir.normalized;
            Debug.Log("dir: " + dir);
            // Update r_current
            r_current = r_current + dir * 100000f;
            // r_current = r_current + magneticUnitVector * 1f;
            // r_current = r_current + magneticUnitVector;
            // r_current = newPosition;
            // Debug.Log("r_current updated: " + r_current);
        }

        fieldLinePoints = points;

        createLine2(points);
        // lr.positionCount = fieldLinePoints.Length;
        // lr.SetPositions(fieldLinePoints);
    }

    private void createLine1(Vector3[] points)
    {
        // Create a new GameObject
        GameObject lineObject = new GameObject("Line1");

        // Attach LineRenderer component to the new GameObject
        LineRenderer lineRenderer = lineObject.AddComponent<LineRenderer>();

        // Assign the points to the LineRenderer
        lineRenderer.positionCount = points.Length;
        lineRenderer.SetPositions(points);

        lineRenderer.material = new Material(Shader.Find("Legacy Shaders/Particles/Alpha Blended Premultiply"));
        lineRenderer.loop = true;

        // Optionally, you can configure other properties of the LineRenderer
        lineRenderer.startColor = Color.red;
        lineRenderer.endColor = Color.blue;
        lineRenderer.startWidth = 0.1f;
        lineRenderer.endWidth = 0.1f;
    }

    private void createLine2(Vector3[] points)
    {
        // Create a new GameObject
        GameObject lineObject = new GameObject("Line2");

        // Attach LineRenderer component to the new GameObject
        LineRenderer lineRenderer = lineObject.AddComponent<LineRenderer>();

        // Assign the points to the LineRenderer
        lineRenderer.positionCount = points.Length;
        lineRenderer.SetPositions(points);

        lineRenderer.material = new Material(Shader.Find("Legacy Shaders/Particles/Alpha Blended Premultiply"));
        lineRenderer.loop = true;

        // Optionally, you can configure other properties of the LineRenderer
        lineRenderer.startColor = Color.green;
        lineRenderer.endColor = Color.white;
        lineRenderer.startWidth = 0.1f;
        lineRenderer.endWidth = 0.1f;
    }

}
