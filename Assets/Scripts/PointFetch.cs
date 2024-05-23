using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PointFetch : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            RaycastHit hit;

            if (Physics.Raycast(ray, out hit))
            {
                // Check if the hit collider is the capsule collider
                if (hit.collider is CapsuleCollider)
                {
                    // Fetch the point on the capsule collider
                    Vector3 pointOnCapsule = GetPointOnCapsule(hit.collider as CapsuleCollider, hit.point);
                    Debug.Log("Point on capsule: " + pointOnCapsule);
                }
            }
        }
    }


    Vector3 GetPointOnCapsule(CapsuleCollider capsuleCollider, Vector3 point)
    {
        // Transform the hit point into the local space of the capsule
        Vector3 localPoint = capsuleCollider.transform.InverseTransformPoint(point);

        // Calculate the closest point on the capsule surface using the local point
        Vector3 closestPoint = ClosestPointOnCapsule(localPoint, capsuleCollider.radius, capsuleCollider.height);

        // Transform the closest point back to the world space
        Vector3 pointOnCapsule = capsuleCollider.transform.TransformPoint(closestPoint);

        return pointOnCapsule;
    }


    Vector3 ClosestPointOnCapsule(Vector3 localPoint, float radius, float height)
    {
        float halfHeight = height * 0.5f;

        // Clamp the local point to the height of the capsule
        localPoint.y = Mathf.Clamp(localPoint.y, -halfHeight, halfHeight);

        // Calculate the closest point on the infinite cylinder part of the capsule
        Vector3 closestPoint = new Vector3(localPoint.x, localPoint.y, 0f);
        closestPoint = closestPoint.normalized * radius;

        // Adjust the closest point to account for the capsule end caps
        if (localPoint.y > halfHeight - radius)
        {
            closestPoint.y = halfHeight;
        }
        else if (localPoint.y < -halfHeight + radius)
        {
            closestPoint.y = -halfHeight;
        }

        return closestPoint;
    }

}
