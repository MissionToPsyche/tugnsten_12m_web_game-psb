using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MagnetometerController : MonoBehaviour
{
    private int numArrows = 3;
    private int numEllipses = 5;
    private GameObject torus;
    private int numPoints = 200;
    private Vector3 magneticMoment;
    [SerializeField] private Sprite arrowImg;
    [SerializeField] private GameObject buttonObj;

    // Start is called before the first frame update
    void Start()
    {
        TorusGenerator torusGenerator = GameObject.Find("TorusGenerator").GetComponent<TorusGenerator>();
        torus = new GameObject("MagneticTorus");
        magneticMoment = torusGenerator.drawTorus(numEllipses, torus, numPoints);
        torus.AddComponent<MoveTorus>();

        List<(Vector3, Vector3)> fieldPoints = getFieldPoints();
        drawArrows(fieldPoints);

        // temporary
        Button button = buttonObj.GetComponent<Button>();
        button.onClick.AddListener(checkCorrectness);
    }


    private void checkCorrectness()
    {
        float targetRotation = 0f;
        Vector3 targetScale = new(1, 1, 1);

        float scaleMargin = 0.15f;
        float rotationMargin = 10f;

        float rotation = torus.transform.localRotation.eulerAngles.z;
        float rotation180 = rotation - Mathf.Sign(rotation) * 180;
        Vector3 scale = torus.transform.localScale;

        float diff1 = Mathf.Abs(rotation - targetRotation);
        float diff2 = Mathf.Abs(rotation180 - targetRotation);

        if ((diff1 < rotationMargin || diff2 < rotationMargin) && Mathf.Abs(Vector3.Distance(scale, targetScale)) < scaleMargin)
        {
            Debug.Log("yay!");
            // return true;
        }
        else
        {
            Debug.Log("boo");
            // return false;
        }
    }

    // Update is called once per frame
    void Update()
    {

    }

    private List<(Vector3, Vector3)> getFieldPoints()
    {
        List<(Vector3, Vector3)> fieldPoints = new List<(Vector3, Vector3)>();
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
            } while (ellipseNum == Mathf.Floor(torus.transform.childCount / 2));

            GameObject ellipse = ellipses[ellipseNum];

            int pointIndex;
            Vector3 r;
            // keep arrows out of Psyche
            do
            {
                // get random point on ellipse line within a range
                pointIndex = Random.Range((int)(numPoints * cutoff), (int)(numPoints * (1 - cutoff)));

                r = ellipse.GetComponent<LineRenderer>().GetPosition(pointIndex);

            } while (proximity(r, fieldPoints) || (r.magnitude < 1f));


            // r = new(2.238773f, 2.197586f, 0);
            // r = new(6371 * Mathf.Pow(10, 3), 0, 0);
            r = new(0, 6371 * Mathf.Pow(10, 3), 0);
            // r = new(0, 2, 0);
            // r = new(2, 0, 0);

            float angle = Vector3.SignedAngle(magneticMoment, r, Vector3.forward);
            Debug.Log("angle: " + angle);
            angle *= Mathf.Deg2Rad;

            // // calculate magnetic field radial and tangential components
            float radial = -vacuumPermeability * Vector3.Dot(magneticMoment, r.normalized) * 2f * Mathf.Sin(angle) / (4f * Mathf.PI * Mathf.Pow(r.magnitude, 3f));
            float tangential = vacuumPermeability * Vector3.Dot(magneticMoment, r.normalized) * Mathf.Cos(angle) / (4f * Mathf.PI * Mathf.Pow(r.magnitude, 3f));
            Debug.Log("radial component: " + radial);
            Debug.Log("tangential component: " + tangential);



            // Debug.Log("r: " + r);
            // Debug.Log("mag mom: " + magneticMoment);


            // Calculate the angle between the magnetic moment and the position vector
            Vector3 cross = Vector3.Cross(r.normalized, magneticMoment.normalized);
            float angle2 = Mathf.Acos(Vector3.Dot(r.normalized, magneticMoment.normalized)) * Mathf.Rad2Deg;

            // Adjust angle sign based on the direction of the cross product
            angle2 *= Mathf.Sign(Vector3.Dot(cross, Vector3.back));

            Debug.Log("angle2: " + angle2);
            angle2 *= Mathf.Deg2Rad;

            // Calculate the radial and tangential components using trigonometric functions
            float radial2 = vacuumPermeability * magneticMoment.magnitude * 2f * Mathf.Cos(angle2) / (4f * Mathf.PI * Mathf.Pow(r.magnitude, 3f));
            float tangential2 = -vacuumPermeability * magneticMoment.magnitude * Mathf.Sin(angle2) / (4f * Mathf.PI * Mathf.Pow(r.magnitude, 3f));

            Debug.Log("radial component2: " + radial2);
            Debug.Log("tangential component2: " + tangential2);


            Vector3 realMag = (vacuumPermeability / (4 * Mathf.PI)) * ((((2 * Vector3.Dot(magneticMoment, r)) * r) - (magneticMoment * Mathf.Pow(r.magnitude, 2))) / Mathf.Pow(r.magnitude, 5));
            Debug.Log("test: " + realMag);



            Vector3 radialVector1 = new Vector3(radial2 * Mathf.Cos(angle), radial2 * Mathf.Sin(angle), 0);
            Vector3 tangentialVector1 = new Vector3(tangential2 * Mathf.Sin(angle), tangential2 * Mathf.Cos(angle), 0);
            Debug.Log("rad vect1: " + radialVector1);
            Debug.Log("tan vect1: " + tangentialVector1);

            // make components vectors
            Vector3 radialVector = r.normalized * radial;
            Vector3 tangentialVector = new Vector3(-r.y, r.x, 0).normalized * tangential;
            Debug.Log("rad vect: " + radialVector);
            Debug.Log("tan vect: " + tangentialVector);
            Vector3 radialVector2 = r.normalized * radial2;
            Vector3 tangentialVector2 = new Vector3(-r.y, r.x, 0).normalized * tangential2;
            Debug.Log("rad vect2: " + radialVector2);
            Debug.Log("tan vect2: " + tangentialVector2);

            // make magnetic field one vector
            Vector3 magField = radialVector + tangentialVector;
            Debug.Log("magField: " + magField);
            Vector3 magField2 = radialVector2 + tangentialVector2;
            Debug.Log("magField2: " + magField2);

            fieldPoints.Add((r, magField2));

            // ellipses.RemoveAt(ellipseNum);
        }

        return fieldPoints;
    }

    private bool proximity(Vector3 r, List<(Vector3, Vector3)> fieldPoints)
    {
        foreach ((Vector3, Vector3) point in fieldPoints)
        {
            if (Vector3.Distance(r, point.Item1) < 1.0f)
            {
                return true;
            }
        }
        return false;
    }

    private void drawArrows(List<(Vector3, Vector3)> fieldPoints)
    {
        int i = 0;
        foreach ((Vector3, Vector3) point in fieldPoints)
        {
            // Rotation facing in the direction of the magnetic field's magnitude
            // Quaternion rotation = Quaternion.LookRotation(Vector3.forward, point.Item2);
            Vector3 perpendicularDirection = Vector3.Cross(point.Item2, Vector3.forward);
            Quaternion rotation = Quaternion.LookRotation(Vector3.forward, -perpendicularDirection);

            float fieldMagnitude = point.Item2.magnitude;

            // janky math to normalize the scale of the arrows
            float reducedMag = fieldMagnitude / 1000000f;
            float modifiedMagnitude = (0.15f / (1f + Mathf.Exp(-(reducedMag - 1f)))) + (1f - Mathf.Exp(-reducedMag)) * (0.1f - 0.06f) * ((reducedMag - 0.2f) / (10f - 0.2f));

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

            // Debug.Log("arrow: " + arrow.transform.rotation);

            i++;
        }
    }

}
