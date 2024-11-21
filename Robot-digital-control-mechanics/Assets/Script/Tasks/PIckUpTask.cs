using UnityEngine;

public class PIckUpTask : MonoBehaviour
{
    public GameObject cubeA; 
    private Rigidbody cubeRigidbody; 

    private bool isHoldingCube = false; 
    private bool canPickup = false; 
    private Vector3 collisionOffset; 
    private Transform originalParent; 

    private void Start()
    {
        cubeRigidbody = cubeA.GetComponent<Rigidbody>();
        originalParent = cubeA.transform.parent;
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            Debug.Log("Spacebar pressed");

            if (canPickup)
            {
                if (!isHoldingCube)
                {
                    AttachCube();
                }
                else
                {
                    DetachCube();
                }
            }
        }
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject == cubeA)
        {
            canPickup = true;
            // Calculate collision offset (local position of the collision relative to the robot arm)
            ContactPoint contact = collision.contacts[0]; // Get the first contact point
            collisionOffset = transform.InverseTransformPoint(contact.point);
            Debug.Log("cubeA is in range for pickup");
        }
    }

    private void OnCollisionExit(Collision collision)
    {
        if (collision.gameObject == cubeA)
        {
            canPickup = false;
        }
    }

    private void AttachCube()
    {
        Debug.Log("Picking up cubeA");
        cubeRigidbody.isKinematic = true;
        cubeA.transform.SetParent(transform);
        cubeA.transform.localPosition = collisionOffset;
        isHoldingCube = true;
        Debug.Log("isHoldingCube set to true");
    }

    private void DetachCube()
    {
        Debug.Log("Detaching cubeA");
        cubeRigidbody.isKinematic = false;
        isHoldingCube = false;
        canPickup = false;
        Debug.Log("isHoldingCube set to false");
        Debug.Log("cubeA detached");
    }
}
