using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TargetIndicator : MonoBehaviour
{
    // MOVEMENT VARIABLES 
    public Transform target;
    [SerializeField] Vector3 direction;
    [SerializeField] float angle;

    // PLAYER VARIABLES
    public Player player;

    // Arrow Image
    public GameObject arrow;


    // Use this for initialization
    void Start ()
    {

	}
	
	// Update is called once per frame
	void Update ()
    {
        if (player.GetState() == StatePlayer.IN_NODE)
        {
            arrow.SetActive(true);
        }
        else
        {
            if(arrow.activeInHierarchy)
            {
                arrow.SetActive(false);
            }
        }
    }

    public void SetTarget(Transform new_target)
    {
        target = new_target;
        direction = target.position - transform.position;
        angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
        transform.rotation = Quaternion.AngleAxis(angle, Vector3.forward);
    }
}
