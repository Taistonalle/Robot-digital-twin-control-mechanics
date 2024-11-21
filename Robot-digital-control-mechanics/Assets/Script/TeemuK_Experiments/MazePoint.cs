using UnityEngine;
[RequireComponent(typeof(Rigidbody))]
public class MazePoint : MonoBehaviour {
    [SerializeField] GameObject nextPointToActivate;

    //Add point visited on MazeTask script, activate next point and destroy this object
    private void OnTriggerEnter(Collider other) {
        if (other.gameObject.CompareTag("RobotArm")) {
            FindFirstObjectByType<MazeTask>().AddVisitedPoint();
            ActivateNextPoint(nextPointToActivate);
            Destroy(gameObject);
        }
    }

    void ActivateNextPoint(GameObject point) {
        if (point == null) return;
        point.gameObject.SetActive(true);
    }
}