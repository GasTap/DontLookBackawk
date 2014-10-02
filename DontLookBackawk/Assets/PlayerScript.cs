using UnityEngine;
using System.Collections;

public class PlayerScript : MonoBehaviour
{

    float jumpPow = 2f;
    float flyPow = 1.5f;
    bool grounded = true;

    // Use this for initialization
    void Start()
    {
        
    }
    
    // Update is called once per frame
    void Update()
    {
        RaycastHit2D ray = Physics2D.Raycast(transform.position - Vector3.up*0.6f, -Vector2.up, 0.1f);
        if (ray.collider != null)
        {
            if (ray.collider.gameObject.tag == "Platform")
            {
                Debug.Log("Grounded");
                grounded = true;
            } 
        } else
        {
            grounded = false;
        }
        if (Input.GetKeyDown(KeyCode.Space))
        {
            if(grounded)
            {
                Debug.Log("Jumping");
                Vector2 newVelocity = new Vector2(rigidbody2D.velocity.x, jumpPow * Time.deltaTime);
            } else
            {

            }
        }
    }
}
