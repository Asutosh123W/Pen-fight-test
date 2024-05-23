using UnityEngine;

public class Player2Movement : MonoBehaviour
{
    public float moveSpeed = 5f; 

    void Start()
    {
        
    }

    void Update()
    {
        
        float horizontalInput = 0f;
        float verticalInput = 0f;

        
        if (Input.GetKey(KeyCode.RightArrow))
        {
            horizontalInput = 1f; 
        }
        else if (Input.GetKey(KeyCode.LeftArrow))
        {
            horizontalInput = -1f; 
        }

        if (Input.GetKey(KeyCode.UpArrow))
        {
            verticalInput = 1f; 
        }
        else if (Input.GetKey(KeyCode.DownArrow))
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
