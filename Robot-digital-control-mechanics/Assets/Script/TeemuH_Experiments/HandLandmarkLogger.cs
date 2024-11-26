using UnityEngine;

public class HandLandmarkLogger : MonoBehaviour
{
    void Update()
    {
        // Find the parent object that contains the point annotations
        GameObject parentObject = GameObject.Find("Point List Annotation");

        if (parentObject == null)
        {
            Debug.LogError("Parent object not found!");
            return;
        }

        // Loop through each child object (Point Annotation(Clone)) under the parent
        for (int i = 0; i < parentObject.transform.childCount; i++)
        {
            Transform child = parentObject.transform.GetChild(i);

            // Check if the child object is active before processing
            if (child.gameObject.activeSelf)
            {
                // Log local position with axis details
                Vector3 localPosition = child.localPosition;
                Debug.Log($"Index {i}: Local Position X = {localPosition.x}, Y = {localPosition.y}, Z = {localPosition.z}");

                // Optionally log world position for comparison (commented out)
                // Vector3 worldPosition = child.position;
                // Debug.Log($"Index {i}: World Position X = {worldPosition.x}, Y = {worldPosition.y}, Z = {worldPosition.z}");
            }
            else
            {
                Debug.Log($"Index {i}: Object is inactive.");
            }
        }
    }
}
