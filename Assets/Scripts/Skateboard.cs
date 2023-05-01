using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

public class Skateboard : MonoBehaviourPunCallbacks
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

    public GameObject FloorController;

    RaycastHit[] raycasts;
    Vector3 last_location;

    //NEW VARIABLES *-------------------------------------*
    public Transform viewPoint;

    public float moveSpeed = 5f, runSpeed = 8f;
    private float activeMoveSpeed;
    private Vector3 moveDir, movement;

    public CharacterController charCon;

    private Camera cam;

    public float jumpForce = 7.5f, gravityMod = 2.5f;

    public bool isGrounded;
    public LayerMask groundLayers;

    //NEW VARIABLES END *-------------------------------------*


    // Controls
    public KeyCode right = KeyCode.RightArrow;
    public KeyCode left = KeyCode.LeftArrow;
    public KeyCode jump = KeyCode.Space;

    // Start is called before the first frame update
    void Start()
    {
        speed = base_speed;

        cam = Camera.main;

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
        /*
            //turns
            if (Input.GetKey(right))
             {
                 Turn(Time.deltaTime * turn_speed);
             }
             if (Input.GetKey(left))
             {
                 Turn(-Time.deltaTime * turn_speed);
             }
        */
        //NEW CODE *---------------------------------------------------*
        if (photonView.IsMine)
        {
            moveDir = new Vector3(Input.GetAxisRaw("Horizontal"), 0, Input.GetAxisRaw("Vertical"));

            if (Input.GetKey(KeyCode.LeftShift))            //Checks if player is pressing left shift for sprint

            {
               activeMoveSpeed = runSpeed;
            }
            else                                            //Else normal movement speed
            {
                activeMoveSpeed = moveSpeed;
            }


            float yVel = movement.y;
            movement = (transform.forward) * activeMoveSpeed;
            movement.y = yVel;


            transform.rotation = Quaternion.Euler(transform.rotation.eulerAngles.x, transform.rotation.eulerAngles.y + (moveDir.x * 2), transform.rotation.eulerAngles.z);


            if (charCon.isGrounded)
            {
                movement.y = 0f;
            }

            if (Input.GetButtonDown("Jump") && charCon.isGrounded)
            {
                movement.y = jumpForce;
            }

            if (Input.GetKey(KeyCode.LeftShift))
            {
                activeMoveSpeed = runSpeed;
            }
            else
            {
                CastFloorRay(new Vector3(0.1f, 0, 0.1f));
                CastFloorRay(new Vector3(-0.1f, 0, 0.1f));
                CastFloorRay(new Vector3(0.1f, 0, -0.1f));
                CastFloorRay(new Vector3(-0.1f, 0, -0.1f));
                activeMoveSpeed = moveSpeed;
            }


        //Dies Function
        if (Dies())
        {
            Debug.Log("Dead");
            Explode();
        }

        movement.y += Physics.gravity.y * Time.deltaTime * gravityMod;

        charCon.Move(movement * Time.deltaTime);
    }

        //NEW CODE *---------------------------------------------------*  
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
        if (Physics.Raycast(ray, out hit, .6f, beam_layer))
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
        if (Physics.Raycast(ray, out hit, 3.5f, beam_layer))
        {
            FloorController.GetComponent<FloorController>().Add_Pin(hit.collider.transform, true);
        }
    }

    //private void OnCollisionExit(Collision collision)
    //{
    //    FloorController.GetComponent<FloorController>().Add_Pin(collision.transform);
    //}

    private void LateUpdate()
    {
        if (photonView.IsMine)
        {
            cam.transform.position = viewPoint.position;
            cam.transform.rotation = viewPoint.rotation;
        }
    }
}
