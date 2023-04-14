using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LaserTrail : MonoBehaviour
{
    public GameObject laser_panel;

    RaycastHit[] raycasts;
    Vector3 last_location;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    void AddTrail(Vector3 new_location)
    {
        //Vector3 direction = new_location - last_location;

        Instantiate(laser_panel, transform);
    }

    // Update is called once per frame
    void Update()
    {
        foreach (RaycastHit ray in raycasts)
        {
            if (ray.collider != null)
            {
                Debug.Log("HIT");
            }
        }
    }
}
