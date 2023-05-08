using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using Photon.Realtime;

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

    //public GameObject Opponent;

    //NEW/NEWER VARIABLES END *-------------------------------------*

    // Boost Settings
    public UIBoost boostPanel;
    public int maxBoost = 50;
    public int boostUse = 20;
    public int boostRefresh = 10;
    public int refreshWait = 1;
    private float currentBoost;
    private bool refreshing = false;

    // Score UI
    public UIScore scorePanel;
    private int wins = 0;

    // Controls
    public KeyCode right = KeyCode.RightArrow;
    public KeyCode left = KeyCode.LeftArrow;
    public KeyCode jump = KeyCode.Space;
    public KeyCode boost = KeyCode.LeftShift;

    public string[] whosLeft = new string[8];

    // Start is called before the first frame update
    void Start()
    {
        material = GetComponent<Material>();
        speed = base_speed;
        currentBoost = maxBoost;

        cam = Camera.main;

        direction = Vector3.forward;

        FloorController = GameObject.Find("Floor Controller"); // fixed floor not working on spawn

        Player[] players = PhotonNetwork.PlayerList;

        for (int i = 0; i < players.Length; i++)
        {
            whosLeft[i] = players[i].NickName;
        }
        //GameObject[] Opponents = GameObject.FindGameObjectsWithTag("Player");
        //if (Opponents[0] == this)
        //{
        //    Opponent = Opponents[1];
        //}
        //else
        //{
        //    Opponent = Opponents[0];
        //}
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


    // Fixed Update
    void FixedUpdate()
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

            if (Input.GetKey(boost))            //Checks if player is pressing left shift for sprint
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

            if (Input.GetKey(boost) && currentBoost > 0)
            {
                activeMoveSpeed = runSpeed;
                currentBoost -= boostUse * Time.deltaTime;
                refreshing = false;
                StopCoroutine(BoostRecharge());
            }
            else if (Input.GetKeyUp(boost) && !refreshing)
            {
                StartCoroutine(BoostRecharge());
            }
            else
            {
                CastFloorRay(new Vector3(0.1f, 0, 0.1f));
                CastFloorRay(new Vector3(-0.1f, 0, 0.1f));
                CastFloorRay(new Vector3(0.1f, 0, -0.1f));
                CastFloorRay(new Vector3(-0.1f, 0, -0.1f));

                activeMoveSpeed = moveSpeed;
            }

            // Update UI (might move so it's not necessarilly updating every frame, only when it changes
            //boostPanel.SetBoost(maxBoost, (int)currentBoost);
            // Here is where would update score once that's implemented :P
            // Ideally only update it when a match finishes
            // scorePanel.UpdateWins(wins);

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
        Ray rayCenter = new Ray(origin, transform.forward);
        Ray rayRight = new Ray(origin + new Vector3 (.1f,0f,0f), transform.forward);
        Ray rayLeft = new Ray(origin + new Vector3(-.1f, 0f, 0f), transform.forward);
        Debug.DrawRay(origin, transform.forward);
        Debug.DrawRay(origin + new Vector3(.1f, 0f, 0f), transform.forward);
        Debug.DrawRay(origin + new Vector3(-.1f, 0f, 0f), transform.forward);
        RaycastHit hitCenter;
        RaycastHit hitLeft;
        RaycastHit hitRight;
        if (Physics.Raycast(rayCenter, out hitCenter, .6f, beam_layer) || Physics.Raycast(rayLeft, out hitLeft, .6f, beam_layer) || Physics.Raycast(rayRight, out hitRight, .6f, beam_layer))
        {
            isAlive = false;
            ExplodeAnimation();
            photonView.RPC("RemoveFromWhosLeft", RpcTarget.All);

            //GameObject.Find("Sweeper").GetComponent<Sweeper>().isActive = true;// this is not working rn but that is fine
        }
        isAlive = true;
    }


    void CastFloorRay(Vector3 offset)
    {
        Vector3 origin = new Vector3(transform.position.x + offset.x, transform.position.y + offset.y, transform.position.z + offset.z);
        Ray ray = new Ray(origin, Vector3.down);
        //Debug.DrawRay(origin, Vector3.down);
        RaycastHit hit;
        if (Physics.Raycast(ray, out hit, 3.5f, beam_layer))
        {
            Vector3 location = hit.collider.transform.position;
            FloorController.GetPhotonView().RPC("TestPunRPC", RpcTarget.All, location);

            //FloorController.GetComponent<FloorController>().Add_Pin(hit.collider.transform);

            //Opponent.GetComponent<Skateboard>().Add_Pin(hit.collider.transform, true);
        }
    }


    [PunRPC]
    public void RemoveFromWhosLeft()
    {
        string winner = "";
        int counter = 0;

        for(int i = 0; i < whosLeft.Length; i++)
        {
            if (PhotonNetwork.NickName == whosLeft[i])
            {
                whosLeft[i] = "";
            }
        }

        for (int i = 0; i < whosLeft.Length; i++)
        {
            if (whosLeft[i] != "")
            {
                counter += 1;
            }
        }

        if(counter < 2)
        {
            for (int i = 0; i < whosLeft.Length; i++)
            {
                if (whosLeft[i] != "")
                {
                    winner = whosLeft[i];
                }
            }


            Debug.Log(winner + "YEAHHHH");
            //winner is the string
            //HEREHEREHERE
        }

    }

        public void Add_Pin(Transform new_pin, bool is_red)
    {
        FloorController.GetComponent<FloorController>().Add_Pin(new_pin, is_red);
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

    IEnumerator BoostRecharge()
    {
        refreshing = true;
        yield return new WaitForSeconds(refreshWait);
        while (refreshing)
        {
            currentBoost += boostRefresh * Time.deltaTime;
            if (currentBoost >= maxBoost)
            {
                currentBoost = maxBoost;
                refreshing = false;
            }
            yield return null;
        }
    }
}
