using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

public class Skateboard : MonoBehaviourPunCallbacks
{
    public float base_speed;
    internal float speed;
    public float jumpStrength;
    internal float jump_timer = 0f;

    public LayerMask beam_layer;


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

    public bool isGrounded; // GCom: should be private?
    public LayerMask groundLayers;

    //NEWER VARIABLES *-------------------------------------*

    private bool isAlive = true;
    public float turnFactor;

    private Material material;

    public Vector3 starting_location;
    private float spawn_timer = 0;

    //NEW/NEWER VARIABLES END *-------------------------------------*


    // Controls
    public KeyCode right = KeyCode.RightArrow;
    public KeyCode left = KeyCode.LeftArrow;
    public KeyCode jump = KeyCode.Space;

    // Start is called before the first frame update
    void Start()
    {
        material = GetComponent<Material>();
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


    public void Spawn(float time)
    {
        spawn_timer = time;
        isAlive = true;
        transform.position = starting_location;
        //camera_script.transform.LookAt(GameObject.Find("InvisibleFloor").transform);
        // add a way to face the right way? juice GCom:
        //camera_script.ResetView();
    }


    // Update is called once per frame
    void Update()
    {
    
        //NEW CODE *---------------------------------------------------*
        if (photonView.IsMine)
        {
            spawn_timer -= Time.deltaTime;
            if (Input.GetButtonDown("Jump"))
            {
                spawn_timer = 0;
            }

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

            transform.rotation = Quaternion.Euler(transform.rotation.eulerAngles.x, transform.rotation.eulerAngles.y + (moveDir.x * turnFactor), transform.rotation.eulerAngles.z);

            //GCom: do we need a ray here? or do we want to use floor ray for that maybe?
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

            movement.y += Physics.gravity.y * Time.deltaTime * gravityMod;

            if (isAlive && spawn_timer <= 0)
            {
                CheckDeath();
                charCon.Move(movement * Time.deltaTime);
            }
        }

        //NEW CODE *---------------------------------------------------*  
    }


    void ExplodeAnimation()
    {
        var exp = GetComponentInChildren<ParticleSystem>();
        exp.Play();
        //camera_script.max_distance *= 3f;
        //camera_script.max_height *= 2f;
        //camera_script.targetIsAlive = false;

        material.SetFloat("_Alpha", 0.5f); // WHY ISNT THIS WORKING GCom
    }


    void CheckDeath()
    {
        Vector3 origin = new Vector3(transform.position.x, transform.position.y + 0.1f, transform.position.z);
        Ray ray = new Ray(origin, transform.forward);
        Debug.DrawRay(origin, transform.forward);
        RaycastHit hit;
        if (Physics.Raycast(ray, out hit, .6f, beam_layer))
        {
            isAlive = false;
            ExplodeAnimation();
            //GameObject.Find("Sweeper").GetComponent<Sweeper>().isActive = true;// this is not working rn but that is fine
        }
        isAlive = true;
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
