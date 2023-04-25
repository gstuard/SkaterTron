using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Skateboard : MonoBehaviour
{
    public bool sharp_turns;

    public float base_speed;
    internal float speed;
    public float turn_speed;
    public float jumpStrength;

    public Camera1 camera_script;

    internal Vector3 direction;
    internal Rigidbody rb;

    public GameObject FloorController;

    RaycastHit[] raycasts;
    Vector3 last_location;

    // Start is called before the first frame update
    void Start()
    {
        speed = base_speed;
        rb = GetComponent<Rigidbody>();
        direction = Vector3.forward;
    }

    //void AddTrail(Vector3 new_location)
    //{
    //    Instantiate(laser_panel, transform);
    //}

    void Turn(float turn_degrees)
    {
        transform.Rotate(0, turn_degrees, 0);
        direction = Quaternion.AngleAxis(turn_degrees, Vector3.up) * direction;

    }

    // Update is called once per frame
    void Update()
    {
        if (sharp_turns)
        {
            if (Input.GetKeyUp(KeyCode.RightArrow))
            {
                Turn(90f);
            }
            if (Input.GetKeyUp(KeyCode.LeftArrow))
            {
                Turn(-90f);
            }
        }
        else
        {
            if (Input.GetKey(KeyCode.RightArrow))
            {
                Turn(Time.deltaTime * turn_speed);
            }
            if (Input.GetKey(KeyCode.LeftArrow))
            {
                Turn(-Time.deltaTime * turn_speed);
            }
        }

        if (Input.GetKeyUp(KeyCode.Space))
        {
            rb.velocity = new Vector3(rb.velocity.x, jumpStrength, rb.velocity.z);
        }

        //if (Input.GetKeyDown(KeyCode.UpArrow))
        //{
        //    direction = new Vector3(0, 0, 1);
        //}
        //if (Input.GetKeyDown(KeyCode.DownArrow))
        //{
        //    direction = new Vector3(0, 0, -1);
        //}

        rb.velocity = new Vector3(transform.forward.x * speed, rb.velocity.y, transform.forward.z * speed);
    }
    private void OnCollisionExit(Collision collision)
    {
        FloorController.GetComponent<FloorController>().Add_Pin(collision.transform);
    }
}
