// <copyright file="CameraController.cs" company="DIS Copenhagen">
using UnityEngine;
using System.Collections;

public class Camera1 : MonoBehaviour
{
    public GameObject target;

    public LayerMask obstacleLayerMask;

    public float rotationSpeed;

    public float max_distance;
    public float minPitch;
    public float maxPitch;

    private float distance;
    private float pitch = 45;
    private float yaw = 0;

    void Start()
    {
        distance = max_distance;
    }

    void Update()
    {
        transform.rotation = Quaternion.Slerp(transform.rotation, target.transform.rotation, rotationSpeed * Time.deltaTime);
        transform.position = target.transform.position - transform.forward * distance;
        transform.position = new Vector3(transform.position.x, transform.position.y + 1.5f, transform.position.z);
    }
}
