using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Movement : MonoBehaviour
{
    public Rigidbody2D rb;
    public float velocity = 3;
    
    public Camera cam; //reference to the camera

    Vector2 mousePos; //Vector for the mouse position
    Vector2 movement;

    // Start is called before the first frame update
    void Start()
    {
      rb =  GetComponent<Rigidbody2D>();
    }
    private void Update()
    {
        movement.y = Mathf.Lerp(movement.y, Input.GetAxis("Vertical"),0.2f); //Gets axis for vertical input.
        movement.x = Mathf.Lerp(movement.x, Input.GetAxis("Horizontal"), 0.2f);  //Gets axis for horizontal input.


       mousePos = cam.ScreenToWorldPoint(Input.mousePosition); //makes the mouse position relative to the screen in game.

    }
    // Update is called once per frame
    void FixedUpdate()
    {
        rb.MovePosition(rb.position + movement * velocity * Time.fixedDeltaTime); //rotates the rigidbody with relation to the mouse.
        Vector2 lookDirection = mousePos - rb.position;
        float angle = Mathf.LerpAngle(rb.rotation, Mathf.Atan2(lookDirection.y, lookDirection.x) * Mathf.Rad2Deg - 90f, 0.2f); //calculates the angle to adjust the body towards the mosue position.
        rb.rotation = angle;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        Debug.Log("Chat");
    }
}
