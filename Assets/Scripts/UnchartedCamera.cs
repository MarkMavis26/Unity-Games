using UnityEngine;
using UnityEngine.EventSystems;

public class UnchartedCamera : MonoBehaviour
{
    public GameObject player;
    public float rotationSpeed = 20;
    void Start()
    {
        
    }

    void Update()
    {
        float horizontalInput = Input.GetAxis("Horizontal");
        float verticalInput = Input.GetAxis("Vertical");
        

        if(horizontalInput != 0f || verticalInput != 0f)
        {
            Vector3 movementDirection = player.GetComponent<Uncharted3rdPCont>().moveDirection;
            if(movementDirection != Vector3.zero)
            {
                Quaternion toRotation = Quaternion.LookRotation(movementDirection, Vector3.up);
                player.transform.rotation = Quaternion.Slerp(player.transform.rotation, toRotation, Time.deltaTime * rotationSpeed);
            }
        }
        
        /*
        //rotate orientation / direction
        Vector3 viewDirection = player.transform.position - new Vector3(transform.position.x, player.transform.position.y, transform.position.z);
        player.transform.forward = viewDirection.normalized;
        */
        
    }
}
