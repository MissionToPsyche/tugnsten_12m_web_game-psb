using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MagnetometerController : MonoBehaviour
{
    private int numArrows = 3;
    private int numEllipses = 5;
    private GameObject torus;
    private int numPoints = 200;
    [SerializeField] private Sprite arrowImg;

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
            float cutoff = 0.25f;

            int ellipseNum;
            // don't use ellipses closest to Psyche
            do
            {
                ellipseNum = Random.Range(1, ellipses.Count); // get random ellipse line
            } while(ellipseNum == Mathf.Floor(torus.transform.childCount / 2));

            // Debug.Log("ellipse num: " + ellipseNum);

            GameObject ellipse = ellipses[ellipseNum];

            int pointIndex;
            Vector3 r;
            // keep arrows out of Psyche
            do
            {
                // get random point on ellipse line within a range
                pointIndex = Random.Range((int)(numPoints * cutoff), (int)(numPoints * (1 - cutoff)));

                r = ellipse.GetComponent<LineRenderer>().GetPosition(pointIndex);

            } while(r.magnitude < 1f);

            // Debug.Log("point: " + pointIndex);

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

            // ellipses.RemoveAt(ellipseNum);
        }

        return fieldPoints;
    }

    private void drawArrows(List<(Vector3, Vector3)> fieldPoints)
    {
        int i = 0;
        foreach ((Vector3, Vector3) point in fieldPoints)
        {
            // Rotation facing in the direction of the magnetic field's magnitude
            Quaternion rotation = Quaternion.LookRotation(Vector3.forward, point.Item2);

            float fieldMagnitude = point.Item2.magnitude;

            // janky math to normalize the scale of the arrows
            float reducedMag = fieldMagnitude / 1000000f;
            float modifiedMagnitude = (0.15f/(1f+Mathf.Exp(-(reducedMag-1f)))) + (1f-Mathf.Exp(-reducedMag)) * (0.1f-0.06f) * ((reducedMag-0.2f)/(10f-0.2f));

            // Debug.Log("orig mag " + point.Item2);
            // Debug.Log("mag float " + fieldMagnitude);
            // Debug.Log("reduced mag " + reducedMag);
            // Debug.Log("mod mag " + modifiedMagnitude);

            GameObject arrow = new("arrow" + i);
            arrow.AddComponent<SpriteRenderer>();
            arrow.GetComponent<SpriteRenderer>().sprite = arrowImg;
            arrow.transform.SetPositionAndRotation(point.Item1, rotation);

            // bring arrow in front of torus
            Transform ArrowTransform = arrow.transform;
            Vector3 newPosition = ArrowTransform.position;
            newPosition.z = -1;
            ArrowTransform.position = newPosition;

            arrow.transform.localScale = new(modifiedMagnitude, modifiedMagnitude, 1);

            i++;
        }
    }

}
