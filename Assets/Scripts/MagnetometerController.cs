using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MagnetometerController : MonoBehaviour
{
    private int numArrows = 3;
    private int numEllipses = 5;
    private GameObject torus;
    private int numPoints = 200;
    [SerializeField] private Sprite arrowPrefab;

    // Start is called before the first frame update
    void Start()
    {
        TorusGenerator torusGenerator = GameObject.Find("TorusGenerator").GetComponent<TorusGenerator>();
        torus = new GameObject("MagneticTorus");
        List<float> ellipseAxes = torusGenerator.drawTorus(numEllipses, torus, numPoints);
        torus.AddComponent<MoveTorus>();

        List<(Vector3, float, Vector3)> fieldPoints = getFieldPoints(ellipseAxes);
        drawArrows(fieldPoints);
    }

    // Update is called once per frame
    void Update()
    {

    }

    private List<(Vector3, float, Vector3)> getFieldPoints(List<float> ellipseAxes)
    {
        List<(Vector3, float, Vector3)> fieldPoints = new List<(Vector3, float, Vector3)>();

        // variables for calculating magnetic field
        Vector3 magneticMoment = new Vector3(2 * Mathf.Pow(10f, 14f), 0, 0);
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
            float cutoff = 0.2f;

            int ellipseNum;
            // don't use closest lines
            do
            {
                ellipseNum = Random.Range(0, ellipses.Count); // get random ellipse line
            } while (ellipseNum == 0 || ellipseNum == Mathf.Floor(torus.transform.childCount / 2));

            Debug.Log("ellipse num: " + ellipseNum);

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
            fieldPoints.Add((r, ellipseAxes[ellipseNum], magField));

            ellipses.RemoveAt(ellipseNum);
        }

        return fieldPoints;
    }

    private void drawArrows(List<(Vector3, float, Vector3)> fieldPoints)
    {
        int i = 0;
        foreach ((Vector3, float, Vector3) point in fieldPoints)
        {
            // Rotation facing in the direction of the magnetic field's magnitude
            Quaternion rotation = Quaternion.LookRotation(Vector3.forward, point.Item3);

            GameObject go = new();
            go.AddComponent<SpriteRenderer>();
            go.GetComponent<SpriteRenderer>().sprite = arrowPrefab;
            go.transform.SetPositionAndRotation(point.Item1, rotation);
            go.transform.localScale = new(0.1f, 0.1f, 1);

            i++;
        }
    }

}
