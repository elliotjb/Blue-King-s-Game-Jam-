using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObstacleScript : MonoBehaviour {

    public Vector3 nodePosition;
    public float Angle = 10.0f;
    public float LastAngle = 0.0f;
    public bool leaving = false;
	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
        transform.RotateAround(nodePosition, Vector3.forward, Angle * Time.deltaTime );
        //if(Input.GetKeyDown(KeyCode.R) == true)
        //{
        //    GetComponent<Animator>().SetBool("Leave", true);
        //}
	}

    public void Explode()
    {
        GetComponent<Animator>().SetBool("Die", true);
    }

    public void Leave()
    {
        GetComponent<Animator>().SetBool("Leave", true);
        leaving = true;
    }
   
}
