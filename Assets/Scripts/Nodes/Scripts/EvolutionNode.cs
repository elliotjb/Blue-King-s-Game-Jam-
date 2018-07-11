using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EvolutionNode : MonoBehaviour
{
    public bool used = false;
	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Player")&&isActiveAndEnabled&& !used)
        {
            //Show Evolution Menu
            other.gameObject.GetComponent<Player>().ActiveEvoProvider();
            GetComponent<MeshRenderer>().enabled = false;
            used = true;
        }
    }
}
