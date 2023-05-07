using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MainMenuDrift : MonoBehaviour
{
    public int xDrift;
    public int yDrift;
    public int zDrift;

    // Update is called once per frame
    void Update()
    {
        transform.Rotate(new Vector3(xDrift, yDrift, zDrift) * Time.deltaTime);
    }
}