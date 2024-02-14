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

        float targetRotation = Vector3.SignedAngle(Vector3.right, magneticMoment, Vector3.forward);
        Debug.Log("rotation target: " + targetRotation);

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
        float rotation2 = rotation - Mathf.Sign(rotation)*180;
        Vector3 scale = torus.transform.localScale;

        float diff1 = Mathf.Abs(rotation - targetRotation);
        float diff2 = Mathf.Abs(rotation2 - targetRotation);

        if((diff1 < rotationMargin || diff2 < rotationMargin) && Mathf.Abs(Vector3.Distance(scale, targetScale)) < scaleMargin)
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

            } while(proximity(r, fieldPoints) || (r.magnitude < 1f));

            float angle = Vector3.SignedAngle(magneticMoment, r, Vector3.forward);
            angle *= Mathf.Deg2Rad;

            // calculate magnetic field radial and tangential components
            float radial = -vacuumPermeability * Vector3.Dot(magneticMoment, r.normalized) * 2f * Mathf.Sin(angle) / (4f * Mathf.PI * Mathf.Pow(r.magnitude, 3f));
            float tangential = vacuumPermeability * Vector3.Dot(magneticMoment, r.normalized) * Mathf.Cos(angle) / (4f * Mathf.PI * Mathf.Pow(r.magnitude, 3f));
            // Debug.Log("radial component: " + radial);
            // Debug.Log("tangential component: " + tangential);

            // make components vectors
            Vector3 radialVector = r.normalized * radial;
            Vector3 tangentialVector = new Vector3(-r.y, r.x, 0).normalized * tangential;

            // make magnetic field one vector
            Vector3 magField = radialVector + tangentialVector;
            // Debug.Log("magField: " + magField);

            fieldPoints.Add((r, magField));

            // ellipses.RemoveAt(ellipseNum);
        }

        return fieldPoints;
    }

    private bool proximity(Vector3 r, List<(Vector3, Vector3)> fieldPoints)
    {
        foreach((Vector3, Vector3) point in fieldPoints)
        {
            if(Vector3.Distance(r, point.Item1) < 1.0f)
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
            Quaternion rotation = Quaternion.LookRotation(Vector3.forward, point.Item2);

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

            i++;
        }
    }

}
