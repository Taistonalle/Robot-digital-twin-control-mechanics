using UnityEngine;
[RequireComponent(typeof(Rigidbody))]
public class MazeStartingArea : MonoBehaviour {
    //When arm leaves the starting area, start timer and destroy this object.
    private void OnTriggerExit(Collider other) {
        if (other.gameObject.CompareTag("RobotArm")) {
            FindFirstObjectByType<MazeTask>().StartTimer();
            Destroy(gameObject);
        }
    }
}