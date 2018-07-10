using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum ColorType
{
    ORANGE_COLOR = 0,
    GREEN_COLOR,
    PINK_COLOR
}
public class ColorBall : MonoBehaviour {

    public List<GameObject> boles;

    public float speed1 = 438.14f;
    public Vector3 axis1;
    public Vector3 axis2;
    public Vector3 axis3;

    private bool orange_color = false;
    private bool green_color = false;
    private bool pink_color = false;


    // Use this for initialization
    void Start()
    {
        axis1 = new Vector3(1, 1, 0);
        axis2 = new Vector3(-1, 1, 0);
        axis3 = new Vector3(0, 1, 0);

        boles[0].SetActive(false);
        boles[1].SetActive(false);
        boles[2].SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {
        if (green_color)
        {
            boles[0].transform.RotateAround(transform.position, axis1, speed1 * Time.deltaTime);
        }
        if (orange_color)
        { 
            boles[1].transform.RotateAround(transform.position, axis2, speed1 * Time.deltaTime);
        }
        if (pink_color)
        {
            boles[2].transform.RotateAround(transform.position, axis3, speed1* Time.deltaTime);
        }
    }

    public void ActivateColor(ColorType type)
    {
        if (type == ColorType.GREEN_COLOR)
        {
            green_color = true;
            boles[0].SetActive(true);
        }
        if (type == ColorType.ORANGE_COLOR)
        {
            orange_color = true;
            boles[1].SetActive(true);
        }
        if (type == ColorType.PINK_COLOR)
        {
            pink_color = true;
            boles[2].SetActive(true);
        }
    }

    public void DesactivateColor(ColorType type)
    {
        if (type == ColorType.GREEN_COLOR)
        {
            green_color = false;
            boles[0].SetActive(false);
        }
        if (type == ColorType.ORANGE_COLOR)
        {
            orange_color = false;
            boles[1].SetActive(false);
        }
        if (type == ColorType.PINK_COLOR)
        {
            pink_color = false;
            boles[2].SetActive(false);
        }
    }
}
