using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

public class FloorController : MonoBehaviourPunCallbacks
{
    public bool floor_active;

    public Sweeper sweeper;

    public static int MaximumRisingPins = 320;
    public static int MaximumBufferedPins = 40;

    public float pin_speed;
    public float pin_height;
    public float pin_low_height;

    internal Transform[] pins = new Transform[MaximumRisingPins];
    internal Transform[] pin_buffer = new Transform[MaximumBufferedPins];
    internal int buffer_index = 0;
    internal int array_index = 0;

    public Material red;
    public Material blue;

    // Start is called before the first frame update
    void Start()
    {

    }

    //[PunRPC]
    //public void TestPunRPC(int instanceID)
    //{
    //    Debug.Log("Works across network!" + instanceID);
        
    //}


    //[PunRPC]
    public void Add_Pin(Transform new_pin, bool is_red = true)
    {
        //if (is_red)
        //{
        //    transform.GetComponent<Renderer>().sharedMaterial = red;
        //}
        if (array_index >= MaximumRisingPins)
        {
            array_index = 0;
        }
        if (buffer_index >= MaximumBufferedPins)
        {
            buffer_index = 0;
        }
        pins[array_index++] = pin_buffer[buffer_index];
        pin_buffer[buffer_index++] = new_pin;
    }


    // Update is called once per frame
    void Update()
    {
        if (floor_active)
        {
            for (int pin_index = 0; pin_index < MaximumRisingPins; pin_index++)
            {
                if (pins[pin_index] == null || pins[pin_index].position.y >= pin_height)
                {
                    // skip
                }
                else
                {
                    // pins[pin_index].GetComponent<Renderer>().sharedmaterial = red; // OR blue, somehow
                    float new_y = Mathf.MoveTowards(pins[pin_index].position.y, pin_height, pin_speed);
                    pins[pin_index].position = new Vector3(pins[pin_index].position.x, new_y, pins[pin_index].position.z);
                }
            }
            if (Input.GetKeyUp(KeyCode.R))
            {
                ResetGame();
            }
        }
    }


    void ResetGame()
    {
        sweeper.isActive = true;
        pins = new Transform[MaximumRisingPins];
        pin_buffer = new Transform[MaximumBufferedPins];
    }


    void ClearMap()
    {
        int blocks = transform.childCount;
        int chunks = 64;
        int beams = 100;

        //Transform block1 = GetComponentInChildren<Transform>();
        //int chunks = block1.childCount;

        //Transform chunk1 = GetComponentInChildren<Transform>();
        //int beams = chunk1.childCount;

        for (int block_index = 0; block_index < blocks; block_index++)
        {
            for (int chunk_index = 0; chunk_index < chunks; chunk_index++)
            {
                for (int beam_index = 0; beam_index < beams; beam_index++)
                {
                    // might use this?
                }
            }
        }

    }
}
