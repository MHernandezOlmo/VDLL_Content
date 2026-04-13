using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Splines;

public class SplineFollower : MonoBehaviour
{
    [Header("Splines")]
    public List<SplineContainer> splineContainers = new List<SplineContainer>();

    [Header("Movement")]
    public float speed = 0.5f;

    [Range(0f, 1f)]
    public float t = 0f;

    private int currentSplineIndex = 0;
    private bool isMoving = false;

    private void Start()
    {
        PlaceAtCurrentSplineStart();
    }

    private void Update()
    {
        if (splineContainers == null || splineContainers.Count == 0)
            return;

        if (Input.GetKeyDown(KeyCode.Space))
        {
            if (!isMoving)
            {
                StartCurrentSpline();
            }
        }

        if (!isMoving)
            return;

        SplineContainer currentSpline = splineContainers[currentSplineIndex];
        if (currentSpline == null)
            return;

        t += speed * Time.deltaTime;

        if (t >= 1f)
        {
            t = 1f;
            UpdateTransform(currentSpline);
            isMoving = false;

            currentSplineIndex++;
            if (currentSplineIndex >= splineContainers.Count)
                currentSplineIndex = splineContainers.Count - 1;

            return;
        }

        UpdateTransform(currentSpline);
    }

    private void StartCurrentSpline()
    {
        if (currentSplineIndex >= splineContainers.Count)
            return;

        t = 0f;
        isMoving = true;
        PlaceAtCurrentSplineStart();
    }

    private void PlaceAtCurrentSplineStart()
    {
        if (currentSplineIndex >= splineContainers.Count)
            return;

        SplineContainer currentSpline = splineContainers[currentSplineIndex];
        if (currentSpline == null)
            return;

        Vector3 position = currentSpline.EvaluatePosition(0f);
        Vector3 forward = currentSpline.EvaluateTangent(0f);

        transform.position = position;

        if (forward.sqrMagnitude > 0.0001f)
            transform.rotation = Quaternion.LookRotation(forward);
    }

    private void UpdateTransform(SplineContainer spline)
    {
        Vector3 position = spline.EvaluatePosition(t);
        Vector3 forward = spline.EvaluateTangent(t);

        transform.position = position;

        if (forward.sqrMagnitude > 0.0001f)
            transform.rotation = Quaternion.LookRotation(forward);
    }
}