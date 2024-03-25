using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ArrowGenerator : MonoBehaviour
{
    [SerializeField] private Sprite arrowImg;
    private float magneticMoment;
    private List<int> ellipseNums = new List<int>();
    private int numEllipses;

    public List<(Vector3, Vector3, Vector3)> getFieldPoints(Torus torus, int numPoints, int numArrows)
    {
        List<(Vector3, Vector3, Vector3)> fieldPoints = new List<(Vector3, Vector3, Vector3)>();
        GameObject torusObject = torus.torusObject;
        List<Ellipse> ellipses = torus.getEllipses();
        this.numEllipses = ellipses.Count;

        // iterate to get points for arrows
        for (int i = 0; i < numArrows; i++)
        {
            Vector3 r;
            Vector3 nextPoint;
            int count = 0;
            int ctr = 0;
            Ellipse ellipse;
            List<(int, Vector3)> ellipseUsablePoints;

            int ellipseNum;
            do
            {
                // don't use ellipses too close to Psyche
                do
                {
                    ellipseNum = Random.Range(0, ellipses.Count); // get random ellipse line

                } while (ellipses[ellipseNum].usablePoints.Count == 0);

                ellipse = ellipses[ellipseNum];
                ellipseUsablePoints = ellipse.usablePoints;

                int pointIndex;

                // keep arrows out of Psyche
                do
                {
                    // get random point on ellipse line
                    pointIndex = Random.Range(0, ellipseUsablePoints.Count);
                    r = ellipseUsablePoints[pointIndex].Item2;
                    nextPoint = ellipse.lineObject.GetComponent<LineRenderer>().GetPosition(ellipseUsablePoints[pointIndex].Item1 + 1);

                    if (ctr >= ellipseUsablePoints.Count * 2)
                    {
                        break;
                    }
                    ctr++;

                } while (proximity(r, fieldPoints) || (r.magnitude < 1f));

                if (count >= ellipses.Count)
                {
                    break;
                }
                count++;

            } while (ctr >= ellipseUsablePoints.Count * 2);
            ellipseNums.Add(ellipseNum);

            Vector3 magField = calcMagField(r, torus.magneticMoment);

            fieldPoints.Add((r, magField, nextPoint));
        }

        return fieldPoints;
    }

    private bool proximity(Vector3 r, List<(Vector3, Vector3, Vector3)> fieldPoints)
    {
        foreach ((Vector3, Vector3, Vector3) point in fieldPoints)
        {
            // TODO: maybe try to use m_Collider.bounds.Intersects(m_Collider2.bounds) to stop arrows from overlapping (can't do here rn)
            if (Vector3.Distance(r, point.Item1) < 2.0f) 
            {
                return true;
            }
        }
        return false;
    }

    private Vector3 calcMagField(Vector3 r, Vector3 magneticMoment)
    {
        float vacuumPermeability = 4f * Mathf.PI * Mathf.Pow(10f, -7f);

        // r = new(2.238773f, 2.197586f, 0);
        // r = new(-2.238773f, 2.197586f, 0);
        // r = new(6371 * Mathf.Pow(10, 3), 0, 0);
        // r = new(0, 6371 * Mathf.Pow(10, 3), 0);
        // r = new(0, 2, 0);
        // r = new(2, 0, 0);

        // Calculate the angle between the magnetic moment and the position vector
        Vector3 cross = Vector3.Cross(r.normalized, magneticMoment.normalized);
        float angle = Mathf.Acos(Vector3.Dot(r.normalized, magneticMoment.normalized)) * Mathf.Rad2Deg;
        // Adjust angle sign based on the direction of the cross product
        angle *= Mathf.Sign(Vector3.Dot(cross, Vector3.back));
        // Debug.Log("angle2: " + angle2);
        angle *= Mathf.Deg2Rad;

        // Calculate the radial and tangential components
        float radial = ((vacuumPermeability * magneticMoment.magnitude * 2) / (4 * Mathf.PI)) * (Mathf.Cos(angle) / Mathf.Pow(r.magnitude, 3f));
        float tangential = ((-vacuumPermeability * magneticMoment.magnitude) / (4 * Mathf.PI)) * (Mathf.Sin(angle) / Mathf.Pow(r.magnitude, 3f));
        // Debug.Log("radial component: " + radial);
        // Debug.Log("tangential component: " + tangential);

        // // calculation with vectors (gives slightly differnt magnitudes)
        // Debug.Log("dot for B: " + Vector3.Dot(magneticMoment, r));
        // float component1 = (vacuumPermeability / (4 * Mathf.PI));
        // Vector3 component2 = ((3 * r * Vector3.Dot(magneticMoment, r)) / Mathf.Pow(r.magnitude, 5));
        // Vector3 component3 = (magneticMoment / Mathf.Pow(r.magnitude, 3f));
        // Debug.Log("comp1B: " + component1 + " comp2B: " + component2 + " comp3B: " + component3);
        // float x1 = component1 * (((3 * r.x * Vector3.Dot(magneticMoment, r)) / Mathf.Pow(r.magnitude, 5)) - (magneticMoment.x / Mathf.Pow(r.magnitude, 3f)));
        // float y1 = component1 * (((3 * r.y * Vector3.Dot(magneticMoment, r)) / Mathf.Pow(r.magnitude, 5)) - (magneticMoment.y / Mathf.Pow(r.magnitude, 3f)));
        // Debug.Log("x1B: " + x1 + " y1B: " + y1);
        // Vector3 B = component1 * (component2 - component3);
        // Debug.Log("B: " + B);

        // make components into vectors
        Vector3 radialVector = new Vector3(radial * Mathf.Cos(angle), radial * Mathf.Sin(angle), 0);
        Vector3 tangentialVector = new Vector3(tangential * Mathf.Sin(angle), tangential * Mathf.Cos(angle), 0);
        // Debug.Log("radial vector: " + radialVector);
        // Debug.Log("tangential vector: " + tangentialVector);

        // combine component vectors
        Vector3 magField = radialVector + tangentialVector;
        // Debug.Log("magField3: " + magField3);

        return magField;
    }

    public void drawArrows(List<(Vector3, Vector3, Vector3)> fieldPoints)
    {
        int i = 0;
        foreach ((Vector3, Vector3, Vector3) point in fieldPoints)
        {
            float fieldMagnitude = point.Item2.magnitude;
            // Debug.Log("mag field: " + fieldMagnitude);
            float modifiedMagnitude = mapRange(fieldMagnitude);

            // Debug.Log("orig mag " + point.Item2);
            // Debug.Log("mag float " + fieldMagnitude);
            // Debug.Log("reduced mag " + reducedMag);
            // Debug.Log("mod mag " + modifiedMagnitude);

            float fadeValue = 1.0f - ((0.6f / 4) * ((ellipseNums[i]-1) % (numEllipses/2)));
            // Debug.Log("fadeValue: " + fadeValue);

            GameObject arrow = new("arrow" + i);
            arrow.AddComponent<SpriteRenderer>();
            arrow.GetComponent<SpriteRenderer>().sprite = arrowImg;
            arrow.GetComponent<SpriteRenderer>().color = new Color(fadeValue, fadeValue, fadeValue, 1);
            arrow.transform.SetPositionAndRotation(point.Item1, Quaternion.identity);

            float angle;
            if (fieldMagnitude == 0)
            {
                // set random arrow rotation
                Vector3 direction = point.Item3 - arrow.transform.position;
                angle = (Mathf.Atan2(direction.y, direction.x) + 90f) * Mathf.Rad2Deg;

                // set random magnitude
                modifiedMagnitude = Random.Range(0.06f, 0.3f);
            }
            else
            {
                // set arrow rotation
                Vector3 direction = point.Item3 - arrow.transform.position;
                angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
            }
            arrow.transform.rotation = Quaternion.Euler(0, 0, angle);

            // bring arrow in front of torus
            Transform ArrowTransform = arrow.transform;
            Vector3 newPosition = ArrowTransform.position;
            newPosition.z = -1;
            ArrowTransform.position = newPosition;

            arrow.transform.localScale = new(modifiedMagnitude, modifiedMagnitude, 1);

            i++;
        }
    }

    private float mapRange(float magStrength)
    {
        // Debug.Log("mag: " + magStrength);
        // Define the input range
        float inputMin = (1 * Mathf.Pow(10, 4));
        float inputMax = (3 * Mathf.Pow(10, 8));

        // Define the output range
        float outputMin = 0.58f;
        float outputMax = 37.06f;

        float mappedMagStrength = (magStrength - inputMin) * ((outputMax - outputMin) / (inputMax - inputMin)) + outputMin;
        // Debug.Log("mappedVal: " + mappedMagStrength);

        // Function to perform the linear transformation
        float modifiedMagnitude = (0.15f / (1f + Mathf.Exp(-(mappedMagStrength - 1f)))) + (1f - Mathf.Exp(-mappedMagStrength)) * (0.1f - 0.06f) * ((mappedMagStrength - 0.2f) / (10f - 0.2f));
        // Debug.Log("modified Magnitude: " + modifiedMagnitude);
        return modifiedMagnitude;
    }
}
