using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    public float moveSpeed = 5f; 

    void Start()
    {

    }

    void Update()
    {
       
        float horizontalInput = 0f;
        float verticalInput = 0f;

        
        if (Input.GetKey(KeyCode.D))
        {
            horizontalInput = 1f; 
        }
        else if (Input.GetKey(KeyCode.A))
        {
            horizontalInput = -1f; 
        }

        if (Input.GetKey(KeyCode.W))
        {
            verticalInput = 1f; 
        }
        else if (Input.GetKey(KeyCode.S))
        {
            verticalInput = -1f; 
        }

        
        Vector3 movement = new Vector3(horizontalInput, 0f, verticalInput);

       
        if (movement != Vector3.zero)
        {
            movement.Normalize();
        }

     
        Vector3 newPosition = transform.position + movement * moveSpeed * Time.deltaTime;

        
        transform.position = newPosition;
    }
}
