using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FloorController : MonoBehaviour
{
    public bool floor_active;

    public static int MaximumRisingPins = 320;
    public static int MaximumBufferedPins = 24;

    public float pin_speed;
    public float pin_height;
    public float pin_low_height;

    internal Transform[] pins = new Transform[MaximumRisingPins];
    internal Transform[] pin_buffer = new Transform[MaximumBufferedPins];
    internal int buffer_index = 0;
    internal int array_index = 0;


    // Start is called before the first frame update
    void Start()
    {
        
    }

    public void Add_Pin(Transform new_pin)
    {
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
                    float new_y = pins[pin_index].position.y + pin_speed * Time.deltaTime;
                    pins[pin_index].position = new Vector3(pins[pin_index].position.x, new_y, pins[pin_index].position.z);
                }
            }
        }
    }
}
