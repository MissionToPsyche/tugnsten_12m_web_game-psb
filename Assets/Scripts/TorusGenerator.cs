using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class TorusGenerator : MonoBehaviour
{
    private int numPoints;
    private int numEllipses;
    private GameObject torusObject;
    private float rotationAngle = 0f;
    private float minScale = 2.5f;
    private float maxScale = 4.5f;

    public Torus drawTorus(int numEllipses, int numPoints)
    {
        this.numPoints = numPoints;
        this.numEllipses = numEllipses;
        this.torusObject = new GameObject("MagneticTorus");
        Torus torus = new Torus();
        torus.torusObject = torusObject;
        float minMagnitude = 2 * Mathf.Pow(10f, 13f);
        float maxMagnitude = 2 * Mathf.Pow(10f, 15f);

        List<Vector3> magMoments = generateMagneticMoments(minMagnitude, maxMagnitude);
        int magMomentIndex = Random.Range(0, magMoments.Count);
        Vector3 magneticMoment = magMoments[magMomentIndex];
        // Debug.Log("mag mom: " + magneticMoment);
        // Debug.Log("mag mag: " + magneticMoment.magnitude);
        // magneticMoment = new Vector3(2 * Mathf.Pow(10f, 14f), 0, 0);
        // magneticMoment = new Vector3(8 * Mathf.Pow(10f, 22f), 0, 0);
        // magneticMoment = new Vector3(maxMagnitude / Mathf.Sqrt(2f), maxMagnitude / Mathf.Sqrt(2f), 0);
        // magneticMoment = new Vector3(maxMagnitude, 0, 0);
        // magneticMoment = new Vector3(0, 0, 0);
        // Debug.Log("mag mom2: " + magneticMoment);
        // Debug.Log("mag mag2: " + magneticMoment.magnitude);
        torus.magneticMoment = magneticMoment;

        // Calculate the angle between the magnetic moment and the position vector
        Vector3 cross = Vector3.Cross(Vector3.right.normalized, magneticMoment.normalized);
        rotationAngle = Mathf.Acos(Vector3.Dot(Vector3.right.normalized, magneticMoment.normalized));
        // Debug.Log("angle: " + rotationAngle * Mathf.Rad2Deg);

        float ellipseFactor = mapEllipseScale(minMagnitude, maxMagnitude, magneticMoment.magnitude);
        float ellipseRatio = 2f;
        int reflection = 1;
        int ellipseNum = 1;

        // generates 2x numEllipses
        for (int i = 1; i <= numEllipses; i++)
        {
            float semiMajorAxis = (0.75f * (i) + Mathf.Pow(2, i) / (i + 2)) / ellipseFactor;
            float semiMinorAxis = (0.75f * 0.75f * (i) + Mathf.Pow(2, i) / (i + 1)) / ellipseFactor / ellipseRatio;
            // Debug.Log("major: " + semiMajorAxis);
            // Debug.Log("minor: " + semiMinorAxis);
            Vector3 ellipseCenter = new Vector3(0, semiMinorAxis, 0);

            Ellipse ellipse = new Ellipse(semiMajorAxis, semiMinorAxis, ellipseCenter);

            createEllipse(ellipseNum, reflection, ellipse);

            // flips across x axis and resets to generate second half of ellipses
            if (i == 5 && reflection == 1)
            {
                i = 0;
                reflection = -1;
            }

            torus.getEllipses().Add(ellipse);
            ellipseNum++;
        }

        setScaleAndRotation(ellipseFactor);
        return torus;
    }

    private void createEllipse(int ellipseNum, int reflection, Ellipse ellipse)
    {
        Vector3[] points = new Vector3[numPoints];
        float angleStep = 360f / numPoints;
        float semiMajorAxis = ellipse.semiMajorAxis;
        float semiMinorAxis = ellipse.semiMinorAxis;
        Vector3 ellipseCenter = ellipse.center;
        List<(int, Vector3)> usablePoints = new List<(int, Vector3)>();

        // generates points to form ellipse
        for (int i = 0; i < numPoints; i++)
        {
            float angle = (i * angleStep) + 270;
            angle *= Mathf.Deg2Rad;

            float x = semiMajorAxis * Mathf.Cos(angle);
            float y = reflection * (semiMinorAxis * Mathf.Sin(angle) + ellipseCenter.y);

            float rotatedX = (x * Mathf.Cos(rotationAngle)) - (y * Mathf.Sin(rotationAngle));
            float rotatedY = (x * Mathf.Sin(rotationAngle)) + (y * Mathf.Cos(rotationAngle));

            points[i] = new Vector3(rotatedX, rotatedY, 0);

            if(points[i].magnitude > 1)
            {
                usablePoints.Add((i, points[i]));
            }
        }

        ellipse.usablePoints = usablePoints;
        drawEllipse(ellipseNum, points, ellipse);
    }

    private void drawEllipse(int ellipseNum, Vector3[] points, Ellipse ellipse)
    {
        // Create a new GameObject
        GameObject lineObject = new GameObject("Ellipse" + (ellipseNum - 1));

        // set ellipse gameobject as a child of torus gameobject
        lineObject.transform.SetParent(torusObject.transform, false);
        lineObject.transform.localScale = Vector3.one;

        // Attach LineRenderer component to the new GameObject
        LineRenderer lineRenderer = lineObject.AddComponent<LineRenderer>();

        // makes line renderer scale/rotate/transform with parent (torusObject)
        lineRenderer.useWorldSpace = false;

        // Assign the points to the LineRenderer
        lineRenderer.positionCount = points.Length;
        lineRenderer.SetPositions(points);

        // set material
        lineRenderer.material = new Material(Shader.Find("Legacy Shaders/Particles/Alpha Blended Premultiply"));

        // close loop
        lineRenderer.loop = true;

        // set color and width
        // lineRenderer.startColor = Color.red;
        // lineRenderer.endColor = Color.blue;
        applyGradient(lineRenderer, ellipseNum);
        lineRenderer.startWidth = 0.1f;
        lineRenderer.endWidth = 0.1f;

        ellipse.lineObject = lineObject;
    }

    private void applyGradient(LineRenderer lineRenderer, int ellipseNum)
    {
        // Create a static gradient
        Gradient staticGradient = new Gradient();

        float whiteWashValue = ((0.60f / 4) * ((ellipseNum-1) % (numEllipses/2)));
        // Debug.Log("ellipseNum: " + ellipseNum + " whiteWashValue: " + whiteWashValue);

        // Define color keys
        GradientColorKey[] colorKeys = new GradientColorKey[2];
        colorKeys[0].color = new Color(1, whiteWashValue, whiteWashValue, 1);
        colorKeys[0].time = 0.0f;
        colorKeys[0].time = 0.35f;
        colorKeys[1].color = new Color(whiteWashValue, whiteWashValue, 1, 1);
        colorKeys[1].time = 1.0f;
        colorKeys[1].time = 0.65f;

        // Define alpha keys (optional, here set to fully opaque)
        GradientAlphaKey[] alphaKeys = new GradientAlphaKey[2];
        alphaKeys[0].alpha = 1.0f;
        alphaKeys[0].time = 0.0f;
        alphaKeys[0].time = 0.35f;
        alphaKeys[1].alpha = 1.0f;
        alphaKeys[1].time = 1.0f;
        alphaKeys[1].time = 0.65f;

        // Assign color and alpha keys to the gradient
        staticGradient.SetKeys(colorKeys, alphaKeys);

        // Set the LineRenderer's color gradient
        lineRenderer.colorGradient = staticGradient;
    }

    private float mapEllipseScale(float magStrengthMin, float magStrengthMax, float magStrength)
    {
        // Calculate the input scale
        float inputScale = magStrengthMax - magStrengthMin;
        
        // Calculate the inverted proportion of magStrength within its range
        float invertedProportion = 1 - ((magStrength - magStrengthMin) / inputScale);
        
        // Calculate the output scale
        float outputScale = maxScale - minScale;
        
        // Apply the inverted proportion to the output range
        float ellipseScale = invertedProportion * outputScale + minScale;
        // Debug.Log("scale: " + ellipseScale);

        return ellipseScale;
    }

    private (float, float) mapTorusScaleRange(float ellipseScale)
    {
        // Define the output ranges for specific values
        float minScaleMin = 0.3f;
        float maxScaleMin = 0.55f;
        float minScaleMax = 1.0f;
        float maxScaleMax = 1.8f;

        // Calculate the percentage of the input value within the range
        float percentage = (ellipseScale - minScale) / (maxScale - minScale);

        // Calculate the mapped output range
        float mappedScaleMin = Mathf.Lerp(minScaleMin, maxScaleMin, percentage);
        float mappedScaleMax = Mathf.Lerp(minScaleMax, maxScaleMax, percentage);
        // Debug.Log("min: " + mappedScaleMin);
        // Debug.Log("max: " + mappedScaleMax);

        return (mappedScaleMin, mappedScaleMax);
    }

    private List<Vector3> generateMagneticMoments(float minMagnitude, float maxMagnitude)
    {
        List<Vector3> magMoments = new List<Vector3>();
        // Adding moments with max magnitude
        Vector3 maxXMoment = new Vector3(maxMagnitude, 0f, 0f);
        Vector3 maxYMoment = new Vector3(0f, maxMagnitude, 0f);
        magMoments.Add(maxXMoment);
        magMoments.Add(maxYMoment);

        float componentMaxMagnitude = maxMagnitude / Mathf.Sqrt(2f);

        // Adding moments with random magnitudes
        for (int i = 0; i < 8; i++)
        {
            // x component
            float magMomentMagnitude = Random.Range(minMagnitude, componentMaxMagnitude);
            int factor = Random.Range(-1, 1);
            float magXComp = factor * magMomentMagnitude;

            // y component
            magMomentMagnitude = Random.Range(minMagnitude, componentMaxMagnitude);
            factor = Random.Range(-1, 1);
            float magYComp = factor * magMomentMagnitude;

            Vector3 randomMoment = new Vector3(magXComp, magYComp, 0f);
            magMoments.Add(randomMoment);
        }

        return magMoments;
    }

    private void setScaleAndRotation(float ellipseFactor)
    {
        Transform t = torusObject.transform;
        (float, float) scaleRange = mapTorusScaleRange(ellipseFactor);

        int zRotation = Random.Range(0, 360);
        float scaleFactor = Random.Range(scaleRange.Item1, scaleRange.Item2); // keep torus in screen and bigger than Psyche

        Vector3 scale = new(scaleFactor, scaleFactor, scaleFactor);

        t.eulerAngles = new Vector3(0, 0, (float)zRotation);
        t.localScale = scale;
    }
}
