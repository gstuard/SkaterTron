using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Sweeper : MonoBehaviour
{
    public float speed;
    public float pin_height;

    public bool isActive;

    public float startx, endx;

    //public ScriptableObject script // the var that restarts the game.

    // Start is called before the first frame update
    void Start()
    {
        isActive = false;
    }

    // Update is called once per frame
    void Update()
    {
        if (isActive)
        {
            transform.position = new Vector3(transform.position.x + speed * Time.deltaTime, transform.position.y, transform.position.z);
            if (transform.position.x > endx)
            {
                transform.position = new Vector3(startx, transform.position.y, transform.position.z);
                isActive = false;
                // call a method that restarts the game!
            }
        }
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Beam"))
        {
            collision.transform.position = new Vector3(collision.transform.position.x, pin_height, collision.transform.position.z);
        }
        else if (collision.gameObject.CompareTag("Player"))
        {
            collision.gameObject.GetComponent<Skateboard>().Spawn(20f);
        }
    }
}
