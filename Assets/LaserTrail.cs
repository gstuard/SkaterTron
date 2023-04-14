using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LaserTrail : MonoBehaviour
{

    RaycastHit[] raycasts;
    Vector3[] locations;
    Vector3 last_location;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    void AddTrail(Vector3 new_location)
    {
        //locations 

        Vector3 direction = new_location - last_location;

        //Ray new_ray = Physics.Raycast(last_location, direction, Vector3.Distance(new_location, last_location));
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
