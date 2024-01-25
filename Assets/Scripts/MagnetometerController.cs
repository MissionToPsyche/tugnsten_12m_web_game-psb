using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MagnetometerController : MonoBehaviour
{
    private int numArrows = 3;
    private int numEllipses = 5;
    private GameObject torus;
    private int numPoints = 200;

    // Start is called before the first frame update
    void Start()
    {
        TorusGenerator torusGenerator = GameObject.Find("TorusGenerator").GetComponent<TorusGenerator>();
        torus = new GameObject("MagneticTorus");
        torusGenerator.drawTorus(numEllipses, torus, numPoints);
        torus.AddComponent<MoveTorus>();

        List<(Vector3, Vector3)> fieldPoints = getFieldPoints();
        drawArrows(fieldPoints);
    }

    // Update is called once per frame
    void Update()
    {

    }

    private List<(Vector3, Vector3)> getFieldPoints()
    {
        List<(Vector3, Vector3)> fieldPoints = new List<(Vector3, Vector3)>();

        // variables for calculating magnetic field
        Vector3 magneticMoment = new Vector3(0, 2 * Mathf.Pow(10f, 14f), 0);
        float vacuumPermeability = 4f * Mathf.PI * Mathf.Pow(10f, -7f);

        List<GameObject> ellipses = new List<GameObject>();

        // get ellipse gameobjects
        for (int i = 0; i < torus.transform.childCount; i++)
        {
            GameObject child = torus.transform.GetChild(i).gameObject;
            ellipses.Add(child);
        }

        // iterate to get points for arrows
        for (int i = 0; i < numArrows; i++)
        {
            float cutoff = 0.1f;
            int ellipseNum = Random.Range(0, ellipses.Count); // get random ellipse line
            int pointIndex = Random.Range((int)(numPoints * cutoff), (int)(numPoints * (1 - cutoff))); // get random point on ellipse line within a range

            // calculate radius and angle
            GameObject ellipse = ellipses[ellipseNum];
            Vector3 r = ellipse.GetComponent<LineRenderer>().GetPosition(pointIndex);

            // Calculate angle with respect to the x-axis
            float angle = Mathf.Atan2(r.y, r.x) * Mathf.Rad2Deg;
            // Ensure the angle is positive (between 0 and 360)
            angle = (angle + 360f) % 360f;

            // calculate magnetic field radial and tangential components
            float radial = -vacuumPermeability * magneticMoment.magnitude * 2f * Mathf.Sin(angle * Mathf.Deg2Rad) / (4f * Mathf.PI * Mathf.Pow(r.magnitude, 3f));
            float tangential = vacuumPermeability * magneticMoment.magnitude * Mathf.Cos(angle * Mathf.Deg2Rad) / (4f * Mathf.PI * Mathf.Pow(r.magnitude, 3f));
            // Debug.Log("radial component: " + radial);
            // Debug.Log("tangential component: " + tangential);

            // make components vectors
            Vector3 radialVector = new Vector3(radial * Mathf.Cos(angle * Mathf.Deg2Rad), radial * Mathf.Sin(angle * Mathf.Deg2Rad), 0f);
            Vector3 tangentialVector = new Vector3(-tangential * Mathf.Sin(angle * Mathf.Deg2Rad), tangential * Mathf.Cos(angle * Mathf.Deg2Rad), 0);
            // Debug.Log("radial component adjusted: " + radialVector);
            // Debug.Log("tangential component adjusted: " + tangentialVector);

            // make magnetic field one vector
            Vector3 magField = radialVector + tangentialVector;
            fieldPoints.Add((r, magField));

            ellipses.RemoveAt(ellipseNum);
        }

        return fieldPoints;
    }

    private void drawArrows(List<(Vector3, Vector3)> fieldPoints)
    {
        int i = 0;
        foreach ((Vector3, Vector3) point in fieldPoints)
        {
            Debug.Log("point: " + point.Item1 + " magnetic field: " + point.Item2);

            GameObject arrow = new GameObject("arrow" + i);

            // add components
            arrow.AddComponent<MeshFilter>();
            MeshRenderer mr = arrow.AddComponent<MeshRenderer>();
            ArrowGenerator ag = arrow.AddComponent<ArrowGenerator>();

            // set material and color
            mr.material = new Material(Shader.Find("Diffuse"));
            mr.material.SetColor("_Color", new Color(255f, 0f, 0f));

            // calc stem length
            float stemLength = 0.5f; // default
            float magnitude = point.Item2.magnitude;
            int digits = countDigits(magnitude);
            switch(digits)
            {
                case 12:
                    stemLength = 1.0f;
                    break;
                case 11:
                    stemLength = 09f;
                    break;
                case 10:
                    stemLength = 0.8f;
                    break;
                case 9:
                    stemLength = 0.7f;
                    break;
                case 8:
                    stemLength = 0.6f;
                    break;
                case 7:
                    stemLength = 0.5f;
                    break;
                case 6:
                    stemLength = 0.4f;
                    break;
                case 5:
                    stemLength = 0.3f;
                    break;
            }
            Debug.Log("stemLength: " + stemLength);

            // set position/rotation
            arrow.transform.position = point.Item1;

            // add code to get angle of rotation to match direction of point.Item2's direction


            // get rotation angle
            // Quaternion rotation = Quaternion.LookRotation(point.Item2.normalized);
            // arrow.transform.rotation = rotation;


            // Vector3 magNorm = point.Item2.normalized;
            // float rotation = magNorm.normalized;

            // arrow.transform.Rotate(new Vector3(0, 0, rotation));

            ag.GenerateArrow(stemLength);
            i++;
        }
    }

    private int countDigits(float mag) 
    { 
        int count = 0; 
        while (mag > 1.0f) { 
            mag /= 10; 
            ++count; 
        } 
        return count; 
    } 

}
