using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerTorque : MonoBehaviour
{
    Camera mainCamera;

    void Start()
    {
        mainCamera = Camera.main;
    }

    void Update()
    {
        // Check for mouse click
        if (Input.GetMouseButtonDown(0))
        {

            // Cast a ray from the mouse position into the scene
            Ray ray = mainCamera.ScreenPointToRay(Input.mousePosition);
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

        if (Input.GetMouseButtonUp(0))
        {

            // Cast a ray from the mouse position into the scene
            Ray ray = mainCamera.ScreenPointToRay(Input.mousePosition);
            RaycastHit hit;

            // Check if the ray hits a collider
            if (Physics.Raycast(ray, out hit))
            {
                // Check if the hit collider is the capsule or box collider
                if (hit.collider == GetComponent<MeshCollider>())
                {
                    // Get the point of intersection on the capsule collider
                    Vector3 pointOnBox = hit.point;
                    Debug.Log("Point on Box: " + pointOnBox);
                }

            }
        }

    }
}
