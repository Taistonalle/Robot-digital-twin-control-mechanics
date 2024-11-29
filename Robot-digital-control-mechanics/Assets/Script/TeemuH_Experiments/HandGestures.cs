using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class HandGestures: MonoBehaviour
{
    bool stopped = true;

    [Header("Values")]
    [SerializeField][Range(0.1f, 5f)] float moveAmount;
    [SerializeField][Range(0.1f, 1f)] float loopWaitTime;

    [Header("Sliders")]
    [SerializeField] Slider X_pos_slider;

    private float previousZ = 0f; // Store the Z value of index 0 from the previous frame

    void Update()
    {
        // Find the parent object that contains the point annotations
        GameObject parentObject = GameObject.Find("Point List Annotation");

        if (parentObject == null)
        {
            Debug.LogError("Parent object not found!");
            return;
        }

        // Ensure index 0 exists
        if (parentObject.transform.childCount == 0)
        {
            Debug.LogError("No landmarks found!");
            return;
        }

        Transform landmark0 = parentObject.transform.GetChild(0);

        if (landmark0.gameObject.activeSelf)
        {
            Vector3 localPosition = landmark0.localPosition;
            Debug.Log($"Index 0: Local Position X = {localPosition.x}, Y = {localPosition.y}, Z = {localPosition.z}");

            // Check if Z value is increasing or decreasing
            if (localPosition.z > previousZ)
            {
                if (!stopped) StopRoutine();
                X_Up(); // Trigger X_Up function
            }
            else if (localPosition.z < previousZ)
            {
                if (!stopped) StopRoutine();
                X_Down(); // Trigger X_Down function
            }

            // Update previousZ for the next frame
            previousZ = localPosition.z;
        }
        else
        {
            Debug.Log("Index 0 is inactive.");
        }
    }

    // Trigger X_Up routine
    void X_Up()
    {
        stopped = false;
        StartCoroutine(X_Up_Routine());
    }

    // Trigger X_Down routine
    void X_Down()
    {
        stopped = false;
        StartCoroutine(X_Down_Routine());
    }

    IEnumerator X_Up_Routine()
    {
        while (!stopped)
        {
            X_pos_slider.value += moveAmount;
            yield return new WaitForSeconds(loopWaitTime);
        }
    }

    IEnumerator X_Down_Routine()
    {
        while (!stopped)
        {
            X_pos_slider.value -= moveAmount;
            yield return new WaitForSeconds(loopWaitTime);
        }
    }

    public void StopRoutine()
    {
        stopped = true;
        StopAllCoroutines();
    }
}