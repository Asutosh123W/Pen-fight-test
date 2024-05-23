using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PenCollision : MonoBehaviour
{
    void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Pen"))
        {
            // Handle collision logic (e.g., reduce health)
        }
    }
}
