using Alteruna;
using UnityEngine;

public class LineForce1 : MonoBehaviour
{
    [SerializeField] private float shotPower = 10f;
    [SerializeField] private float maxStretchDistance = 5f; // Maximum stretching distance allowed
    [SerializeField] private LineRenderer lineRenderer;
    [SerializeField] private Rigidbody rb;
    [SerializeField] AnimationCurve ac;
     GameObject P;
    [SerializeField] GameObject P1;
    [SerializeField] GameObject P2;


    private bool point1 = true;
    private bool point2 = false;
    private bool isAiming = false;
    private Vector3 aimTarget;
    private Vector2 swipeStartPos;
    private bool swipeInProgress = false;
    public bool player2chance = false;
    private LineForce lf;
    Vector3 EulerAngleVelocity;
    public float rotationSpeed;
    private Alteruna.Avatar _avatar;

    private void Start()
    {
        _avatar = GetComponent<Alteruna.Avatar>();

        if (!_avatar.IsMe)
            return;



        lineRenderer.enabled = false;
        lf = FindObjectOfType<LineForce>();
        EulerAngleVelocity = new Vector3(0, 0, 0);
        P = GameObject.FindGameObjectWithTag("corner3");
    }

    void Rotate(float rotationSpeed)
    {
        Quaternion deltaRotation = Quaternion.Euler(EulerAngleVelocity);
        Quaternion targetRotation = rb.rotation * deltaRotation;

        // Interpolate between the current rotation and the target rotation
        Quaternion newRotation = Quaternion.Lerp(rb.rotation, targetRotation, rotationSpeed);

        // Apply the new rotation to the Rigidbody
        rb.MoveRotation(newRotation);
    }

    private void Update()
    {

        if (!_avatar.IsMe)
            return;



        if (Input.GetMouseButtonDown(0) && !isAiming && player2chance)
        {
            Point();
            StartAiming();
        }

        if (isAiming)
        {
            UpdateAiming();
        }
      

        
        
    }

    private void StartAiming()
    {
        isAiming = true;
        lineRenderer.enabled = true;
        lineRenderer.SetPosition(0, P.transform.position);
        aimTarget = GetMouseWorldPosition();
        swipeStartPos = Input.mousePosition;
        swipeInProgress = true;
    }

    private void UpdateAiming()
    {
        aimTarget = GetMouseWorldPosition();

        // Calculate the direction and distance from the pen's current position to the mouse cursor
        Vector3 direction = (aimTarget - P.transform.position).normalized;
        float distance = Vector3.Distance(P.transform.position, aimTarget);

        // Clamp the distance to the maximum stretch distance
        if (distance > maxStretchDistance)
        {
            aimTarget = P.transform.position + direction * maxStretchDistance;
            distance = maxStretchDistance; // Update the distance value to the clamped distance
        }

        lineRenderer.SetPosition(1, aimTarget);

        if (Input.GetMouseButtonUp(0) && swipeInProgress)
        {
            Shoot();
            EndAiming();
            if (point1)
            {
                PointChange();
                point1 = false;
                point2 = true;
            }
            else if (point2)
            {
                PointChange();
                point2 = false;
                point1 = true;
            }

            player2chance = false;
            lf.player1chance = true;
        }
    }

    private void PointChange()
    {
        if (point1)
        {
            P = GameObject.FindGameObjectWithTag("corner2");

        }
        else if (point2)
        {
            P = GameObject.FindGameObjectWithTag("corner3");
        }

    }

    private void Shoot()
    {
        isAiming = false;
        lineRenderer.enabled = false;

        Vector3 mouseClickPosition = GetMouseWorldPosition();
        Vector3 offset = mouseClickPosition - P.transform.position;

        // Apply force at the offset point, considering offset direction
        rb.AddForceAtPosition(offset.normalized * shotPower, mouseClickPosition, ForceMode.Impulse);

        // Apply torque to introduce spin based on offset
        Vector3 torqueDirection = Vector3.Cross(offset, Vector3.forward); // Adjust as needed for desired spin
        rb.AddTorque(P.transform.position * shotPower * 0.01f); // Adjust 0.5f for torque intensity

        Rotate(rotationSpeed);


    }
    private void EndAiming()
    {
        isAiming = false;
        lineRenderer.enabled = false;
        swipeInProgress = false;

        // Calculate swipe direction
        Vector2 swipeDirection = (Vector2)Input.mousePosition - swipeStartPos;

        // Ensure swipe is predominantly horizontal (adjust threshold as needed)
        if (Mathf.Abs(swipeDirection.x) > Mathf.Abs(swipeDirection.y))
        {
            // Horizontal swipe detected
            Vector3 direction = (aimTarget - P.transform.position).normalized;
            float strength = Mathf.Clamp(swipeDirection.magnitude, 0f, maxStretchDistance);

            rb.AddForce(direction * strength * shotPower, ForceMode.Impulse);
            rb.AddTorque(Vector3.forward); // Example torque (adjust as needed)
        }
    }

    private Vector3 Point()
    {
        // Check for mouse click
        if (Input.GetMouseButtonDown(0))
        {
            // Cast a ray from the mouse position into the scene
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            RaycastHit hit;

            // Check if the ray hits a collider
            if (Physics.Raycast(ray, out hit))
            {
                // Check if the hit collider is the capsule or box collider
                if (hit.collider == GetComponent<CapsuleCollider>())
                {
                    // Get the point of intersection on the capsule collider
                    Vector3 pointOnCapsule = hit.point;
                    Debug.Log("Point on capsule: " + pointOnCapsule);
                    
                }

            }

        }

        

        return Vector3.zero;

    }


    private Vector3 GetMouseWorldPosition()
    {
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        Plane plane = new Plane(Vector3.up, P.transform.position);
        float distance;

        if (plane.Raycast(ray, out distance))
        {
            return ray.GetPoint(distance);
        }

        return Vector3.zero;
    }
}