// <copyright file="CameraController.cs" company="DIS Copenhagen">
using UnityEngine;
using System.Collections;

public class Camera1 : MonoBehaviour
{
    public Transform target;

    public LayerMask obstacleLayerMask;

    public float rotationSpeed;

    public float max_distance;
    public float minPitch;
    public float maxPitch;

    private float distance;
    private float pitch;
    private float yaw;

    void Start()
    {
        pitch = 45;
        yaw = 0;
        distance = max_distance;
    }

    void Update()
    {
        transform.rotation = Quaternion.Slerp(transform.rotation, target.rotation, rotationSpeed * Time.deltaTime);
        transform.position = target.position - transform.forward * distance;
        transform.position = new Vector3(transform.position.x, transform.position.y + 1.5f, transform.position.z);
    }
}
