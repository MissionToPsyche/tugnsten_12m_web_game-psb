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
        // calcFieldPoints();
        calcFieldPoints2();
        // calcFieldPoints3();
        // calcFieldPoints4();
    }

    private void calcFieldPoints()
    {
        Vector3[] points = new Vector3[numPoints];
        float angleStep = 360f / numPoints;
        // Vector3 r = new Vector3(0,6371f*Mathf.Pow(10f,3f),0);
        // Vector3 r = new Vector3(6371f*Mathf.Pow(10f,3f),0,0);
        // Vector3 magneticMoment = new Vector3(0, 8 * Mathf.Pow(10f, 22f), 0);
        Vector3 r = new Vector3(2000f,0,0);
        Vector3 magneticMoment = new Vector3(0, 2 * Mathf.Pow(10f, 14f), 0);
        float vacuumPermeability = 4f*Mathf.PI*Mathf.Pow(10f,-7f);
        Vector3 ellipseCenter = new Vector3(1200f/1000f, 0, 0);


        // float r = 6371f*Mathf.Pow(10f,3f);
        // float magneticMoment = 8f * Mathf.Pow(10f, 22f); 
        // float angle = 90f;


        // float magneticPotential = -magneticMoment * Mathf.Cos(angle * Mathf.Deg2Rad) / (4f*Mathf.PI * Mathf.Pow(r, 2f));
        // Debug.Log("magnetic potential: " + magneticPotential);
        // float radial = vacuumPermeability * magneticMoment.magnitude * 2f * Mathf.Cos(angle * Mathf.Deg2Rad) / (4f * Mathf.PI * Mathf.Pow(r.magnitude, 3f));
        // Debug.Log("radial component: " + radial);
        // float tangential = vacuumPermeability * magneticMoment.magnitude * Mathf.Sin(angle * Mathf.Deg2Rad) / (4f * Mathf.PI * Mathf.Pow(r.magnitude, 3f));
        // Debug.Log("tangential component: " + tangential);

        // Calculates the points along the ellipse
        for (int i = 0; i < numPoints; i++)
        {


            points[i] = new Vector3(r.x/1000f, r.y/1000f, 0);
            Debug.Log("point: " + points[i]);

            float angle = i * angleStep;
            Debug.Log("angle: " + angle);

            // vectorPotential = (vacuumPermeability / (4*Mathf.PI)) * (Vector3.Cross(magneticMoment, r) / Mathf.Pow(r.magnitude, 3));
            // Vector3 magneticField = vacuumPermeability/(4*Mathf.PI) * ((Vector3.Cross(3*r,(Vector3.Cross(magneticMoment, r))) / Mathf.Pow(r.magnitude, 5f)) - (magneticMoment / Mathf.Pow(r.magnitude, 3f)));
            
            // float magneticPotential = -magneticMoment * Mathf.Cos(angle * Mathf.Deg2Rad) / (4f*Mathf.PI * Mathf.Pow(r, 2f));
            // Debug.Log("magnetic potential: " + magneticPotential);

            Debug.Log("r: " + r);
            Debug.Log("r mag: " + r.magnitude);



            float radial = -vacuumPermeability * magneticMoment.magnitude * 2f * Mathf.Sin(angle * Mathf.Deg2Rad) / (4f * Mathf.PI * Mathf.Pow(r.magnitude, 3f));
            float tangential = vacuumPermeability * magneticMoment.magnitude * Mathf.Cos(angle * Mathf.Deg2Rad) / (4f * Mathf.PI * Mathf.Pow(r.magnitude, 3f));
            Debug.Log("radial component: " + radial);
            Debug.Log("tangential component: " + tangential);

            // float x = radial * Mathf.Cos(angle * Mathf.Deg2Rad) + tangential * Mathf.Cos((angle + Mathf.PI/2f) * Mathf.Deg2Rad);
            // float y = radial * Mathf.Sin(angle * Mathf.Deg2Rad) + tangential * Mathf.Sin((angle + Mathf.PI/2f) * Mathf.Deg2Rad);
            // Debug.Log("x: " + x);
            // Debug.Log("y: " + y);


            Vector3 radialVector = new Vector3(radial * Mathf.Cos(angle * Mathf.Deg2Rad), radial * Mathf.Sin(angle * Mathf.Deg2Rad), 0f);
            Vector3 tangentialVector = new Vector3(-tangential * Mathf.Sin(angle * Mathf.Deg2Rad), tangential * Mathf.Cos(angle * Mathf.Deg2Rad), 0);
            Debug.Log("radial component adjusted: " + radialVector);
            Debug.Log("tangential component adjusted: " + tangentialVector);


            // float x2 = radialVector.magnitude * Mathf.Cos(angle * Mathf.Deg2Rad) + tangentialVector * Mathf.Cos((angle + Mathf.PI/2f) * Mathf.Deg2Rad);
            // float y2 = radialVector.magnitude * Mathf.Sin(angle * Mathf.Deg2Rad) + tangentialVector * Mathf.Sin((angle + Mathf.PI/2f) * Mathf.Deg2Rad);
            // Debug.Log("x2: " + x2);
            // Debug.Log("y2: " + y2);


            
            
           
            // Vector3 magneticField = vacuumPermeability * (3*Vector3.Dot(magneticMoment, r) * r - magneticMoment) / Mathf.Pow(r.magnitude, 3);
            // Debug.Log("magentic field (B): " + magneticField);
            // Vector3 magneticUnitVector = magneticField.normalized;
            // Debug.Log("magnetic unit vector (B_hat): " + magneticUnitVector);

            /////////
            // Update r based on the angle and magnetic unit vector
            // float x = r.magnitude * Mathf.Cos(angle * Mathf.Deg2Rad);
            // float y = r.magnitude * Mathf.Sin(angle * Mathf.Deg2Rad);
            // float ellipseRadius = r.magnitude; // Assuming r is the current position vector magnitude

            // Vector3 newPosition = new Vector3(x, y, 0);

            // Calculate the new position in the plane of the ellipse
            // Vector3 newPosition = cosAngle * ellipseRadius * magneticUnitVector + sinAngle * ellipseRadius * Vector3.Cross(Vector3.up, magneticUnitVector);


            // Vector3 dir = new Vector3(x,y,0);
            Vector3 dir = radialVector + tangentialVector;
            // Vector3 dir = new Vector3(radial, tangential, 0);
            // Vector3 dir = new Vector3(tangential, radial, 0);


            dir = dir.normalized;
            Debug.Log("dir: " + dir);
            // Update r
            // float angleStepRadians = angleStep * Mathf.Deg2Rad;
            // r = r + dir * angleStepRadians;  // Update position vector
            r = r + dir * 100f;
            // r = r + magneticUnitVector * 1f;
            // r = r + magneticUnitVector;
            // r = newPosition;
            // Debug.Log("r updated: " + r);
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
        // Vector3 r = new Vector3(0,0.5f*6371f*Mathf.Pow(10f,3f),0);
        Vector3 r = new Vector3(6371f*Mathf.Pow(10f,3f),0,0);
        Vector3 magneticMoment = new Vector3(0, 8 * Mathf.Pow(10f, 22f), 0);
        float vacuumPermeability = 4f*Mathf.PI*Mathf.Pow(10f,-7f);


        // float r = 6371f*Mathf.Pow(10f,3f);
        // float magneticMoment = 8f * Mathf.Pow(10f, 22f); 
        // float angle = 90f;


        // float magneticPotential = -magneticMoment * Mathf.Cos(angle * Mathf.Deg2Rad) / (4f*Mathf.PI * Mathf.Pow(r, 2f));
        // Debug.Log("magnetic potential: " + magneticPotential);
        // float radial = vacuumPermeability * magneticMoment.magnitude * 2f * Mathf.Cos(angle * Mathf.Deg2Rad) / (4f * Mathf.PI * Mathf.Pow(r.magnitude, 3f));
        // Debug.Log("radial component: " + radial);
        // float tangential = vacuumPermeability * magneticMoment.magnitude * Mathf.Sin(angle * Mathf.Deg2Rad) / (4f * Mathf.PI * Mathf.Pow(r.magnitude, 3f));
        // Debug.Log("tangential component: " + tangential);

        // Calculates the points along the ellipse
        for (int i = 0; i < numPoints; i++)
        {


            points[i] = new Vector3(r.x/1000000, r.y/1000000, 0);
            Debug.Log("point: " + points[i]);

            float angle = i * angleStep;
            Debug.Log("angle: " + angle);

            // vectorPotential = (vacuumPermeability / (4*Mathf.PI)) * (Vector3.Cross(magneticMoment, r) / Mathf.Pow(r.magnitude, 3));
            // Vector3 magneticField = vacuumPermeability/(4*Mathf.PI) * ((Vector3.Cross(3*r,(Vector3.Cross(magneticMoment, r))) / Mathf.Pow(r.magnitude, 5f)) - (magneticMoment / Mathf.Pow(r.magnitude, 3f)));
            
            // float magneticPotential = -magneticMoment * Mathf.Cos(angle * Mathf.Deg2Rad) / (4f*Mathf.PI * Mathf.Pow(r, 2f));
            // Debug.Log("magnetic potential: " + magneticPotential);
            float radial = -vacuumPermeability * (magneticMoment.magnitude * 2f * Mathf.Sin(angle * Mathf.Deg2Rad) / (4f * Mathf.PI * Mathf.Pow(r.magnitude, 3f)));
            Debug.Log("radial component: " + radial);
            float tangential = vacuumPermeability * (magneticMoment.magnitude * Mathf.Cos(angle * Mathf.Deg2Rad) / (4f * Mathf.PI * Mathf.Pow(r.magnitude, 3f)));
            Debug.Log("tangential component: " + tangential);
           
            // Vector3 magneticField = vacuumPermeability * (3*Vector3.Dot(magneticMoment, r) * r - magneticMoment) / Mathf.Pow(r.magnitude, 3);
            // Debug.Log("magentic field (B): " + magneticField);
            // Vector3 magneticUnitVector = magneticField.normalized;
            // Debug.Log("magnetic unit vector (B_hat): " + magneticUnitVector);

            /////////
            // Update r based on the angle and magnetic unit vector
            // float x = r.magnitude * Mathf.Cos(angle * Mathf.Deg2Rad);
            // float y = r.magnitude * Mathf.Sin(angle * Mathf.Deg2Rad);
            // float ellipseRadius = r.magnitude; // Assuming r is the current position vector magnitude

            // Vector3 newPosition = new Vector3(x, y, 0);

            // Calculate the new position in the plane of the ellipse
            // Vector3 newPosition = cosAngle * ellipseRadius * magneticUnitVector + sinAngle * ellipseRadius * Vector3.Cross(Vector3.up, magneticUnitVector);



            // float x = radial * Mathf.Cos(angle * Mathf.Deg2Rad) - tangential * Mathf.Sin(angle * Mathf.Deg2Rad);
            // float y = radial * Mathf.Sin(angle * Mathf.Deg2Rad) + tangential * Mathf.Cos(angle * Mathf.Deg2Rad);
            // Debug.Log("x: " + x);
            // Debug.Log("y: " + y);



            // Vector3 dir = new Vector3(x,y,0);
            Vector3 dir = new Vector3(radial, tangential, 0);
            // Vector3 dir = new Vector3(tangential, radial, 0);
            dir = dir.normalized;
            Debug.Log("dir: " + dir);
            // Update r
            r = r + dir * 100000f;
            // r = r + magneticUnitVector * 1f;
            // r = r + magneticUnitVector;
            // r = newPosition;
            // Debug.Log("r updated: " + r);
        }

        fieldLinePoints = points;

        createLine2(points);
        // lr.positionCount = fieldLinePoints.Length;
        // lr.SetPositions(fieldLinePoints);
    }


    private void calcFieldPoints3()
    {
        Vector3[] points = new Vector3[numPoints];
        float angleStep = 360f / numPoints;
        // Vector3 r = new Vector3(0,0.5f*6371f*Mathf.Pow(10f,3f),0);
        Vector3 r = new Vector3(6371f*Mathf.Pow(10f,3f),0,0);
        Vector3 magneticMoment = new Vector3(8 * Mathf.Pow(10f, 22f), 0, 0);
        float vacuumPermeability = 4f*Mathf.PI*Mathf.Pow(10f,-7f);


        // float r = 6371f*Mathf.Pow(10f,3f);
        // float magneticMoment = 8f * Mathf.Pow(10f, 22f); 
        // float angle = 90f;


        // float magneticPotential = -magneticMoment * Mathf.Cos(angle * Mathf.Deg2Rad) / (4f*Mathf.PI * Mathf.Pow(r, 2f));
        // Debug.Log("magnetic potential: " + magneticPotential);
        // float radial = vacuumPermeability * magneticMoment.magnitude * 2f * Mathf.Cos(angle * Mathf.Deg2Rad) / (4f * Mathf.PI * Mathf.Pow(r.magnitude, 3f));
        // Debug.Log("radial component: " + radial);
        // float tangential = vacuumPermeability * magneticMoment.magnitude * Mathf.Sin(angle * Mathf.Deg2Rad) / (4f * Mathf.PI * Mathf.Pow(r.magnitude, 3f));
        // Debug.Log("tangential component: " + tangential);

        // Calculates the points along the ellipse
        for (int i = 0; i < numPoints; i++)
        {


            points[i] = new Vector3(r.x/1000000, r.y/1000000, 0);
            Debug.Log("point: " + points[i]);

            float angle = i * angleStep;
            Debug.Log("angle: " + angle);

            // vectorPotential = (vacuumPermeability / (4*Mathf.PI)) * (Vector3.Cross(magneticMoment, r) / Mathf.Pow(r.magnitude, 3));
            // Vector3 magneticField = vacuumPermeability/(4*Mathf.PI) * ((Vector3.Cross(3*r,(Vector3.Cross(magneticMoment, r))) / Mathf.Pow(r.magnitude, 5f)) - (magneticMoment / Mathf.Pow(r.magnitude, 3f)));
            
            // float magneticPotential = -magneticMoment * Mathf.Cos(angle * Mathf.Deg2Rad) / (4f*Mathf.PI * Mathf.Pow(r, 2f));
            // Debug.Log("magnetic potential: " + magneticPotential);
            float radial = -vacuumPermeability * (magneticMoment.magnitude * 2f * Mathf.Cos(angle * Mathf.Deg2Rad) / (4f * Mathf.PI * Mathf.Pow(r.magnitude, 3f)));
            Debug.Log("radial component: " + radial);
            float tangential = vacuumPermeability * (magneticMoment.magnitude * Mathf.Sin(angle * Mathf.Deg2Rad) / (4f * Mathf.PI * Mathf.Pow(r.magnitude, 3f)));
            Debug.Log("tangential component: " + tangential);
           
            // Vector3 magneticField = vacuumPermeability * (3*Vector3.Dot(magneticMoment, r) * r - magneticMoment) / Mathf.Pow(r.magnitude, 3);
            // Debug.Log("magentic field (B): " + magneticField);
            // Vector3 magneticUnitVector = magneticField.normalized;
            // Debug.Log("magnetic unit vector (B_hat): " + magneticUnitVector);

            /////////
            // Update r based on the angle and magnetic unit vector
            // float x = r.magnitude * Mathf.Cos(angle * Mathf.Deg2Rad);
            // float y = r.magnitude * Mathf.Sin(angle * Mathf.Deg2Rad);
            // float ellipseRadius = r.magnitude; // Assuming r is the current position vector magnitude

            // Vector3 newPosition = new Vector3(x, y, 0);

            // Calculate the new position in the plane of the ellipse
            // Vector3 newPosition = cosAngle * ellipseRadius * magneticUnitVector + sinAngle * ellipseRadius * Vector3.Cross(Vector3.up, magneticUnitVector);



            // float x = radial * Mathf.Cos(angle * Mathf.Deg2Rad) - tangential * Mathf.Sin(angle * Mathf.Deg2Rad);
            // float y = radial * Mathf.Sin(angle * Mathf.Deg2Rad) + tangential * Mathf.Cos(angle * Mathf.Deg2Rad);
            // Debug.Log("x: " + x);
            // Debug.Log("y: " + y);



            // Vector3 dir = new Vector3(x,y,0);
            // Vector3 dir = new Vector3(radial, tangential, 0);
            Vector3 dir = new Vector3(tangential, radial, 0);
            dir = dir.normalized;
            Debug.Log("dir: " + dir);
            // Update r
            r = r + dir * 100000f;
            // r = r + magneticUnitVector * 1f;
            // r = r + magneticUnitVector;
            // r = newPosition;
            // Debug.Log("r updated: " + r);
        }

        fieldLinePoints = points;

        createLine2(points);
        // lr.positionCount = fieldLinePoints.Length;
        // lr.SetPositions(fieldLinePoints);
    }

    public void calcFieldPoints4()
    {
        Vector3[] points = new Vector3[numPoints];
        float angleStep = 360f / numPoints;
        float vacuumPermeability = 4f*Mathf.PI*Mathf.Pow(10f,-7f);
        // Vector3 magneticMoment = new Vector3(0, 2 * Mathf.Pow(10f, 14f), 0);
        // Vector3 r = new Vector3(5000f,500,0);
        // Vector3 ellipseCenter = new Vector3(0, 500, 0);
        Vector3 r = new Vector3(6371f*Mathf.Pow(10f,3f),500000,0);
        Vector3 magneticMoment = new Vector3(8 * Mathf.Pow(10f, 22f), 0, 0);
        Vector3 ellipseCenter = new Vector3(0, 500000, 0);

        for (int i = 0; i < numPoints; i++)
        {
            points[i] = new Vector3(r.x/1000000, r.y/1000000, 0);
            Debug.Log("point: " + points[i]);

            float ellipseAngle = i * angleStep;
            Debug.Log("ellipse angle: " + ellipseAngle);

            float angle = Mathf.Atan(r.y / r.x);
            if(ellipseAngle > 90 && ellipseAngle < 270)
            {
                angle += Mathf.PI;
            }            
            Debug.Log("angle: " + angle*Mathf.Rad2Deg);

            float radial = -vacuumPermeability * magneticMoment.magnitude * 2f * Mathf.Sin(angle) / (4f * Mathf.PI * Mathf.Pow(r.magnitude, 3f));
            float tangential = vacuumPermeability * magneticMoment.magnitude * Mathf.Cos(angle) / (4f * Mathf.PI * Mathf.Pow(r.magnitude, 3f));
            Debug.Log("radial component: " + radial);
            Debug.Log("tangential component: " + tangential);

            Vector3 radialVector = new Vector3(radial * Mathf.Cos(angle), radial * Mathf.Sin(angle), 0f);
            Vector3 tangentialVector = new Vector3(-tangential * Mathf.Sin(angle), tangential * Mathf.Cos(angle), 0);
            Debug.Log("radial component adjusted: " + radialVector);
            Debug.Log("tangential component adjusted: " + tangentialVector);

            Vector3 dir = radialVector + tangentialVector;
            Debug.Log("dir: " + dir);
            dir = dir.normalized;
            Debug.Log("dir normalized: " + dir);

            Vector3 newAnglePoint = new Vector3((r.x - ellipseCenter.x) * Mathf.Cos(angleStep * Mathf.Deg2Rad) - (r.y - ellipseCenter.y) * Mathf.Sin(angleStep * Mathf.Deg2Rad) + ellipseCenter.x, (r.x - ellipseCenter.x) * Mathf.Sin(angleStep * Mathf.Deg2Rad) + (r.y - ellipseCenter.y) * Mathf.Cos(angleStep * Mathf.Deg2Rad) + ellipseCenter.y);
            Vector3 directionPoint = new Vector3(r.x + dir.x * 2f, r.y + dir.y * 2f);

            Vector3 intersectionPoint = findIntersection(r, directionPoint, ellipseCenter, newAnglePoint);
            r = intersectionPoint;
        }

        createLine1(points);
    }

    private Vector3 findIntersection(Vector3 r, Vector3 directionPoint, Vector3 ellipseCenter, Vector3 newAnglePoint)
    {
        Debug.Log("r: " + r);
        Debug.Log("directionPoint: " + directionPoint);
        Debug.Log("ellipseCenter: " + ellipseCenter);
        Debug.Log("newAnglePoint: " + newAnglePoint);

        float x1 = r.x;
        float y1 = r.y;
        float x2 = directionPoint.x;
        float y2 = directionPoint.y;

        float x3 = ellipseCenter.x;
        float y3 = ellipseCenter.y;
        float x4 = newAnglePoint.x;
        float y4 = newAnglePoint.y;


        float denominator = determinant(x1 - x2, y1 - y2, x3 - x4, y3 - y4);

        // Check if the vectors are parallel
        if (denominator == 0)
        {
            Debug.Log("no intersection");
            return new Vector3(0,0,0); // Vectors are parallel, no intersection point
        }

        // Calculate the intersection point
        float xNumerator = determinant(determinant(x1, y1, x2, y2), x1 - x2, determinant(x3, y3, x4, y4), x3 - x4);
        float yNumerator = determinant(determinant(x1, y1, x2, y2), y1 - y2, determinant(x3, y3, x4, y4), y3 - y4);

        float xIntersection = xNumerator / denominator;
        float yIntersection = yNumerator / denominator;

        return new Vector3(xIntersection, yIntersection, 0);
    }

    private float determinant(float a, float b, float c, float d)
    {
        return a * d - b * c;
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
