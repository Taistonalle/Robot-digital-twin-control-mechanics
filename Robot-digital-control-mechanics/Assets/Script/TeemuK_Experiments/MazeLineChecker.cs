using UnityEngine;

public class MazeLineChecker : MonoBehaviour {
    [SerializeField] LineRenderer mazeLineRenderer;
    [SerializeField] Material[] lineMaterials;

    private void OnTriggerEnter(Collider other) {
        if (other.CompareTag("MazeLine")) {
            mazeLineRenderer.material = lineMaterials[0]; //Green colour
            FindFirstObjectByType<MazeTask>().StopLineTimer();
        }
    }

    private void OnTriggerExit(Collider other) {
        if (other.CompareTag("MazeLine")) {
            mazeLineRenderer.material = lineMaterials[1]; //Red colour
            FindFirstObjectByType<MazeTask>().StartLineTimer();
        }
    }
}
