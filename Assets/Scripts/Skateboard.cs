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
    internal float jump_timer = 0f;

    public LayerMask beam_layer;

    public Camera1 camera_script;

    internal Vector3 direction;
    internal Rigidbody rb;

    public GameObject FloorController;

    RaycastHit[] raycasts;
    Vector3 last_location;

    // Controls
    public KeyCode right = KeyCode.RightArrow;
    public KeyCode left = KeyCode.LeftArrow;
    public KeyCode jump = KeyCode.Space;
    public KeyCode boost = KeyCode.X;

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
            if (Input.GetKeyUp(right))
            {
                Turn(90f);
            }
            if (Input.GetKeyUp(left))
            {
                Turn(-90f);
            }
        }
        else
        {
            if (Input.GetKey(right))
            {
                Turn(Time.deltaTime * turn_speed);
            }
            if (Input.GetKey(left))
            {
                Turn(-Time.deltaTime * turn_speed);
            }
        }

        if (Input.GetKeyUp(KeyCode.Space) && jump_timer <= 0)
        {
            jump_timer = 1.0f;
            rb.velocity = new Vector3(rb.velocity.x, jumpStrength, rb.velocity.z);
        }
        if (jump_timer > 0)
        {
            jump_timer -= Time.deltaTime;
        }

        rb.velocity = new Vector3(transform.forward.x * speed, rb.velocity.y, transform.forward.z * speed);

        if (Input.GetKey(boost))
        {
            speed = base_speed * 1.75f;
        }
        else
        {
            CastFloorRay(new Vector3(0.1f, 0, 0.1f));
            CastFloorRay(new Vector3(-0.1f, 0, 0.1f));
            CastFloorRay(new Vector3(0.1f, 0, -0.1f));
            CastFloorRay(new Vector3(-0.1f, 0, -0.1f));
            speed = base_speed;
        }

        if (Dies())
        {
            Debug.Log("Dead");
            Explode();
        }
    }


    void Explode()
    {
        var exp = GetComponentInChildren<ParticleSystem>();
        exp.Play();
        Destroy(gameObject, 2.0f);
    }


    bool Dies()
    {
        Vector3 origin = new Vector3(transform.position.x, transform.position.y + 0.1f, transform.position.z);
        Ray ray = new Ray(origin, transform.forward);
        Debug.DrawRay(origin, transform.forward);
        RaycastHit hit;
        if (Physics.Raycast(ray, out hit, .5f, beam_layer))
        {
            return true;
        }
        return false;
    }


    void CastFloorRay(Vector3 offset)
    {
        Vector3 origin = new Vector3(transform.position.x + offset.x, transform.position.y + offset.y, transform.position.z + offset.z);
        Ray ray = new Ray(origin, Vector3.down);
        Debug.DrawRay(origin, Vector3.down);
        RaycastHit hit;
        if (Physics.Raycast(ray, out hit, .5f, beam_layer))
        {
            FloorController.GetComponent<FloorController>().Add_Pin(hit.collider.transform, true);
        }
    }

    //private void OnCollisionExit(Collision collision)
    //{
    //    FloorController.GetComponent<FloorController>().Add_Pin(collision.transform);
    //}
}
