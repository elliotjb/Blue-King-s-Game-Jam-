using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BackgroundScript : MonoBehaviour
{
    public GameObject other_bckg;

    private float offset;
	// Use this for initialization
	void Start () {
        offset = Mathf.Abs(transform.position.x - other_bckg.transform.position.x);
	}
	
	// Update is called once per frame
	void Update ()
    {
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            Debug.Log("hi");
            other_bckg.transform.position = new Vector3(transform.position.x + offset, transform.position.y, transform.position.z);
        }
    }
}
