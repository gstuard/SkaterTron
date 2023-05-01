// <copyright file="CameraController.cs" company="DIS Copenhagen">
using UnityEngine;
using System.Collections;

public class Camera1 : MonoBehaviour
{
    public GameObject target;

    public LayerMask obstacleLayerMask;

    public float rotationSpeed;

    public float max_height;
    private float height;
    public float max_distance;
    private float distance;

    public bool targetIsAlive;

    //public float max_pitch;
    //private float pitch;
    //private float yaw = 0;

    void Start()
    {
        distance = max_distance;
        height = max_height;
        targetIsAlive = true;
    }

    void Update()
    {
        if (distance < max_distance && height < max_height)
        {
            distance = Mathf.MoveTowards(distance, max_distance, .2f);
            height = Mathf.MoveTowards(height, max_height, .15f);
            // I want the camera to pan down here but it has been more trouble than it is worth rn...
        }

        // this code is redundant rn but it works
        
        transform.rotation = Quaternion.Slerp(transform.rotation, target.transform.rotation, rotationSpeed * Time.deltaTime);
        transform.position = target.transform.position - transform.forward * distance;
        transform.position = new Vector3(transform.position.x, transform.position.y + height, transform.position.z);

        //if (!targetIsAlive)
        //{
        //    if (distance < max_distance)
        //    {
        //        transform.LookAt(target.transform);
        //    }
        //}
    }
}
