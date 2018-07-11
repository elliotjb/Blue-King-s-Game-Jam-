using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObstacleScript : MonoBehaviour {

    public Vector3 nodePosition;
    public float Angle = 10.0f;
    public float LastAngle = 0.0f;
    public bool leaving = false;
    private AnimationCurve speed_curve = null;
    private AnimationCurve offset_curve = null;
    private float offset_to_node = 0.0f;

    private float time = 0.0f;
    // Use this for initialization
    void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
        float speed_rotation = Angle;
        if(speed_curve!=null)
        {
            speed_rotation *= speed_curve.Evaluate(time);
            time += Time.deltaTime;
        }
        transform.RotateAround(nodePosition, Vector3.forward, speed_rotation * Time.deltaTime );


        if(offset_curve!=null)
        {

        }
        //if(Input.GetKeyDown(KeyCode.R) == true)
        //{
        //    GetComponent<Animator>().SetBool("Leave", true);
        //}
        //speed_curve.

    }
    public void SetAngle(float angle, AnimationCurve curve_value = null)
    {
        Angle = angle;
        speed_curve = curve_value;
    }
    public void SetOffset(float offset, AnimationCurve curve_value = null)
    {
        offset_to_node = offset;
        offset_curve = curve_value;
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
