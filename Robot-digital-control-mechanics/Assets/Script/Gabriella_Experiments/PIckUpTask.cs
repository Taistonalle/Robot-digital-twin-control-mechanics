using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class PIckUpTask : MonoBehaviour
{
    public GameObject cubeA;
    public GameObject cubeB;
    private Rigidbody cubeRigidbody;
    private float distanceToCubeB;
    private bool isHoldingCube = false;
    private bool canPickup = false;
    private Vector3 collisionOffset;
    private Transform originalParent;
    public TMP_Text distanceText;
    public Button toggleDistanceButton;
    private bool showDistance = false;

    private void Start()
    {
        cubeRigidbody = cubeA.GetComponent<Rigidbody>();
        originalParent = cubeA.transform.parent;
        distanceText.text = "";  
        showDistance = false;
        if (toggleDistanceButton != null)
        {
            toggleDistanceButton.onClick.AddListener(ToggleDistanceText);
        }
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            if (!isHoldingCube && canPickup)
            {
                AttachCube();
            }
            else if (isHoldingCube)
            {
                DetachCube();
            }
            if (showDistance && distanceText.text != "")
            {
                CheckDistance();
            }
        }
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject == cubeA)
        {
            canPickup = true;
            ContactPoint contact = collision.contacts[0];
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
        canPickup = true;
    }

    private void DetachCube()
    {
        cubeA.transform.SetParent(originalParent != null ? originalParent : null);
        cubeRigidbody.isKinematic = false;
        cubeRigidbody.linearVelocity = Vector3.zero;
        cubeRigidbody.angularVelocity = Vector3.zero;
        isHoldingCube = false;
        canPickup = false;
        Debug.Log("cubeA detached");
    }

    private void CheckDistance()
    {
        if (cubeA != null && cubeB != null)
        {
            distanceToCubeB = Vector3.Distance(cubeA.transform.position, cubeB.transform.position);

            float distanceInCm = distanceToCubeB * 100f;

            distanceText.text = $"Distance to target: {distanceInCm:F2} cm";
        }
    }

    private void ToggleDistanceText()
    {
        showDistance = !showDistance; 

        if (showDistance)
        {
            CheckDistance();
        }
        else
        {
            distanceText.text = "";  // Clear the text
        }
    }
}
