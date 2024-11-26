using UnityEngine;

public class HandLandmarkLoggerAdvanced : MonoBehaviour
{
    void Update()
    {
        // Parent object containing the points
        GameObject pointParent = GameObject.Find("Point List Annotation");
        if (pointParent == null)
        {
            Debug.LogError("Point List Annotation not found!");
            return;
        }

        // Parent object containing the connections
        GameObject connectionParent = GameObject.Find("Connection List Annotation");
        if (connectionParent == null)
        {
            Debug.LogError("Connection List Annotation not found!");
            return;
        }

        // Log points
        Debug.Log("=== Logging Points ===");
        Vector3[] points = new Vector3[pointParent.transform.childCount];
        for (int i = 0; i < pointParent.transform.childCount; i++)
        {
            Transform child = pointParent.transform.GetChild(i);

            if (child.gameObject.activeSelf)
            {
                points[i] = child.position; // Store the world position of the point
                Debug.Log($"Index {i}: World Position {points[i]}");
            }
            else
            {
                Debug.Log($"Index {i}: Object is inactive.");
            }
        }

        // Log connections
        Debug.Log("=== Logging Connections ===");
        for (int i = 0; i < connectionParent.transform.childCount; i++)
        {
            Transform connection = connectionParent.transform.GetChild(i);

            if (connection.gameObject.activeSelf)
            {
                // Assuming the connection stores indices of the two connected points
                int startIndex = i; // Replace with logic to get the start index
                int endIndex = i + 1; // Replace with logic to get the end index

                // Log the start and end positions of the connection
                Debug.Log($"Connection {i}: Start {points[startIndex]} -> End {points[endIndex]}");
            }
        }
    }
}
