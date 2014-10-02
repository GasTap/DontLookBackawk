using UnityEngine;
using System.Collections;

public class PlayerScript : MonoBehaviour
{

    float jumpPow = 10f;
    float flyPow = 3f;
    float horSpeed = 1f;
    bool grounded = true;

    // Use this for initialization
    void Start()
    {
        
    }
    
    // Update is called once per frame
    void Update()
    {
        RaycastHit2D ray = Physics2D.Raycast(transform.position - Vector3.up * 0.6f, -Vector2.up, 0.1f);
        if (ray.collider != null)
        {
            if (ray.collider.gameObject.tag == "Platform")
            {
                grounded = true;
            } 
        } else
        {
            grounded = false;
        }
        if (Input.GetKeyDown(KeyCode.Space))
        {
            if (grounded)
            {
                Debug.Log("Jumping");
                Vector2 newVelocity = new Vector2(rigidbody2D.velocity.x, jumpPow);
                rigidbody2D.velocity = newVelocity;
            } else
            {
                Debug.Log("Flapping");
                Vector2 newVelocity = new Vector2(rigidbody2D.velocity.x, (rigidbody2D.velocity.y + flyPow <= 0) ? rigidbody2D.velocity.y + flyPow : 0);
                rigidbody2D.velocity = newVelocity;
            }
        }
        if (Input.GetKey(KeyCode.LeftArrow))
        {
            Debug.Log("MOVING");
            Vector2 newVelocity = new Vector2(-horSpeed, rigidbody2D.velocity.y);
            rigidbody2D.velocity = newVelocity;
        }
        if (Input.GetKey(KeyCode.RightArrow))
        {
            Debug.Log("MOVING");
            Vector2 newVelocity = new Vector2(horSpeed, rigidbody2D.velocity.y);
            rigidbody2D.velocity = newVelocity;
        }
    }
}
