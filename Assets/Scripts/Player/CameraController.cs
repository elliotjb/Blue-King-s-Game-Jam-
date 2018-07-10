using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour
{
    public float offset_x = 20.0f;
    public float speed = 1.0f;

    Vector3 actual_target;
    public float target_pos_x = 0.0f;
    public float target_pos_y = 0.0f;
    public bool moving = false;


    // Use this for initialization
    void Start ()
    {
		
	}
	
	// Update is called once per frame
	void Update ()
    {

    }

    private void LateUpdate()
    {
        //Move the camera to the next position
        if (moving == true && transform.position.x < target_pos_x) 
        {
            transform.position = Vector3.Lerp(transform.position, actual_target, speed * Time.deltaTime);
            //transform.Translate(Time.deltaTime * speed, 0, 0); 
        }

        else if(moving == true && transform.position.x >= target_pos_x)
        {
            transform.position.Set(target_pos_x, target_pos_y, transform.position.z);
            moving = false;
        }
    }

    public void MoveCamera(Transform target)
    {
        moving = true;
        target_pos_x = target.position.x + offset_x;
        target_pos_y = transform.position.y;
        actual_target.Set(target_pos_x, target_pos_y, transform.position.z);       
    }
}
